using System.Collections;
using System.Collections.Generic; 
using UnityEngine;
using UnityEngine.Serialization;

public class MapNode : MonoBehaviour {
    [SerializeField] private StoryId citySpawnLocation; // links to a CitySpawnLocation's cityLocation
    [SerializeField] private Transform worldDestination;
    [SerializeField] private MapNode navUp;
    [SerializeField] private MapNode navLeft;
    [SerializeField] private MapNode navDown;
    [SerializeField] private MapNode navRight;

    public StoryId CitySpawnLocation => citySpawnLocation;
}
