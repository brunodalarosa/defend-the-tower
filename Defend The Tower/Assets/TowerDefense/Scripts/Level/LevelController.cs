using UnityEngine;

namespace TowerDefense.Scripts.Level
{
    public class LevelController : MonoBehaviour
    {
        [Header("References")]
        
        [SerializeField]
        private LevelGrid _levelGrid = null;
        private LevelGrid LevelGrid => _levelGrid;
        
        [SerializeField]
        private LevelUIController _levelUIController = null;
        private LevelUIController LevelUIController => _levelUIController;
        
        [SerializeField]
        private SpawnerController _spawnerController = null;
        private SpawnerController SpawnerController => _spawnerController;
        
        [Space]
        [Header("Attributes")]
    
        public float _spawnCooldown = 5f;
        public float _levelStartDelay = 3f;

        private float _countDown;
        private int _waveNumber;

        private bool _wavesStarted;

        private void Start()
        {
            _countDown = _levelStartDelay;
            _waveNumber = 0;
            _wavesStarted = false;
            
            LevelGrid.Init();
            LevelUIController.HideUI();
        }
    
        private void Update()
        {
            if (_countDown <= 0)
            {
                _waveNumber++;
                _countDown = _spawnCooldown;

                StartCoroutine(SpawnerController.SpawnWave(_waveNumber));
                
                LevelUIController.UpdateCurrentWaveText(_waveNumber);

                if (!_wavesStarted)
                {
                    LevelUIController.ShowUI();
                    _wavesStarted = true;
                }
            }

            _countDown -= Time.deltaTime;
            LevelUIController.UpdateNextWaveSlider(_spawnCooldown, Mathf.Max(_countDown, 0f));
        }
    }
}