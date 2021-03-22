using UnityEngine;

namespace TowerDefense.Scripts.Level
{
    public class LevelController : MonoBehaviour
    {
        private static LevelController _instance;
        public static LevelController Instance => _instance;

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

        [SerializeField] 
        private PortalController _portalController = null;
        private PortalController PortalController => _portalController;

        public ILevelUIUpdateListener GetLevelUIUpdateListener => LevelUIController;
        
        [Space] 
        [Header("Attributes")] 
        public int _startPortalLife = 50;
        public float _spawnCooldown = 5f;
        public float _levelStartDelay = 3f;

        private float _countDown;
        private int _waveNumber;

        private bool _wavesStarted;

        private void Awake()
        {
            if (_instance != null && _instance != this) Destroy(gameObject);
            else _instance = this;
        }

        private void Start()
        {
            _countDown = _levelStartDelay;
            _waveNumber = 0;
            _wavesStarted = false;
            
            LevelGrid.Init();
            PortalController.Init(_startPortalLife);
            
            LevelUIController.UpdatePortalLife(_startPortalLife);
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

        public void LoseLevel()
        {
            Debug.Log("YOU LOSE!");
            //todo todo todo
        }
    }
}