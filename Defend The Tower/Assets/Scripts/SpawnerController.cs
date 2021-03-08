using System.Collections;
using UnityEngine;

public class SpawnerController : MonoBehaviour
{
    [Header("References")]
    
    [SerializeField]
    private Transform _enemyPrefab = null;
    private Transform EnemyPrefab => _enemyPrefab;

    [Space]
    [Header("Attributes")]
    
    public float _spawnCooldown = 5f;
    public float _levelStartDelay = 3f;

    private float _countDown;
    private int _waveNumber;

    private void Start()
    {
        _countDown = _levelStartDelay;
        _waveNumber = 1;
    }
    
    private void Update()
    {
        if (_countDown <= 0)
        {
            StartCoroutine(SpawnWave()); 
            _countDown = _spawnCooldown;
        }

        _countDown -= Time.deltaTime;
    }

    private IEnumerator SpawnWave()
    {
        for (int i = 0; i < _waveNumber; i++)
        {
            SpawnEnemy();

            yield return new WaitForSeconds(0.75f);
        }

        _waveNumber++;
    }

    private void SpawnEnemy()
    {
        var t = transform;
        Instantiate(_enemyPrefab, t.position, t.rotation);
    }
}
