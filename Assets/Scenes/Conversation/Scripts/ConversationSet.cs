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

    private List<ConversationSpeaker> _speakers; 

    public void Prepare() {
        backgroundParent.gameObject.SetActive(false);

        _speakers = speakersParent.GetComponentsInChildren<ConversationSpeaker>(includeInactive: true).ToList();
        foreach (var speaker in _speakers) {
            speaker.Hide();
        }
    }

    public void ShowThing(string id) {
        if (id == "BG") {  
            backgroundParent.gameObject.SetActive(true); // todo nice fade
            return;
        }
        
        var speakerWithId = _speakers.FirstOrDefault(s => s.id == id); 
        if (speakerWithId == null) return;
        speakerWithId.Show();
    }

    public void SetPersonSpeaking(string personId) {
        foreach (var speaker in _speakers.Where(speaker => speaker.IsVisible)) {
            speaker.SetSpeaking(speaker.id == personId);
        }
    }
}
