using System;
using UnityEngine;

namespace TowerDefense.Scripts.Enemy
{
    [Serializable]
    [CreateAssetMenu(fileName = "Enemy", menuName = "EnemyData")]
    public class EnemyData : ScriptableObject
    {
        public Enemy _prefab;
        
        public float _hp;
        public float _speed;
        public float _damagePower;
    }
}