using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TowerDefense.Scripts.Level
{
    public class LevelUIController : MonoBehaviour, ILevelUIUpdateListener
    {
        [SerializeField] 
        private TextMeshProUGUI _currentWaveText = null;
        private TextMeshProUGUI CurrentWaveText => _currentWaveText;

        [SerializeField] 
        private Image _timeToNextWaveSliderImage = null;
        private Image TimeToNextWaveSliderImage => _timeToNextWaveSliderImage;
        
        [SerializeField] 
        private TextMeshProUGUI _buildingEnergyText = null;
        private TextMeshProUGUI BuildingEnergyText => _buildingEnergyText;
        
        [SerializeField] 
        private TextMeshProUGUI _portalLifeText = null;
        private TextMeshProUGUI PortalLifeText => _portalLifeText;

        public void UpdateCurrentWaveText(int waveNumber)
        {
            CurrentWaveText.text = $"Wave {waveNumber}";
        }

        public void UpdateNextWaveSlider(float nextWaveCooldown, float remainingTime)
        {
            var percent = remainingTime / nextWaveCooldown;
            TimeToNextWaveSliderImage.fillAmount = percent;
        }

        public void UpdateBuildingEnergy(int available, int limit)
        {
            BuildingEnergyText.text = $"{available} / {limit}";
        }
        
        public void UpdatePortalLife(int portalLife)
        {
            PortalLifeText.text = $"{portalLife}";
        }
        
        public void HideUI()
        {
            gameObject.SetActive(false);
        }

        public void ShowUI()
        {
            gameObject.SetActive(true);
        }
    }
}
