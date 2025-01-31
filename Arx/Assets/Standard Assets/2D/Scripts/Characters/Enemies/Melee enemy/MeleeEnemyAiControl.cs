﻿using UnityEngine;
using System.Collections;
using System;
using Extensions;
using Assets.Standard_Assets._2D.Scripts.Characters;
using CommonInterfaces.Enums;
using Assets.Standard_Assets._2D.Scripts.Controllers;
using Assets.Standard_Assets.Scripts.StateMachine;

public class MeleeEnemyAiStateManager : StateManager<ICharacterAI, object>
{
    public MeleeEnemyAiStateManager(ICharacterAI controller) : base(controller)
    {
        this.SetInitialState<IddleState<ICharacterAI>>()
            .To<FollowState<ICharacterAI>>((c, o, t) => c.Target != null);

        this.From<FollowState<ICharacterAI>>()
            .To<AttackTargetState<ICharacterAI>>((c, a, t) => c.IsTargetInRange)
            .To<IddleState<ICharacterAI>>((c, o, t) => c.Target == null);

        this.From<AttackTargetState<ICharacterAI>>()
            .To<FollowState<ICharacterAI>>((c, a, t) => !c.IsTargetInRange && !c.Attacking);
    }
}

[RequireComponent(typeof(MeleeEnemyController))]
public class MeleeEnemyAiControl : AbstractPlatformerCharacterAiController, ICharacterAI
{
    [SerializeField]
    private CharacterFinder _characterFinder;
    [SerializeField]
    private float _attackRange = 1;

    private MeleeEnemyController _controller;
    private MeleeEnemyAiStateManager _stateManager;

    public bool Attacking
    {
        get
        {
            return _controller.Attacking;
        }
    }

    public override Direction CurrentDirection
    {
        get
        {
            return _controller.Direction;
        }
    }

    protected override Vector2 Velocity
    {
        get
        {
            return _controller.Velocity;
        }
    }

    public void MoveToTarget()
    {
        _controller.MovementType = MovementType.Run;
        FollowTarget();
    }

    public void StartIddle()
    {
        _controller.MovementType = MovementType.Walk;
        IddleMovement();
    }

    public void StopIddle()
    {
        StopActiveCoroutine();
    }

    public void StopMoving()
    {
        StopActiveCoroutine();
    }

    public void OrderAttack()
    {
        StopActiveCoroutine();
        _controller.OrderAttack();
    }

    // Use this for initialization
    protected override void Awake() {
        base.Awake();
        _controller = GetComponent<MeleeEnemyController>();
        _stateManager = new MeleeEnemyAiStateManager(this);
        _characterFinder.OnCharacterFound += OnCharacterFoundHandler;
    }

    public override void Move(float directionValue)
    {
        _controller.Move(directionValue);
    }

    void Update()
    {
        _stateManager.Perform(null);
    }

    void OnDestroy()
    {
        _characterFinder.OnCharacterFound -= OnCharacterFoundHandler;
    }

    private void FollowTarget()
    {
        _controller.MovementType = MovementType.Run;
        SetActiveCoroutine(FollowTargetCoroutine());
    }

    private void OnCharacterFoundHandler(BasePlatformerController controller)
    {
        if(Target == null)
        {
            Target = controller.gameObject;
        }
    }

    private IEnumerator FollowTargetCoroutine()
    {
        if (Target == null)
        {
            yield break;
        }

        while (true)
        {
            if (IsTargetInRange)
            {
                yield break;
            }
            var currentPosition = this.transform.position;
            var xDifference = Target.transform.position.x - currentPosition.x;
            _controller.Move(xDifference);
            yield return null;
        }
    }
}
