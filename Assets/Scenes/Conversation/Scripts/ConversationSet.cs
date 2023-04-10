using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class ConversationSet : MonoBehaviour
{
    [SerializeField] private StoryId storyIds; 
    [SerializeField] private Transform speakersParent;
    [SerializeField] private Transform backgroundParent;
    // TODO: set music reference somehow

    public StoryId StoryIds => storyIds;
    public Transform SpeakersParent => speakersParent;
    public Transform BackgroundParent => backgroundParent;
}
