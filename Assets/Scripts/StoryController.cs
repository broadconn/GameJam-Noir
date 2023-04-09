using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DefaultNamespace {
    public class StoryController : MonoBehaviour {
        [SerializeField] private List<StoryConversation> story;
        private const string LastStoryIdPrefName = "StoryID";
        private StoryId? _lastSeenStoryId; // the last conversation seen. Should be set at the end of every conversation.

        private void Awake() {
            _lastSeenStoryId = (StoryId) PlayerPrefs.GetInt(LastStoryIdPrefName);
        }

        public StoryId? GetLastStoryId()
        {
            return _lastSeenStoryId;
        }

        public StoryId GetNextStoryId()
        {
            if (_lastSeenStoryId is null) 
                return StoryId.Intro;

            for (var i = 0; i < story.Count-1; i++)
            {
                var storyId = story[i].Id;
                if (storyId == _lastSeenStoryId)
                    return story[i+1].Id;
            }
        
            return StoryId.Intro; // should only reach here if they've finished the game
        }

        public void StartConversation()
        {
            // could do a fancy fade out here
            SceneManager.LoadScene("Conversation");
        }

        public StoryConversation GetConversation(StoryId storyId)
        { 
            return story.First(s => s.Id == storyId); 
        }

        public StoryConversation GetNextConversation()
        {
            var storyId = GetNextStoryId();
            return story.First(s => s.Id == storyId); 
        }
    }
}