﻿using UnityEngine;
using System.Collections;
using GenericComponents.Enums;
using ArxGame.Components;
using CommonInterfaces.Controllers;
using System;
using Assets.Standard_Assets._2D.Scripts.Characters.Arx;
using Assets.Standard_Assets._2D.Scripts.Combat;
using Assets.Standard_Assets.Weapons;
using Assets.Standard_Assets._2D.Scripts.Controllers;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(MainPlatformerController))]
[RequireComponent(typeof(CombatModule))]
[RequireComponent(typeof(EnemyProximityDetector))]
[RequireComponent(typeof(AimBehaviour))]
[RequireComponent(typeof(ChainThrowCombatBehaviour))]
public class ArxAnimationController : MonoBehaviour
{
    private readonly int _HorizontalVelocity = Animator.StringToHash("Horizontal Velocity");
    private readonly int _Grounded = Animator.StringToHash("Grounded");
    private readonly int _Ducking = Animator.StringToHash("Ducking");
    private readonly int _LedgeGrabbing = Animator.StringToHash("Ledge Grabbing");
    private readonly int _VerticalVelocity = Animator.StringToHash("Vertical Velocity");
    private readonly int _Attacking = Animator.StringToHash("Attacking");
    private readonly int _ComboCount = Animator.StringToHash("Combo Count");
    private readonly int _WeaponType = Animator.StringToHash("Weapon Type");
    private readonly int _AttackType = Animator.StringToHash("Attack Type");
    private readonly int _AttackStyle = Animator.StringToHash("Attack Style");
    private readonly int _SlidingDown = Animator.StringToHash("Sliding Down");
    private readonly int _ChargingAttack = Animator.StringToHash("Charging Attack");
    private readonly int _RollingState = Animator.StringToHash("Base Layer.Ducking locomotion.Roll");
    private readonly int DashState = Animator.StringToHash("Base Layer.Grounded.Dash");
    private readonly int _GrabbingRope = Animator.StringToHash("Grabbing Rope"); 
    private readonly int _RopeClimbDirection = Animator.StringToHash("Rope Climb Direction");
    private readonly int _VelocityGoingDown = Animator.StringToHash("Velocity Going Down");
    private readonly int _EnemyNearby = Animator.StringToHash("Enemy Nearby");
    private readonly int _TakingDamage = Animator.StringToHash("Taking Damage");
    private readonly int _GrabbingLadder = Animator.StringToHash("Grabbing Ladder");
    private readonly int Aiming = Animator.StringToHash("Aiming");
    private readonly int Shoot = Animator.StringToHash("Shoot");
    private readonly int Throw = Animator.StringToHash("Throw");
    private readonly int ChainThrustComplete = Animator.StringToHash("Chain Thrust Complete");
    private readonly int LowKicking = Animator.StringToHash("Low Kicking");
    private readonly int StingDash = Animator.StringToHash("Sting Dash");
    private readonly int SprintJump = Animator.StringToHash("Sprint Jump");
    private readonly int Dash = Animator.StringToHash("Dash");
    private readonly int Pushing = Animator.StringToHash("Pushing");
    private readonly int WallDrag = Animator.StringToHash("Wall Drag");
    private readonly int AttackIndex = Animator.StringToHash("Attack Index");

    private float _previousVerticalVelocity = 0;

    private Animator _animator;
    private MainPlatformerController _platformerController;
    private CombatModule _combatModule;
    private EnemyProximityDetector _enemyProximityDetector;
    private AimBehaviour _aimBehaviour;
    private ChainThrowCombatBehaviour _chainThrow;

