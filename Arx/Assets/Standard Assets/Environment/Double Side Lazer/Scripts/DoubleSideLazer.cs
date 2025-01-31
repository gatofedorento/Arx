﻿using Assets.Standard_Assets.Scripts;
using CommonInterfaces.Controllers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets.Environment.Double_Side_Lazer.Scripts
{
    public class DoubleSideLazer : MonoBehaviour
    {
        private Coroutine _lazerCoroutine;
        private RaycastHit2D[] _hits;

        [SerializeField]
        private LineRenderer _p1;
        [SerializeField]
        private LineRenderer _p2;
        [SerializeField]
        private LayerMask _targetLayer;
        [SerializeField]
        private int _damage = 1;
        [SerializeField]
        private bool _startEnabled;


        public DoubleSideLazer()
        {
            _hits = new RaycastHit2D[10];
        }

        private void Start()
        {
            if (_startEnabled)
            {
                ActivateLazer();
            }
        }

        public void ActivateLazer()
        {
            StartCoroutine(LazerRoutine());
        }

        public void DeactivateLazer()
        {
            if(_lazerCoroutine != null)
            {
                StopCoroutine(_lazerCoroutine);
            }
        }

        private IEnumerator LazerRoutine()
        {
            while (true)
            {
                var p1 = _p1.transform.position;
                var p2 = _p2.transform.position;
                var hit1 = GetClosestHit(p1, p2);
                var hit2 = default(RaycastHit2D);
                if (hit1)
                {
                    hit2 = GetClosestHit(p2, p1);
                    var character1 = hit1.transform.GetComponent<ICharacter>();
                    var character2 = hit2.transform.GetComponent<ICharacter>();

                    DealDamage(character1, hit1);
                    DealDamage(character2, hit2);

                }
                ResizeLazer(hit1, hit2);
                yield return null;
            }
        }

        private RaycastHit2D GetClosestHit(Vector3 origin, Vector3 target)
        {
            var hitCount = Physics2D.LinecastNonAlloc(origin, target, _hits, _targetLayer);
            var hit = default(RaycastHit2D);
            for(var idx = 0; idx < hitCount; idx++)
            {
                if (_hits[idx].collider.isTrigger)
                {
                    continue;
                }
                if (hit)
                {
                    hit =
                        Vector2.Distance(hit.point, origin) < Vector2.Distance(_hits[idx].point, origin)
                            ? hit
                            : _hits[idx];
                }
                else
                {
                    hit = _hits[idx];
                }
            }
            return hit;
        }

        private void DealDamage(ICharacter character, RaycastHit2D hit)
        {
            if (character != null)
            {
                character.Attacked(gameObject, _damage, hit.point, DamageType.Environment, AttackTypeDetail.Generic);
            }
        }

        private void ResizeLazer(RaycastHit2D hit1, RaycastHit2D hit2)
        {
            var p1Position = _p1.transform.position;
            var p2Position = _p2.transform.position;

            if (!hit1 && !hit2)
            {
                _p2.enabled = false;
                _p1.SetPositions(new []{ p1Position, p2Position });
            }
            else
            {
                _p2.enabled = true;
                _p1.SetPositions(new[] { p1Position, new Vector3(hit1.point.x, hit1.point.y, 0) });
                _p2.SetPositions(new[] { p2Position, new Vector3(hit2.point.x, hit2.point.y, 0) });
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(_p1.transform.position, _p2.transform.position);
        }
    }
}
