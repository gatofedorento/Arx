﻿using Assets.Standard_Assets.Scripts;
using CommonInterfaces.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets.Weapons
{
    public abstract class EnemyDetection : MonoBehaviour
    {
        private List<ICharacter> _attackedEnemies;

        public Collider2D detectionCollider;

        public GameObject Owner { get; set; }

        public WeaponType WeaponType { get; protected set; }

        public void StartStrongAttack()
        {
            StartAttack();
        }

        public void StartLightAttack(int comboCount)
        {
            StartAttack();
        }

        public void AttackIsOver()
        {
            _attackedEnemies.Clear();
            this.enabled = false;
            detectionCollider.enabled = false;
        }

        void Awake()
        {
            _attackedEnemies = new List<ICharacter>();
            this.enabled = false;
            detectionCollider.enabled = false;
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            var enemy = other.GetComponent<ICharacter>();
            if(enemy == null)
            {
                return;
            }
            if (_attackedEnemies.Contains(enemy))
            {
                return;
            }
            _attackedEnemies.Add(enemy);
            enemy.Attacked(Owner, 10, null, DamageType.Sword, AttackTypeDetail.Generic);
        }

        private void StartAttack()
        {
            this.enabled = true;
            detectionCollider.enabled = true;
        }
    }
}
