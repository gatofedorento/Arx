﻿using Assets.Standard_Assets.Scripts;
using CommonInterfaces.Controllers;
using Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets._2D.Scripts.Hazards
{
    public class DamageOnTouch : MonoBehaviour
    {
        [SerializeField]
        private LayerMask _platformMask;
        [SerializeField]
        private Transform _safeSpot;
        [SerializeField]
        private int _damage = 1;
        [SerializeField]
        private AudioSource _hitSound;

        public bool Active { get; set; }

        public DamageOnTouch()
        {
            Active = true;
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            DamageEnemy(col);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            DamageEnemy(collision.collider);
        }

        private void DamageEnemy(Collider2D col)
        {
            if (!Active)
            {
                return;
            }
            if (!_platformMask.IsInAnyLayer(col.gameObject))
            {
                return;
            }

            var controller = col.GetComponent<ICharacter>();
            if (controller == null)
            {
                return;
            }

            if (_safeSpot != null && controller is MainPlatformerController)
            {
                var mainPlaformer = controller as MainPlatformerController;
                mainPlaformer.Hit(gameObject, _safeSpot.position, _damage, DamageType.Environment);
            }
            else
            {
                controller.Attacked(gameObject, _damage, transform.position, DamageType.Environment);
            }
            if (_hitSound != null)
            {
                _hitSound.Play();
            }
        }
    }
}
