﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets
{
    public static class MathUtilities
    {
        public static Vector2 ExponentialInterpolation(Vector2 origin, Vector2 target, float duration, ref float elapsedLerpTime)
        {
            float t = elapsedLerpTime / duration;
            t = t * t;
            elapsedLerpTime += Time.deltaTime;
            return Vector2.Lerp(origin, target, t);
        }

        public static float ExponentialInterpolation(float origin, float target, float duration, ref float elapsedLerpTime)
        {
            float t = elapsedLerpTime / duration;
            t = t * t;
            elapsedLerpTime += Time.deltaTime;
            return Mathf.Lerp(origin, target, t);
        }

        public static float GetPercentageAlong(Vector3 start, Vector3 end, Vector3 point)
        {
            var ab = end - start;
            var ac = point - start;
            return Vector3.Dot(ac, ab) / ab.sqrMagnitude;
        }

        public static float InverseLerp(Vector3 a, Vector3 b, Vector3 value)
        {
            var ab = b - a;
            var av = value - a;
            return Vector3.Dot(av, ab) / Vector3.Dot(ab, ab);
        }

        public static float InverseLerp(Vector2 a, Vector2 b, Vector2 value)
        {
            var ab = b - a;
            var av = value - a;
            return Vector2.Dot(av, ab) / Vector2.Dot(ab, ab);
        }
    }
}
