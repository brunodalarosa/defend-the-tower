using TowerDefense.Scripts.Level;
using TowerDefense.Scripts.Player;
using UnityEngine;

namespace TowerDefense.Scripts.Enemy
{
    public class Enemy : MonoBehaviour
    {
        public float StartingHp { get; private set; }
        public float Speed { get; private set; }
        public float DamagePower { get; private set; }

        private float _actualHp;

        public void SetValues(EnemyData enemyData)
        {
            StartingHp = enemyData._hp;
            Speed = enemyData._speed;
            DamagePower = enemyData._damagePower;

            _actualHp = StartingHp;
        }

        public virtual void TakeDamage(float damage)
        {
            _actualHp -= damage;
            CheckHitPoints();
        }

        protected virtual void CheckHitPoints()
        {
            if (_actualHp <= 0)
            {
                PlayerLevelController.Instance.IncreaseAvailableEnergy(20);
                Die();
            }
        }

        public virtual void ArrivedAtPortal()
        {
            PortalController.Instance.TakeHit(this);
            Die();
        }

        protected virtual void Die()
        {
            Destroy(gameObject);
        }
    }
}