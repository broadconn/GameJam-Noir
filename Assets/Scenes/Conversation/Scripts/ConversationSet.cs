using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    private List<ConversationSpeaker> speakers;

    private void Awake() {
        speakers = speakersParent.GetComponentsInChildren<ConversationSpeaker>(includeInactive: true).ToList();
    }

    public void Prepare() {
        // disable / hide everything that could get revealed as part of a conversation
        // e.g. background, speakers
        backgroundParent.gameObject.SetActive(false);

        foreach (var speaker in speakers) {
            speaker.Hide();
        }
    }

    public void ShowThing(string id) {
        if (id == "BG") {  
            backgroundParent.gameObject.SetActive(true); // todo nice face
            return;
        }
        
        var relevantThing = speakers.FirstOrDefault(s => s.id == id); 
        if (relevantThing == null) return;
        relevantThing.Show();
    }

    public void SetPersonSpeaking(string personId) {
        foreach (var speaker in speakers.Where(speaker => speaker.IsVisible)) {
            speaker.SetSpeaking(speaker.id == personId);
        }
    }
}
