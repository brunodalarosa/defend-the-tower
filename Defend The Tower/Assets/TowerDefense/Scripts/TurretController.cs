using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

namespace TowerDefense.Scripts
{
    public class TurretController : MonoBehaviour
    {
        public float _turretRange = 5f;
        public string _enemyTag = "Enemy";
        
        private Transform _currentTarget;

        [SerializeField] 
        private Transform _rotationParent = null;
        private Transform RotationParent => _rotationParent;

        private void Start()
        {
            InvokeRepeating(nameof(UpdateCurrentTarget), 0f, 0.5f);
        }

        private void UpdateCurrentTarget()
        {
            var enemies = GameObject.FindGameObjectsWithTag(_enemyTag);

            if (enemies.Length <= 0)
                return;

            float shortestDistance = Mathf.Infinity;
            GameObject nearestEnemy = null;

            foreach (var enemy in enemies)
            {
                float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
                
                if (distanceToEnemy > _turretRange)
                    continue;

                if (distanceToEnemy < shortestDistance)
                {
                    shortestDistance = distanceToEnemy;
                    nearestEnemy = enemy;
                }
            }

            if (nearestEnemy != null && shortestDistance <= _turretRange)
            {
                _currentTarget = nearestEnemy.transform;
            }
            else
            {
                _currentTarget = null;
            }
        }

        private void Update()
        {
            if (_currentTarget == null) //todo pose de descanso do turret?
                return;

            RotationParent.DOLookAt(_currentTarget.position, 0.3f);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _turretRange);
            
        }
    }
}
