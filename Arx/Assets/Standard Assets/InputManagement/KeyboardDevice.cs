﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class KeyboardDevice : IInputDevice
{
    private Sprite[] _sprites;

    public Sprite InteractSprite
    {
        get
        {
            return _sprites.FirstOrDefault(sprite => sprite.name == "EKey");
        }
    }

    public Sprite Up
    {
        get
        {
            return _sprites.FirstOrDefault(sprite => sprite.name == "WKey");
        }
    }

    public bool MouseSupport { get { return true; } }

    public KeyboardDevice()
    {
        var test = Resources.LoadAll("MouseKeyboard");
        _sprites = Resources.LoadAll<Sprite>("MouseKeyboard");
    }

    public Vector2 GetAxis(DeviceAxis axis)
    {
        if(axis == DeviceAxis.Movement)
        {
            var horizontal = Input.GetAxisRaw("Horizontal");
            var xMove = Math.Abs(horizontal) > 0.15f ? 1 : 0;
            return new Vector2(Math.Sign(horizontal) * xMove, Input.GetAxis("Vertical"));
        }

        return Vector2.zero;
    }

    public bool GetButton(DeviceButton button)
    {
        var buttonName = button.ToString();
        return Input.GetButton(buttonName);
    }

    public bool GetButtonDown(DeviceButton button)
    {
        var buttonName = button.ToString();
        return Input.GetButtonDown(buttonName);
    }

    public bool GetButtonUp(DeviceButton button)
    {
        var buttonName = button.ToString();
        return Input.GetButtonUp(buttonName);
    }

    public void Update()
    {
    }
}

