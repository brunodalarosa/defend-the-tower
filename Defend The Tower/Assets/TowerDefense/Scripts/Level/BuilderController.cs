using DG.Tweening;
using TowerDefense.Scripts.Turret;
using TowerDefense.Scripts.Utils;
using UnityEngine;
using UnityEngine.VFX;

namespace TowerDefense.Scripts.Level
{
    public class BuilderController : MonoBehaviour
    {
        [Header("References")]
        
        [SerializeField] 
        private LevelGrid _grid = null;
        private LevelGrid Grid => _grid;

        [SerializeField] 
        private VisualEffect _whiteCloudPoof = null;
        private VisualEffect WhiteCloudPoof => _whiteCloudPoof;

        private bool _buildingMode;
        private TurretController _currentSelectedTurret;

        private TurretController _hoverHologramTurret;
        private Vector3 _hoverHologramTurretPosition;
        private Vector3 _hoveringGridPosition;

        private void Start()
        {
            ExitBuildingMode();
        }

        public void EnterBuildingMode(TurretController turret)
        {
            _currentSelectedTurret = turret;
            _buildingMode = true;
        }

        public void ExitBuildingMode()
        {
            ResetHoveringRefs();
            _currentSelectedTurret = null;
            _buildingMode = false;
        }

        private void Update()
        {
            if (!_buildingMode)
                return;

            var mouseWorldPos = MouseUtils.GetMouseWorldPosition();
            if (!Grid.CanBuildAtGridMousePosition(mouseWorldPos))
            {
                ResetHoveringRefs();
                return;
            }

            _hoveringGridPosition = Grid.GetCenterWorldGridPosition(mouseWorldPos);
            
            if (_hoveringGridPosition == Vector3.negativeInfinity) return;

            if (_hoverHologramTurret == null)
            {
                InstantiateHologramTurret(_hoveringGridPosition);
            }
            else if (_hoverHologramTurretPosition != _hoveringGridPosition)
            {
                Destroy(_hoverHologramTurret.gameObject);
            }
        }

        private void InstantiateHologramTurret(Vector3 hoveringGridPosition)
        {
            _hoverHologramTurret = Instantiate(_currentSelectedTurret, hoveringGridPosition, Quaternion.identity);

            _hoverHologramTurret.enabled = false;

            var hoverTransform = _hoverHologramTurret.transform;
            hoverTransform.localScale = new Vector3(.5f, .5f, .5f);
            hoverTransform.localPosition += new Vector3(0, 1f, 0);

            _hoverHologramTurretPosition = hoveringGridPosition;

            var hoverCollider = _hoverHologramTurret.GetComponent<BoxCollider>();

            if (hoverCollider != null)
                hoverCollider.enabled = false;
        }
        
        public void BuildSelectedTurret()
        {
            ResetHoveringRefs();
            Grid.GetGridFromWorldPos(_hoveringGridPosition, out int x, out int y);
            Grid.UpdateGridData(x, y, 1); //todo user enum que representa a construção?

            var newTurret = Instantiate(_currentSelectedTurret, _hoveringGridPosition, Quaternion.identity);

            newTurret.enabled = false;
            
            var newTurretTransform = newTurret.transform;
            newTurretTransform.localScale = new Vector3(.5f, .5f, .5f);
            newTurretTransform.localPosition += new Vector3(0, 1f, 0);

            Sequence seq = DOTween.Sequence();
            seq.Insert(0, newTurretTransform.DOScale(new Vector3(1f, 1f, 1f), 0.5f));
            seq.Insert(0.5f, newTurretTransform.DOMoveY(0f, 0.1f));
            seq.InsertCallback(0.6f, () => Instantiate(WhiteCloudPoof, _hoveringGridPosition, Quaternion.identity, newTurretTransform));
            seq.InsertCallback(0.7f, () => newTurret.enabled = true);
            seq.Play();
            
            //todo Método que inicia a construção mas demora um tempo?
        }

        private void ResetHoveringRefs()
        {
            if (_hoverHologramTurret != null)
                Destroy(_hoverHologramTurret.gameObject);
            
            _hoverHologramTurret = null;
            _hoverHologramTurretPosition = Vector3.negativeInfinity;
        }
    }
}