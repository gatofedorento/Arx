﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CombatSystem.Interfaces;

namespace CombatSystem.DataStructures
{
    public class AttackConfiguration
    {
        public AttackPreCondition PreCondition { get; set; }
        public int Duration { get; set; }
        public List<IActionConfiguraton> Actions { get; set; }
        public AttackConfiguration NextAttack { get; set; }
        public string AnimationName { get; set; }
        public bool CancelIfAttacked { get; set; }

        public AttackConfiguration()
        {
            Actions = new List<IActionConfiguraton>();
        }
    }
}
