﻿using Assets.Standard_Assets._2D.Scripts.Characters.Arx.StateMachine.TemplateStates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Standard_Assets._2D.Scripts.Characters.Arx.StateMachine.ThrowStates
{
    public class MovingAimThrowState : AimingState
    {
        protected override void PerformAttack()
        {
            StateController.PerformThrow();
        }
    }
}
