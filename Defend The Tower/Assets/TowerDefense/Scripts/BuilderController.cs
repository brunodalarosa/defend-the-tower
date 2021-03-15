using DG.Tweening;
using TowerDefense.Scripts.Utils;
using UnityEngine;
using UnityEngine.VFX;

namespace TowerDefense.Scripts
{
    public class BuilderController : MonoBehaviour
    {
        [Header("References")]
        
        [SerializeField] 
        private LevelGrid _grid = null;
        private LevelGrid Grid => _grid;
        
        [SerializeField] 
        private TurretController _basicTurret = null;
        private TurretController BasicTurret => _basicTurret;

        [SerializeField] 
        private VisualEffect _whiteCloudPoof = null;
        private VisualEffect WhiteCloudPoof => _whiteCloudPoof;

        private bool _buildingMode;
        private TurretController _currentSelectedTurret;

        private TurretController _hoverHologramTurret;
        private Vector3 _hoverHologramTurretPosition;

        private void Start()
        {
            _buildingMode = false;
            _currentSelectedTurret = null;
            ResetHoveringRefs();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                _buildingMode = !_buildingMode;
                _currentSelectedTurret = _buildingMode ? BasicTurret : null;
            }
            
            if (!_buildingMode)
            {
                ResetHoveringRefs();
                return;
            }

            var mouseWorldPos = MouseUtils.GetMouseWorldPosition();
            if (!Grid.CanBuildAtGridMousePosition(mouseWorldPos))
            {
                ResetHoveringRefs();
                return;
            }

            var hoveringGridPosition = Grid.GetCenterWorldGridPosition(mouseWorldPos);
            if (hoveringGridPosition == Vector3.negativeInfinity) return;

            if (_hoverHologramTurret == null)
            {
                InstantiateHologramTurret(hoveringGridPosition);
            }
            else
            {
                if (_hoverHologramTurretPosition != hoveringGridPosition)
                {
                    Destroy(_hoverHologramTurret.gameObject);
                    return;
                }

                if (Input.GetMouseButtonDown(0))
                {
                    BuildSelectedTurret(hoveringGridPosition);
                }
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
        
        private void BuildSelectedTurret(Vector3 hoveringGridPosition)
        {
            ResetHoveringRefs();
            Grid.GetGridFromWorldPos(hoveringGridPosition, out int x, out int y);
            Grid.UpdateGridData(x, y, 1); //todo user enum que representa a construção?

            var newTurret = Instantiate(_currentSelectedTurret, hoveringGridPosition, Quaternion.identity);

            newTurret.enabled = false;
            
            var newTurretTransform = newTurret.transform;
            newTurretTransform.localScale = new Vector3(.5f, .5f, .5f);
            newTurretTransform.localPosition += new Vector3(0, 1f, 0);

            Sequence seq = DOTween.Sequence();
            seq.Insert(0, newTurretTransform.DOScale(new Vector3(1f, 1f, 1f), 0.5f));
            seq.Insert(0.5f, newTurretTransform.DOMoveY(0f, 0.1f));
            seq.InsertCallback(0.6f, () => Instantiate(WhiteCloudPoof, hoveringGridPosition, Quaternion.identity, newTurretTransform));
            seq.InsertCallback(0.7f, () => newTurret.enabled = true);
            seq.Play();

            //todo Particle Effect System de fumaça
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