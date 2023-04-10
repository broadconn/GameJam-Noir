using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DefaultNamespace {
    
    public class StoryController : MonoBehaviour {
        public static readonly string EnteringStoryIdPrefName = "EnteringStoryID"; // the story ID used by the Conversation scene to know what convo to show
        public static readonly string LastStoryIdPrefName = "LastStoryID"; // last conversation completed
        private StoryId? _lastSeenStoryId; // the last conversation seen. Should be set at the end of every conversation.
        
        [SerializeField] private List<StoryConversation> story; // should probably be in ConversationController. 

        private void Awake() {
            _lastSeenStoryId = (StoryId) PlayerPrefs.GetInt(LastStoryIdPrefName);
            print("Last story ID: " + _lastSeenStoryId);
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
    }
}