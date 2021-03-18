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
        private UIController _uiController = null;
        private UIController UIController => _uiController;
        
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
            UIController.HideUI();
        }
    
        private void Update()
        {
            if (_countDown <= 0)
            {
                _waveNumber++;
                _countDown = _spawnCooldown;

                StartCoroutine(SpawnerController.SpawnWave(_waveNumber));
                
                UIController.UpdateCurrentWaveText(_waveNumber);

                if (!_wavesStarted)
                {
                    UIController.ShowUI();
                    _wavesStarted = true;
                }
            }

            _countDown -= Time.deltaTime;
            UIController.UpdateNextWaveSlider(_spawnCooldown, Mathf.Max(_countDown, 0f));
        }
    }
}