using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class EnemyMovementController : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private Enemy _enemyRef = null;
    private Enemy EnemyRef => _enemyRef;
    
    [Space]
    [Header("Attributes")]
    
    public float _speed = 3f;

    private Transform nextWaypoint;
    private int levelWaypointIndex = 0;

    private void Start()
    {
        nextWaypoint = LevelWaypoints.Waypoints[levelWaypointIndex];
        transform.DOLookAt(nextWaypoint.position, 0.5f);
    }

    private void Update()
    {
        var dir = nextWaypoint.position - transform.position;
        transform.Translate(dir.normalized * (_speed * Time.deltaTime), Space.World);

        if (Vector3.Distance(transform.position, nextWaypoint.position) <= 0.2f) UpdateNextWaypoint();
    }

    private void UpdateNextWaypoint()
    {
        if (levelWaypointIndex >= LevelWaypoints.Waypoints.Count - 1) //todo mudar para algo como: chegou no END
        {
            EnemyRef.Die();
            return;
        }

        levelWaypointIndex++;
        nextWaypoint = LevelWaypoints.Waypoints[levelWaypointIndex];
        transform.DOLookAt(nextWaypoint.position, 0.5f);
    }
}
