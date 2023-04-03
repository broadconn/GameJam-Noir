using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryTrigger : MonoBehaviour
{
    [SerializeField] private StoryId _storyId;

    public StoryId GetID()
    {
        return _storyId;
    }
}
