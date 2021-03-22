using UnityEngine;

namespace TowerDefense.Scripts
{
    public class BulletController : MonoBehaviour
    {
        [SerializeField] 
        private GameObject _bulletImpactParticleSystem = null;
        private GameObject BulletImpactParticleSystem => _bulletImpactParticleSystem;

        public float _damage = 3;
        
        private Transform _target;
        private float _speed;

        public void Fire(Transform target, float speed)
        {
            _target = target;
            _speed = speed;
        }

        private void Update()
        {
            if (_target == null)
            {
                Destroy(gameObject);
                return;
            }

            var dir = (_target.position + Vector3.up * 0.6f) - transform.position;
            float distanceThisFrame = _speed * Time.deltaTime;

            if (dir.magnitude <= distanceThisFrame)
            {
                HitTarget();
                return;
            }
            
            transform.Translate(dir.normalized * distanceThisFrame, Space.World);

        }

        private void HitTarget()
        {
            var bulletImpactParticles = Instantiate(BulletImpactParticleSystem, _target.position, _target.rotation);
            
            Destroy(bulletImpactParticles, 2f);
            Destroy(gameObject);
            
            Damage(_target);
        }

        private void Damage(Transform target)
        {
            var enemy = target.GetComponent<Enemy.Enemy>();

            if (enemy != null)
            {
                enemy.TakeDamage(_damage);
            }
        }
    }
}