﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MathHelper
{
    public static class FloatUtils
    {
        public const float FullDegreeTurn = 360;
        public const float HalfDegreeTurn = FullDegreeTurn / 2;

        public static bool IsApproximately(float a, float b, float tolerance)
        {
            return Mathf.Abs(a - b) < tolerance;
        }

        public static float AngleBetween(Vector3 origin, Vector3 other)
        {
            var dir = other - origin;
            return Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        }
    }
}
