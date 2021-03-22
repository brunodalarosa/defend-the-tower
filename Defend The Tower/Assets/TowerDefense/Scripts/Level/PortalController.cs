using System;
using TowerDefense.Scripts.Enemy;
using UnityEngine;

namespace TowerDefense.Scripts.Level
{
    public class PortalController : MonoBehaviour
    {
        private static PortalController _instance;
        public static PortalController Instance => _instance;
        
        private ILevelUIUpdateListener _levelUIUpdateListener;
        
        private float _hp;
        
        private void Awake()
        {
            if (_instance != null && _instance != this) Destroy(gameObject);
            else _instance = this;
        }

        private void Start()
        {
            _levelUIUpdateListener = LevelController.Instance.GetLevelUIUpdateListener;
        }

        public void Init(int startingLife)
        {
            _hp = startingLife;
        }

        public void TakeHit(Enemy.Enemy enemy)
        {
            _hp -= enemy.DamagePower;
            UpdateUI();
        }

        private void CheckLife()
        {
            if (_hp <= 0) LevelController.Instance.LoseLevel();
        }
        
        private void UpdateUI()
        {
            _levelUIUpdateListener.UpdatePortalLife((int) Math.Ceiling(_hp));
        }
    }
}