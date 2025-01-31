﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets._2D.Scripts.Animator_Behaviours.Combat
{
    public class GroundCloseCombatStateBehaviour : StateMachineBehaviour
    {
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);
            animator.gameObject.GetComponent<CloseCombatBehaviour>().NotifyEnterCombatState();
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateExit(animator, stateInfo, layerIndex);
            animator.gameObject.GetComponent<CloseCombatBehaviour>().NotifyCombatFinish();
        }
    }
}
