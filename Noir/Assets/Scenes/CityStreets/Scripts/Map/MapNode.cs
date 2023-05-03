using System;
using System.Collections;
using System.Collections.Generic; 
using UnityEngine;

public class MapNode : MonoBehaviour {
    [SerializeField] private CityLocation citySpawnLocation; 
    [SerializeField] private Transform worldDestination;
    
    // limited to 4 directions so transitions can be mapped to WASD
    [SerializeField] private MapNode navUp;
    [SerializeField] private MapNode navLeft;
    [SerializeField] private MapNode navDown;
    [SerializeField] private MapNode navRight;

    public CityLocation CitySpawnLocation => citySpawnLocation;
    public Transform WorldDestination => worldDestination;
    public MapNode NavUp => navUp;
    public MapNode NavLeft => navLeft;
    public MapNode NavDown => navDown;
    public MapNode NavRight => navRight;

    public List<(ArrowDir Direction, MapNode Node)> NavNodes { get; private set; }

    private void Awake() {
        NavNodes = new List<(ArrowDir, MapNode)> { (ArrowDir.Up, navUp), (ArrowDir.Right, navRight), (ArrowDir.Down, navDown), (ArrowDir.Left, navLeft) };
    }
}
