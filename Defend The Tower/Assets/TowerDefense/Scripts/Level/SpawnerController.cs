using System.Collections;
using TowerDefense.Scripts.Enemy;
using UnityEngine;

namespace TowerDefense.Scripts.Level
{
    public class SpawnerController : MonoBehaviour
    {
        [Header("References")]
    
        [SerializeField]
        private EnemyData _enemyData = null;
        private EnemyData EnemyData => _enemyData;

        public IEnumerator SpawnWave(int numOfEnemiesToSpawn)
        {
            for (int i = 0; i < numOfEnemiesToSpawn; i++)
            {
                SpawnEnemy();

                yield return new WaitForSeconds(0.75f);
            }
        }

        private void SpawnEnemy()
        {
            var t = transform;
            var newEnemy = Instantiate(EnemyData._prefab, t.position, t.rotation);
            newEnemy.SetValues(EnemyData);
        }
    }
}