    public float HorizontalVelocity
    {
        set
        {
            _animator.SetFloat(_HorizontalVelocity, Mathf.Abs(value));
        }
    }
    private bool Grounded
    {
        set
        {
            _animator.SetBool(_Grounded, value);
        }
    }
    private bool Ducking
    {
        set
        {
            _animator.SetBool(_Ducking, value);
        }
    }
    private bool LedgeGrabbing
    {
        set
        {
            _animator.SetBool(_LedgeGrabbing, value);
        }
    }
    private float VerticalVelocity
    {
        set
        {
            _animator.SetFloat(_VerticalVelocity, value);
        }
    }
    private bool Attacking
    {
        set
        {
            _animator.SetBool(_Attacking, value);
        }
    }
    private int ComboCount
    {
        set
        {
            _animator.SetInteger(_ComboCount, value);
        }
    }
    private WeaponType? WeaponType
    {
        set
        {
            var val = value != null ? (int)value : -1;
            _animator.SetInteger(_WeaponType, val);
        }
    }
    private AttackType AttackType
    {
        set
        {
            _animator.SetInteger(_AttackType, (int)value);
        }
    }
    private AttackStyle AttackStyle
    {
        set
        {
            _animator.SetInteger(_AttackStyle, (int)value);
        }
    }
    private bool SlidingDown
    {
        set
        {
            _animator.SetBool(_SlidingDown, value);
        }
    }
    private bool ChargingAttack
    {
        set
        {
            _animator.SetBool(_ChargingAttack, value);
        }
    }
    private bool GrabbingRope
    {
        set
        {
            _animator.SetBool(_GrabbingRope, value);
        }
    }
    private float RopeClimbDirection
    {
        set
        {
            _animator.SetFloat(_RopeClimbDirection, value);
        }
    }
    private bool VelocityGoingDown
    {
        set
        {
            _animator.SetBool(_VelocityGoingDown, value);
        }
    }
    private bool EnemyNearby
    {
        set
        {
            _animator.SetBool(_EnemyNearby, value);
        }
    }
    private bool TakingDamage
    {
        set
        {
            _animator.SetBool(_TakingDamage, value);
        }
    }

    // Use this for initialization
    void Awake () {
        _animator = GetComponent<Animator>();
        _platformerController = GetComponent<MainPlatformerController>();
        _combatModule = GetComponent<CombatModule>();
        _enemyProximityDetector = GetComponent<EnemyProximityDetector>();
        _aimBehaviour = GetComponent<AimBehaviour>();
        _chainThrow = GetComponent<ChainThrowCombatBehaviour>();

        _platformerController.ShootAction += ShootActionHandler;
        _platformerController.ThrowAction += ThrowActionHandler;
        _chainThrow.ChainThrustComplete += ChainThrustCompleteHandler;
    }

    private void ChainThrustCompleteHandler(GrappledCharacter obj)
    {
        _animator.SetTrigger(ChainThrustComplete);
    }

    private void ShootActionHandler()
    {
        _animator.SetTrigger(Shoot);
    }

    private void ThrowActionHandler()
    {
        _animator.SetTrigger(Throw);
    }
	
    void Update()
    {
        HorizontalVelocity = _platformerController.HorizontalSpeed;
        VerticalVelocity = _platformerController.VerticalSpeed;
        Grounded = _platformerController.IsGrounded;
        LedgeGrabbing = _platformerController.LedgeGrab.GrabbingLedge;
        ComboCount = _combatModule.ComboNumber;
        WeaponType = _combatModule.WeaponType;
        AttackType = _combatModule.AttackType;
        AttackStyle = _combatModule.AttackStyle;
        Attacking = _platformerController.Attacking;
        Ducking = _platformerController.Ducking;
        SlidingDown = _platformerController.SlidingDown;
        ChargingAttack = _platformerController.IsCharging;
        GrabbingRope = _platformerController.RopeFound;
        RopeClimbDirection = _platformerController.RopeClimbDirection;
        VelocityGoingDown = _platformerController.VerticalSpeed < _previousVerticalVelocity;
        EnemyNearby = _enemyProximityDetector.EnemyNearby;
        TakingDamage = _platformerController.TakingDamage;
        _animator.SetBool(_GrabbingLadder, _platformerController.GrabbingLadder);
        _animator.SetBool(Aiming, _aimBehaviour.enabled);
        _animator.SetBool(LowKicking, _platformerController.LowKicking);
        _animator.SetBool(StingDash, _platformerController.StingDash);
        _animator.SetBool(SprintJump, _platformerController.SprintJump);
        _animator.SetBool(Dash, _platformerController.Dashing);
        _animator.SetBool(Pushing, _platformerController.Pushable != null);
        _animator.SetBool(WallDrag, _platformerController.WallDragging);
        _animator.SetInteger(AttackIndex, _combatModule.AttackIndex);

        var currentState = _animator.GetCurrentAnimatorStateInfo(0);
        if (_animator.GetCurrentAnimatorClipInfo(0).Length > 0)
        {
            var c = _animator.GetCurrentAnimatorClipInfo(0)[0];
            if (currentState.fullPathHash == _RollingState)
            {
                _animator.speed = c.clip.length / _platformerController.RollingDuration;
            }
            else if (currentState.fullPathHash == DashState)
            {
                _animator.speed = c.clip.length / _platformerController.DashDuration;
            }
            else
            {
                _animator.speed = 1;
            }
        }
        else
        {
            _animator.speed = 1;
        }

        _previousVerticalVelocity = _platformerController.VerticalSpeed;
    }
}
