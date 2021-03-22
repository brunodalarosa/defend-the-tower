using DG.Tweening;
using UnityEngine;

namespace TowerDefense.Scripts.Enemy
{
    [RequireComponent(typeof(Enemy))]
    public class EnemyMovementController : MonoBehaviour
    {
        public float _actualSpeed = 3f;

        private Enemy _enemy = null;
        private Transform _nextWaypoint;
        private int _levelWaypointIndex;

        private void Start()
        {
            _enemy = GetComponent<Enemy>();
            _actualSpeed = _enemy.Speed;
            _nextWaypoint = LevelWaypoints.Waypoints[_levelWaypointIndex];
            transform.DOLookAt(_nextWaypoint.position, 0.5f);
        }

        private void Update()
        {
            var dir = _nextWaypoint.position - transform.position;
            transform.Translate(dir.normalized * (_actualSpeed * Time.deltaTime), Space.World);

            if (Vector3.Distance(transform.position, _nextWaypoint.position) <= 0.2f) UpdateNextWaypoint();
        }

        private void UpdateNextWaypoint()
        {
            if (_levelWaypointIndex >= LevelWaypoints.Waypoints.Count - 1) //todo mudar para algo como: chegou no END
            {
                _enemy.ArrivedAtPortal();
                return;
            }

            _levelWaypointIndex++;
            _nextWaypoint = LevelWaypoints.Waypoints[_levelWaypointIndex];
            transform.DOLookAt(_nextWaypoint.position, 0.5f);
        }
    }
}
