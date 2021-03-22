using UnityEngine;

namespace TowerDefense.Scripts
{
    public class ShooterController : MonoBehaviour
    {
        [SerializeField] 
        private BulletController _bulletPrefab = null;
        private BulletController BulletPrefab => _bulletPrefab;

        public void Shoot(Transform firePoint, Transform target, float speed)
        {
            Instantiate(BulletPrefab, firePoint.position, firePoint.rotation).Fire(target, speed);
        }
    }
}