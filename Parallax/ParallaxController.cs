﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Extensions;

namespace Parallax
{
    public class ParallaxController : MonoBehaviour
    {
        private Vector3? _previousPosition;

        public ParallaxLayer[] parallaxLayers;

        void Update()
        {
            var camera = Camera.main;
            if (camera == null)
            {
                return;
            }
            if (_previousPosition == null)
            {
                _previousPosition = camera.transform.position;
                return;
            }
            
            var cameraPosition = camera.transform.position;
            var positionDif = cameraPosition - _previousPosition.Value;
            
            foreach (var layer in parallaxLayers)
            {
                var layerPosition = layer.layerContainer.transform.position;
                var xPosition = positionDif.x * layer.scrollRate.x;
                var yPosition = positionDif.y * layer.scrollRate.y;
                var scroll = new Vector3(xPosition, yPosition, 0);
                layer.layerContainer.transform.position = scroll + layerPosition;
            }
            _previousPosition = cameraPosition;
        }
    }
}
