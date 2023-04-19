using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CitySpawnLocation : MonoBehaviour {
    [SerializeField] private StoryId cityLocation;  // "StoryId" is kinda confusing here, but it represents a city location. Using the same enum cuz it makes it easy to sync them up.
    public StoryId CityLocation => cityLocation;
}
