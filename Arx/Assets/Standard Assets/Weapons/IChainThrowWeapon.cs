﻿using CommonInterfaces.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets.Weapons
{
    public interface IChainThrowWeapon : IWeapon
    {
        event Action OnAttackFinish;
        bool ReadyToThrow { get; }

        void FocusThrow();
        void Throw(float degrees, Transform origin);
    }
}
