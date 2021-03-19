using TowerDefense.Scripts.Level;
using UnityEngine;

namespace TowerDefense.Scripts.Player
{
    public class PlayerLevelController : MonoBehaviour
    {
        [Header("References")] 
        
        [SerializeField]
        private LevelUIController _levelUIController = null;
        private LevelUIController LevelUIController => _levelUIController;
        
        [Header("Energy")]
        
        [SerializeField]
        private int _startEnergy = 300;

        [SerializeField] 
        private int _energyLimit = 900;

        private ILevelUIUpdateListener _levelUIUpdateListener;

        public int EnergyLimit { get; private set; }
        public int AvailableEnergy { get; private set; }

        public void Start()
        {
            AvailableEnergy = _startEnergy;
            EnergyLimit = _energyLimit;
            _levelUIUpdateListener = LevelUIController;
            
            updateUI();
        }

        /// <summary>
        /// Increate Player Available Building Energy
        /// </summary>
        /// <param name="amount">The amount to increase</param>
        /// <returns>If the new total Available Building Energy exceeds the Limit returns the excess value or 0 if not</returns>
        public int IncreaseAvailableEnergy(int amount)
        {
            AvailableEnergy += amount;
            
            updateUI();
            
            if (AvailableEnergy <= EnergyLimit) return 0;
            
            var remaining = AvailableEnergy - EnergyLimit;
            AvailableEnergy = EnergyLimit;
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
            
            updateUI();

            return AvailableEnergy >= amount;
        }
        
        private void updateUI()
        {
            _levelUIUpdateListener.UpdateBuildingEnergyText(AvailableEnergy, EnergyLimit);
        }

    }
}