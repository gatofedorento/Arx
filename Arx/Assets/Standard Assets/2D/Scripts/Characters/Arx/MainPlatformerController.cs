﻿using CommonInterfaces.Controllers;
using CommonInterfaces.Weapons;
using Extensions;
using GenericComponents.Behaviours;
using GenericComponents.Controllers.Characters;
using GenericComponents.Enums;
using GenericComponents.StateMachine;
using MathHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using CommonInterfaces.Enums;
using Assets.Standard_Assets._2D.Scripts.Characters.Arx.StateMachine;
using Assets.Standard_Assets._2D.Scripts.Characters.Arx;
using ArxGame.Components.Weapons;
using ArxGame.Components.Environment;

[RequireComponent(typeof(CombatModule))]
public class MainPlatformerController : PlatformerCharacterController, IPlatformerCharacterController
{
    private CombatModule _combatModule;
    private StateManager<IPlatformerCharacterController, PlatformerCharacterAction> _stateManager;

    private Rope _rope;
    private RopePart _currentRopePart;

    [SerializeField]
    private float _rollingDuration = 1;
    [SerializeField]
    private float _maxRopeHorizontalForce = 20;
    [SerializeField]
    private float _ropeVerticalSpeed = 4;
    [SerializeField]
    private float _grappleRopeGrabHeightOffset = -6;

    private float _move;
    private float _vertical;
    private bool _jump;
    private bool _roll;
    private bool _releaseRope;
    private bool _aiming;
    private bool _shoot;
    private bool _throw;

    private AttackType _attackAction;

    public StateManager<IPlatformerCharacterController, PlatformerCharacterAction> StateManager
    {
        get
        {
            return _stateManager;
        }
    }

    public ICloseCombatWeapon CloseCombatWeapon
    {
        get
        {
            return _combatModule.CloseCombatWeapon;
        }
        set
        {
            _combatModule.CloseCombatWeapon = value;
        }
    }

    public IShooterWeapon ShooterWeapon
    {
        get
        {
            return _combatModule.ShooterWeapon;
        }
        set
        {
            _combatModule.ShooterWeapon = value;
        }
    }

    public ChainThrow ChainThrowWeapon
    {
        get
        {
            return _combatModule.ChainThrowWeapon;
        }
        set
        {
            _combatModule.ChainThrowWeapon = value;
        }
    }

    public bool IsAttackOver { get; private set; }

    public bool Attacking { get; private set; }

    public int ComboNumber
    {
        get
        {
            return _combatModule.ComboNumber;
        }
    }

    public WeaponType? WeaponType
    {
        get
        {
            return _combatModule.WeaponType;
        }
    }

    public float RollingDuration
    {
        get
        {
            return _rollingDuration;
        }
    }

    public bool IsCharging { get; private set; }

    public float AimAngle { get; set; }

    public bool RopeFound { get { return _rope != null; } }

    public float RopeClimbDirection { get; private set; }

    public GrappleRope GrappleRope { get { return _combatModule.GrappleRope; } }

    public void Move(float move, float vertical, bool jump, bool roll, bool releaseRope, bool aiming)
    {
        _move = move;
        _vertical = vertical;
        _jump = jump;
        _roll = roll;
        _releaseRope = releaseRope;
        _aiming = aiming;
    }

    public void LightAttack()
    {
        _attackAction = AttackType.Primary;
    }

    public void StrongAttack()
    {
        _attackAction = AttackType.Secundary;
    }

    public void ChargeAttack()
    {
        IsCharging = true;
        _attackAction = AttackType.Primary;
    }

    public void ReleaseChargeAttack()
    {
        IsCharging = false;
        _attackAction = AttackType.None;
    }

    public void Shoot()
    {
        _shoot = true;
    }

    public void DoPrimaryGroundAttack()
    {
        Attacking = _combatModule.PrimaryGroundAttack();
        IsAttackOver = !Attacking;
        _attackAction = AttackType.None;
    }

    public void DoSecundaryGroundAttack()
    {
        Attacking = _combatModule.SecundaryGroundAttack();
        IsAttackOver = !Attacking;
        _attackAction = AttackType.None;
    }

    public void DoPrimaryAirAttack()
    {
        Attacking = _combatModule.PrimaryAirAttack();
        IsAttackOver = !Attacking;
        _attackAction = AttackType.None;
    }

    public void DoSecundaryAirAttack()
    {
        Attacking = _combatModule.SecundaryAirAttack();
        IsAttackOver = !Attacking;
        _attackAction = AttackType.None;
    }

    public void DoChargeAttack()
    {
        Attacking = _combatModule.ChargeAttack();
        IsAttackOver = !Attacking;
        _attackAction = AttackType.None;
    }

    public void DoReleaseChargeAttack()
    {
        _combatModule.ReleaseChargeAttack();
    }

    public override void Duck()
    {
        _combatModule.ComboNumber = 0;
        Attacking = false;
        base.Duck();
    }

    public override void Stand()
    {
        _combatModule.ComboNumber = 0;
        Attacking = false;
        base.Stand();
    }

    public void StartIddle() { }
    public void StopIddle() { }

    public void FlipToSlideDownDirection()
    {
        if(CharacterController2D.SlopeNormal.x == 0)
        {
            return;
        }
        Flip(
            CharacterController2D.SlopeNormal.x > 0 
            ? CommonInterfaces.Enums.Direction.Right 
            : CommonInterfaces.Enums.Direction.Left);
    }

