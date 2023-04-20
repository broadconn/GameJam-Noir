using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
    
/// <summary>
/// Kind of a weird class. Some aspects make sense to be global (story id order), some only need to be in the conversation scene (story dialogue).
/// TODO: refactor at some point to have a clearer separation of concerns.
/// </summary>
public class StoryController : MonoBehaviour {
    public static readonly string EnteringStoryIdPrefName = "EnteringStoryID"; // the story ID used by the Conversation scene to know what convo to show
    public static readonly string LastStoryIdPrefName = "LastStoryID";  // the last conversation completed. Gets set at the end of every conversation.
    private StoryId? _lastSeenStoryId; 
    
    [SerializeField] private List<StoryConversation> story; 

    private void Awake() {
        _lastSeenStoryId = (StoryId) PlayerPrefs.GetInt(LastStoryIdPrefName);
    }
    
    public void StartConversation(StoryId storyId) {
        PlayerPrefs.SetInt(EnteringStoryIdPrefName, (int)storyId);
        GameController.Instance.SceneFader.FadeToScene(GameController.ConversationSceneName);
    }

    public StoryId? GetLastStoryId() {
        return _lastSeenStoryId;
    }

    /// <summary>
    /// Last story conversation finished
    /// </summary>
    /// <param name="storyId"></param>
    public void SetLastStoryId(StoryId storyId) {
        PlayerPrefs.SetInt(LastStoryIdPrefName, (int) storyId);
        _lastSeenStoryId = storyId;
    }

    public StoryId GetNextStoryId() {
        if (_lastSeenStoryId is null) 
            return StoryId.Intro;

        for (var i = 0; i < story.Count - 1; i++)
        {
            var storyId = story[i].Id;
            if (storyId == _lastSeenStoryId)
                return story[i+1].Id;
        }
    
        return StoryId.Intro; // should only reach here if they've finished the game
    }

    public StoryConversation GetConversation(StoryId storyId) { 
        return story.First(s => s.Id == storyId); 
    }

    public StoryConversation GetNextConversation() {
        var storyId = GetNextStoryId();
        return story.First(s => s.Id == storyId); 
    }
    
    public CityLocation StoryIdToLocation(StoryId? storyId) {
        switch (storyId) {
            case null:
                return CityLocation.Undefined;
            case StoryId.Intro:
            case StoryId.Diner:
                return CityLocation.Diner;
            case StoryId.Morgue:
                return CityLocation.Morgue;
            case StoryId.Bar:
            case StoryId.Mistress:
            case StoryId.Eavesdrop:
                return CityLocation.Bar;
            case StoryId.Warehouse:
                return CityLocation.Warehouse;
        }
        return CityLocation.Undefined;
    }
}