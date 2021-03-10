using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelWaypoints : MonoBehaviour
{
    public static List<Transform> Waypoints { get; private set; }

    private void Awake()
    {
        Waypoints = new List<Transform>(transform.childCount);

        for (var childIndex = 0; childIndex < transform.childCount; childIndex++)
            Waypoints.Add(transform.GetChild(childIndex));
    }
}