    public void AirSlash()
    {
        VelocityMultiplier = new Vector3(VelocityMultiplier.x, VelocityMultiplier.y * 4f);
        _combatModule.StartDiveAttack();
    }

    public void StopAirSlash()
    {
        VelocityMultiplier = Vector2.one;
        _combatModule.EndDiveAttack();
        OnAttackFinishHandler();
    }

    public void GrabRope()
    {
        if (_rope == null)
        {
            return;
        }

        ApplyMovementAndGravity = false;
        SteadyRotation = false;
        _currentRopePart = _rope.GetRopePartAt(this.transform.position);
        var p = _rope.GetRopePartRigidBodyAt(this.transform.position);
        this.gameObject.transform.parent = _currentRopePart.transform;
    }

    public void LetGoRope()
    {
        this.gameObject.transform.parent = null;
        _currentRopePart = null;
        _rope = null;
        ApplyMovementAndGravity = true;
        SteadyRotation = true;
    }

    public void MoveOnRope(float horizontal, float vertical)
    {
        RopeClimbDirection = 0;

        var closestSegment = _rope.GetClosestRopeSegment(this.transform.position);
        this.gameObject.transform.parent = _currentRopePart.transform;
        this.gameObject.transform.position =
            FloatUtils.ClosestPointOnLine(closestSegment.Value.P1, closestSegment.Value.P2, this.transform.position);
        this.gameObject.transform.rotation = _currentRopePart.transform.rotation;

        if (Mathf.Abs(vertical) > 0.01)
        {
            var move = new Vector3(0, _ropeVerticalSpeed * Time.deltaTime * Mathf.Sign(vertical));
            this.transform.localPosition += move;
            RopeClimbDirection = vertical > 0 ? 1 : -1;
        }
        if (Mathf.Abs(horizontal) > 0.01)
        {
            _currentRopePart.PhysicsRopePart.AddForce(new Vector2(_maxRopeHorizontalForce * Math.Sign(horizontal), 0));
        }
    }

    public void DoAimingMove(float move)
    {
        base.DoMove(move, false);
    }

    public void SetDirectionToAimDirection()
    {
        if((AimAngle >= 0 && AimAngle <= 90) || (AimAngle <= 360 && AimAngle >= 270))
        {
            Flip(Direction.Right);
        }
        else
        {
            Flip(Direction.Left);
        }
    }

    public void DoShoot()
    {
        _combatModule.Shoot();
    }

    public void Aim(bool aim)
    {
        _combatModule.Aiming = aim;
    }

    public void Throw()
    {
        _throw = true;
    }

    public void DoThrow()
    {
        _combatModule.Throw();
        IsAttackOver = false;
    }

    public void GrabGrapple()
    {
        ApplyMovementAndGravity = false;
        SteadyRotation = false;
        DetectPlatform = false;
        // ChainThrowCombatBehaviour does the bellow line, 
        //but we still need to do it here, since it may be invalided by hit detection parenting
        this.transform.parent = _combatModule.GrappleRope.RopeEnd.gameObject.transform;
        this.transform.localPosition = new Vector3(0, _grappleRopeGrabHeightOffset);
    }

    public void MoveOnGrapple(float horizontal, float vertical)
    {
        if (Mathf.Abs(horizontal) > 0.01)
        {
            var body = _combatModule.GrappleRope.RopeEnd.GetComponent<Rigidbody2D>();
            body.AddForce(new Vector2(_maxRopeHorizontalForce * Math.Sign(horizontal), 0));
        }
        if (Mathf.Abs(vertical) > 0.01)
        {
            _combatModule.ClimbGrapple(vertical, _ropeVerticalSpeed);
            //var move = new Vector3(0, _ropeVerticalSpeed * Time.deltaTime * Mathf.Sign(vertical));
            //this.transform.localPosition += move;
        }
    }

    public void ReleaseGrapple()
    {
        _combatModule.ReleaseGrapple();
        this.transform.localRotation = Quaternion.identity;
        ApplyMovementAndGravity = true;
        SteadyRotation = true;
        DetectPlatform = true;
    }

    protected override void Awake()
    {
        base.Awake();
        _combatModule = GetComponent<CombatModule>();
        _stateManager = new PlatformerCharacterStateManager(this, _rollingDuration);
        _combatModule.OnAttackFinish += OnAttackFinishHandler;
        CharacterController2D.onTriggerEnterEvent += OnTriggerEnterEventHandler;
    }

    protected override void Update()
    {
        base.Update();

        _combatModule.AimAngle = AimAngle;
        var action = new PlatformerCharacterAction(_move, _vertical, _jump, _roll, _attackAction, _releaseRope, _aiming, _shoot, _throw);
        _stateManager.Perform(action);
        _move = 0;
        _vertical = 0;
        _jump = false;
        _roll = false;
        _aiming = false;
        _shoot = false;
        _throw = false;
    }

    private void OnAttackFinishHandler()
    {
        IsAttackOver = true;
    }

    private void OnTriggerEnterEventHandler(Collider2D collider)
    {
        if (!collider.IsTouching(CharacterController2D.BoxCollider2D))
        {
            return;
        }

        var rope = collider.gameObject.GetComponent<Rope>();
        if (rope == null)
        {
            return;
        }

        _rope = rope;
    }
}
