//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34014
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using UnityEngine;

public static class Vector3Extensions
{
    public static Vector2 ToVector2(this Vector3 vec3)
    {
        return new Vector2(vec3.x, vec3.y);
    }

    public static bool Approximately(this Vector2 vec2, Vector2 other)
    {
        return Mathf.Approximately(vec2.x, other.x) && Mathf.Approximately(vec2.y, other.y);
    }
}


