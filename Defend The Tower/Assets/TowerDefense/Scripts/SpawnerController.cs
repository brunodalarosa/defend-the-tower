using System.Collections;
using UnityEngine;

public class SpawnerController : MonoBehaviour
{
    [Header("References")]
    
    [SerializeField]
    private Transform _enemyPrefab = null;
    private Transform EnemyPrefab => _enemyPrefab;

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
        Instantiate(_enemyPrefab, t.position, t.rotation);
    }
}
