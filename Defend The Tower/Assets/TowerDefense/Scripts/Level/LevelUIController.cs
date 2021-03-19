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
        private RectTransform _nextWaveUI = null;
        private RectTransform NextWaveUI => _nextWaveUI;
    
        [SerializeField] 
        private Image _timeToNextWaveSliderImage = null;
        private Image TimeToNextWaveSliderImage => _timeToNextWaveSliderImage;
        
        [SerializeField] 
        private TextMeshProUGUI _buildingEnergyText = null;
        private TextMeshProUGUI BuildingEnergyText => _buildingEnergyText;

        public void UpdateCurrentWaveText(int waveNumber)
        {
            CurrentWaveText.text = $"Wave {waveNumber}";
        }

        public void UpdateNextWaveSlider(float nextWaveCooldown, float remainingTime)
        {
            var percent = remainingTime / nextWaveCooldown;
            TimeToNextWaveSliderImage.fillAmount = percent;
        }

        public void HideUI()
        {
            CurrentWaveText.gameObject.SetActive(false);
            NextWaveUI.gameObject.SetActive(false);
            TimeToNextWaveSliderImage.gameObject.SetActive(false);
        }

        public void ShowUI()
        {
            CurrentWaveText.gameObject.SetActive(true);
            NextWaveUI.gameObject.SetActive(true);
            TimeToNextWaveSliderImage.gameObject.SetActive(true);
        }

        public void UpdateBuildingEnergyText(int available, int limit)
        {
            BuildingEnergyText.text = $"{available} / {limit}";
        }
    }
}
