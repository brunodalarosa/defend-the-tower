using DG.Tweening;
using UnityEngine;

namespace TowerDefense.Scripts.Turret
{
    public class TurretController : MonoBehaviour
    {
        [Header("References")]
        
        [SerializeField] 
        private ShooterController _shooterController = null;
        private ShooterController ShooterController => _shooterController;
        
        [SerializeField] 
        private Transform _firePoint = null;
        private Transform FirePoint => _firePoint;

        [SerializeField] 
        private Transform _rotationParent = null;
        private Transform RotationParent => _rotationParent;
        
        [Header("Configurations")]
        
        public float _turretRange = 5f;
        public float _fireRate = 1f;

        public float _bulletSpeed = 10f;

        public string _enemyTag = "Enemy";
        
        private Transform _currentTarget;
        private float _fireCountdown = 0f;

        private void Start()
        {
            InvokeRepeating(nameof(UpdateCurrentTarget), 0f, 0.5f);
        }

        private void Update()
        {
            if (_currentTarget == null) //todo pose de descanso do turret?
                return;

            RotationParent.DOLookAt(_currentTarget.position, 0.3f);

            if (_fireCountdown <= 0f)
            {
                ShooterController.Shoot(FirePoint, _currentTarget, _bulletSpeed);
                _fireCountdown = 1f / _fireRate;
            }

            _fireCountdown -= Time.deltaTime;
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

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _turretRange);
            
        }
    }
}
