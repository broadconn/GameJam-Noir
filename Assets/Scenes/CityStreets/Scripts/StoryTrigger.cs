using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class StoryTrigger : MonoBehaviour
{
    [SerializeField] private StoryId storyId;

    public StoryId GetID()
    {
        return storyId;
    }

    private void OnTriggerEnter(Collider other)
    {
        GameController.Instance.StartConversation(storyId);
    }
}
