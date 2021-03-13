using CodeMonkey.Utils;
using TMPro;
using TowerDefense.Scripts.Utils;
using UnityEngine;

namespace TowerDefense.Scripts
{
    [RequireComponent(typeof(MeshRenderer))]
    public class GameGrid : MonoBehaviour
    {
        public float _cellSize = 1;

        private MeshRenderer _mesh;
        private int _width;
        private int _height;
        private int[,] _gridArray;
        private TextMeshPro[,] _debugTextArray;

        private void Start()
        {
            _mesh = GetComponent<MeshRenderer>();

            var bounds = _mesh.bounds;
            var size = bounds.max - bounds.min;
            
            _width =  (int) size.x;
            _height = (int) size.z;
            
            _gridArray = new int[_width, _height];
            _debugTextArray = new TextMeshPro[_width, _height];

            for (int x = 0; x < _gridArray.GetLength(0); x++)
            {
                for (int y = 0; y < _gridArray.GetLength(1); y++)
                {
                    _gridArray[x, y] = 0;
                    
                    _debugTextArray[x,y] = UtilsClass.CreateWorldText($"{x},{y}",
                        transform,
                        GetWorldPositionAtOrigin(x, y),
                        2,
                        Color.white);
                    
                    Debug.DrawLine(GetWorldPositionAtOrigin(0, y), GetWorldPositionAtOrigin(_width, y), Color.white, 100f);
                }
                
                Debug.DrawLine(GetWorldPositionAtOrigin(x, 0), GetWorldPositionAtOrigin(x, _height), Color.white, 100f);
            }
            
            Debug.DrawLine(GetWorldPositionAtOrigin(0, _height), GetWorldPositionAtOrigin(_width, _height), Color.white, 100f);
            Debug.DrawLine(GetWorldPositionAtOrigin(_width, 0), GetWorldPositionAtOrigin(_width, _height), Color.white, 100f);
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                UpCellValue(MouseUtils.GetMouseWorldPosition());
            }
            else if (Input.GetMouseButtonDown(1))
            {
                DownCellValue(MouseUtils.GetMouseWorldPosition());
            }
        }

        private Vector3 GetWorldPositionAtOrigin(int x, int y)
        {
            return transform.position + new Vector3(x,0, y) * _cellSize;
        }
        
        private Vector3 GetWorldPositionAtCenter(int x, int y)
        {
            return (transform.position + new Vector3(x,0, y) * _cellSize) + new Vector3(_cellSize, _cellSize) * 0.5f;
        }

        private void UpCellValue(int x, int y)
        {
            if (x < 0 || y < 0 || x >= _width || y >= _height) return;
            
            _gridArray[x, y]++;
            _debugTextArray[x, y].text = _gridArray[x, y].ToString();
        }
        
        private void DownCellValue(int x, int y)
        {
            if (x < 0 || y < 0 || x >= _width || y >= _height) return;
            
            _gridArray[x, y]--;
            _debugTextArray[x, y].text = _gridArray[x, y].ToString();
        }

        private void GetGridFromWorldPos(Vector3 worldPosition, out int x, out int y)
        {
            x = Mathf.FloorToInt(worldPosition.x / _cellSize);
            y = Mathf.FloorToInt(worldPosition.z / _cellSize);
        }
        
        public void UpCellValue(Vector3 worldPosition)
        {
            GetGridFromWorldPos(worldPosition, out var x, out var y);
            UpCellValue(x,y);
        }
        
        public void DownCellValue(Vector3 worldPosition)
        {
            GetGridFromWorldPos(worldPosition, out var x, out var y);
            DownCellValue(x,y);
        }
    }
}