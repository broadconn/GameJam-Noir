using System;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshController : MonoBehaviour { 
    public static bool CheckPointIsOnMesh(Vector3 point) {
        return NavMesh.SamplePosition(point, out var hit, Mathf.Infinity, NavMesh.AllAreas); 
    }
}
