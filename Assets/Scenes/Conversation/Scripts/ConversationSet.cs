using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConversationSet : MonoBehaviour
{
    [SerializeField] private StoryId storyId;
    [SerializeField] private Transform speakersParent;
    [SerializeField] private Transform backgroundParent;
    // TODO: set music reference somehow

    public StoryId StoryId => storyId;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
