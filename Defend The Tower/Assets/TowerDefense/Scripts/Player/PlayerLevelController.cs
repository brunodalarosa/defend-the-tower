using System;
using TowerDefense.Scripts.Level;
using UnityEngine;

namespace TowerDefense.Scripts.Player
{
    public class PlayerLevelController : MonoBehaviour
    {
        private static PlayerLevelController _instance;
        public static PlayerLevelController Instance => _instance;
        
        [Header("Energy")]
        
        [SerializeField]
        private int _startEnergy = 300;

        [SerializeField] 
        private int _energyLimit = 900;

        private ILevelUIUpdateListener _levelUIUpdateListener;

        public int EnergyLimit { get; private set; }
        public int AvailableEnergy { get; private set; }

        private void Awake()
        {
            if (_instance != null && _instance != this) Destroy(gameObject);
            else _instance = this;
        }

        public void Start()
        {
            AvailableEnergy = _startEnergy;
            EnergyLimit = _energyLimit;
            _levelUIUpdateListener = LevelController.Instance.GetLevelUIUpdateListener;
            
            UpdateUI();
        }

        /// <summary>
        /// Increate Player Available Building Energy
        /// </summary>
        /// <param name="amount">The amount to increase</param>
        /// <returns>If the new total Available Building Energy exceeds the Limit returns the excess value or 0 if not</returns>
        public int IncreaseAvailableEnergy(int amount)
        {
            AvailableEnergy += amount;

            var remaining = AvailableEnergy <= EnergyLimit ? 0 : AvailableEnergy - EnergyLimit;

            if (remaining > 0) 
                AvailableEnergy = EnergyLimit;

            UpdateUI();
            
            return remaining;
        }

        /// <summary>
        /// Decrease Player Available Building Energy
        /// </summary>
        /// <param name="amount">The amount to decrease</param>
        /// <returns>True if the player still have the necessary amount to build that same building again</returns>
        public bool DecreaseAvailableEnergy(int amount)
        {
            AvailableEnergy -= amount;
            
            UpdateUI();

            return AvailableEnergy >= amount;
        }
        
        private void UpdateUI()
        {
            _levelUIUpdateListener.UpdateBuildingEnergy(AvailableEnergy, EnergyLimit);
        }

    }
}