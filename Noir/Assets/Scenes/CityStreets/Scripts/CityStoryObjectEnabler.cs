using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityStoryObjectEnabler : MonoBehaviour {
    public StoryId IdToActivateOn;

    private void Awake() {
        var nextStory = GameController.Instance.StoryController.GetNextStoryId();
        if(nextStory != IdToActivateOn)
            gameObject.SetActive(false);
    }
}
