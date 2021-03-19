using System;
using TowerDefense.Scripts.Level;
using TowerDefense.Scripts.Turret;
using UnityEngine;

namespace TowerDefense.Scripts.Player
{
    public class PlayerBuilderController : MonoBehaviour
    {
        [Header("References")]
        
        [SerializeField] 
        private BuilderController _builderController = null;
        private BuilderController BuilderController => _builderController;
        
        [SerializeField] 
        private PlayerLevelController _playerLevelController = null;
        private PlayerLevelController PlayerLevelController => _playerLevelController;
        
        [Header("Turrets Blueprints")]
        
        [SerializeField] 
        private TurretBlueprint _basicTurret = null;
        private TurretBlueprint BasicTurret => _basicTurret;

        private TurretType _selectedTurretType = TurretType.None;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1)) //Basic
            {
                if (_selectedTurretType == TurretType.Basic)
                {
                    Reset();
                    return;
                }

                var basic = GetTurretByType(TurretType.Basic);

                if (PlayerLevelController.AvailableEnergy >= basic._energyCost)
                {
                    BuilderController.EnterBuildingMode(basic._prefab);
                    _selectedTurretType = TurretType.Basic;
                }
                else
                {
                    //todo informar o jogador que ele não tem energia suficiente pra construir esse tipo de turret
                }
            }

            if (_selectedTurretType == TurretType.None)
                return;
            
            if (Input.GetMouseButtonDown(0))
            {
                BuilderController.BuildSelectedTurret();

                var turret = GetTurretByType(_selectedTurretType);
                
                var canBuildAgain = PlayerLevelController.DecreaseAvailableEnergy(turret._energyCost);

                if (!canBuildAgain)
                {
                    BuilderController.ExitBuildingMode();
                    _selectedTurretType = TurretType.None;
                }
            } 
            else if (Input.GetMouseButtonDown(1)) Reset();
        }

        private void Reset()
        {
            BuilderController.ExitBuildingMode();
            _selectedTurretType = TurretType.None;
        }

        private TurretBlueprint GetTurretByType(TurretType type)
        {
            return type switch
            {
                TurretType.None => null,
                TurretType.Basic => BasicTurret,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }
    }
}