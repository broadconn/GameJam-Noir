using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityStoryObject : MonoBehaviour {
    public StoryId IdToActivateOn;

    private void Awake() {
        var nextStory = GameController.Instance.StoryController.GetNextStoryId();
        print("Heading towards:" + nextStory);
        if(nextStory != IdToActivateOn)
            gameObject.SetActive(false);
    }
}
