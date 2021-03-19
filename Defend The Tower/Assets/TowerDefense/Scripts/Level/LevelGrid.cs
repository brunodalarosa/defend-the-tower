using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CodeMonkey.Utils;
using TMPro;
using TowerDefense.Scripts.Extensions;
using TowerDefense.Scripts.Utils;
using UnityEngine;

// ReSharper disable ConditionIsAlwaysTrueOrFalse

namespace TowerDefense.Scripts.Level
{
    [RequireComponent(typeof(MeshRenderer))]
    public class LevelGrid : MonoBehaviour
    {
        private const bool DebugMode = false;
        private bool _started;

        public float _cellSize = 1;

        private MeshRenderer _mesh;
        private int _width;
        private int _height;
        private int[,] _gridData;
        private TextMeshPro[,] _debugTextGrid;

        public void Init()
        {
            _mesh = GetComponent<MeshRenderer>();

            var bounds = _mesh.bounds;
            var size = bounds.max - bounds.min;
            
            _width =  (int) size.x;
            _height = (int) size.z;
            
            _gridData = new int[_width, _height];
            _debugTextGrid = new TextMeshPro[_width, _height];
            
            string path = "Assets/Resources/Levels/level-0.txt";

            var levelDefinition = GetLevelDefinition(path);

            for (int x = 0; x < _gridData.GetLength(0); x++)
            {
                for (int y = 0; y < _gridData.GetLength(1); y++)
                {
                    _gridData[x, y] = levelDefinition[x][y];

                    if (DebugMode)
                    {
                        _debugTextGrid[x,y] = UtilsClass.CreateWorldText($"{x},{y}",
                            transform,
                            GetWorldPositionAtOrigin(x, y),
                            2,
                            Color.white);
                    
                        Debug.DrawLine(GetWorldPositionAtOrigin(0, y), GetWorldPositionAtOrigin(_width, y), Color.white, 100f);
                    }
                }
                
                if (DebugMode)
                    Debug.DrawLine(GetWorldPositionAtOrigin(x, 0), GetWorldPositionAtOrigin(x, _height), Color.white, 100f);
            }

            if (DebugMode)
            {
                Debug.DrawLine(GetWorldPositionAtOrigin(0, _height), GetWorldPositionAtOrigin(_width, _height), Color.white, 100f);
                Debug.DrawLine(GetWorldPositionAtOrigin(_width, 0), GetWorldPositionAtOrigin(_width, _height), Color.white, 100f);
            }

            _started = true;
        }

        private static List<List<int>> GetLevelDefinition(string path)
        {
            var reader = new StreamReader(path);
            var levelString = reader.ReadToEnd();

            var levelDefinition = new List<List<int>>();

            foreach (var line in levelString.Split(new[] {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries))
            {
                var lineValues = line.Split(',');

                var levelLine = lineValues.Select(tileValue => Convert.ToInt32(tileValue)).ToList();

                levelDefinition.Add(levelLine);
            }

            return levelDefinition;
        }

        private void Update()
        {
            if (!DebugMode || !_started)
                return;
            
            if (Input.GetMouseButtonDown(0))
            {
                UpCellValue(MouseUtils.GetMouseWorldPosition());
            }
            else if (Input.GetMouseButtonDown(1))
            {
                DownCellValue(MouseUtils.GetMouseWorldPosition());
            }
            else if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                _gridData.Print2DArray();
            }
        }

        public bool CanBuildAtGridMousePosition(Vector3 worldPosition)
        {
            if (!_started) throw new InvalidOperationException("Trying to use LevelGrid before initializing it!");
            
            GetGridFromWorldPos(worldPosition, out int x, out int y);
            
            if (x < 0 || y < 0 || x >= _width || y >= _height) return false;

            return _gridData[x, y] <= 0;
        }

        public Vector3 GetCenterWorldGridPosition(Vector3 worldPosition)
        {
            if (!_started) throw new InvalidOperationException("Trying to use LevelGrid before initializing it!");
            
            GetGridFromWorldPos(worldPosition, out int x, out int y);
            
            if (x < 0 || y < 0 || x >= _width || y >= _height) return Vector3.negativeInfinity;

            return GetWorldPositionAtCenter(x, y);
        }

        public void UpdateGridData(int x, int y, int value)
        {
            if (!_started) throw new InvalidOperationException("Trying to use LevelGrid before initializing it!");
            
            if (x < 0 || y < 0 || x >= _width || y >= _height) throw new InvalidOperationException("Trying to update GridData at invalid position!");
                
            _gridData[x, y] = value;
        }

        private Vector3 GetWorldPositionAtOrigin(int x, int y)
        {
            return transform.position + new Vector3(x,0, y) * _cellSize;
        }
        
        private Vector3 GetWorldPositionAtCenter(int x, int y)
        {
            return (transform.position + new Vector3(x,0, y) * _cellSize) + new Vector3(_cellSize, _cellSize, _cellSize) * 0.5f;
        }
        
        public void GetGridFromWorldPos(Vector3 worldPosition, out int x, out int y)
        {
            x = Mathf.FloorToInt(worldPosition.x / _cellSize);
            y = Mathf.FloorToInt(worldPosition.z / _cellSize);
        }

        private void UpCellValue(int x, int y)
        {
            if (x < 0 || y < 0 || x >= _width || y >= _height) return;
            
            _gridData[x, y]++;
            _debugTextGrid[x, y].text = _gridData[x, y].ToString();
        }
        
        private void DownCellValue(int x, int y)
        {
            if (x < 0 || y < 0 || x >= _width || y >= _height) return;
            
            _gridData[x, y]--;
            _debugTextGrid[x, y].text = _gridData[x, y].ToString();
        }

        private void UpCellValue(Vector3 worldPosition)
        {
            GetGridFromWorldPos(worldPosition, out int x, out int y);
            UpCellValue(x,y);
        }

        private void DownCellValue(Vector3 worldPosition)
        {
            GetGridFromWorldPos(worldPosition, out int x, out int y);
            DownCellValue(x,y);
        }
    }
}