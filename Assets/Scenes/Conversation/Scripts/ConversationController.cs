using System;
using System.Collections.Generic; 
using UnityEngine;

namespace Scenes.Conversation.Scripts
{
    public class ConversationController : MonoBehaviour
    {
        private const string EnteringStoryIdPrefName = "EnteringStoryID"; // the story ID set right before entering this scene
        private List<StorySegment> conversation;

        private void Start()
        {
            var enteringStoryId = (StoryId)PlayerPrefs.GetInt(EnteringStoryIdPrefName);
            
            // Get next story conversation
            var rawDialogue = GameController.Instance.StoryController.GetConversation(enteringStoryId).Dialogue;
            CreateConversation(rawDialogue);
            
            // fade out black UI 
        }

        void CreateConversation(string rawDialogue)
        {
            conversation = new List<StorySegment>();

            var sections = rawDialogue.Split("||");
            
            foreach (var s in sections)
            {
                Debug.Log("Section: " + s);
                
                var storySegment = new StorySegment();
                conversation.Add(storySegment); 
                storySegment.SpeakerId = ExtractSpeaker(s);
                storySegment.Dialogue = ExtractDialogue(s);
                storySegment.objectsToEnable = ExtractObjectsToEnable(s); 
                storySegment.musicToPlay = ExtractMusic(s);

                var result = JsonUtility.ToJson(storySegment);
                Debug.Log(result);
            }
        }
 
        private static string ExtractSpeaker(string s)
        {
            try { 
                return s.Substring(s.IndexOf('<') + 1, s.IndexOf('>') - s.IndexOf('<') - 1);
            }
            catch (Exception) {
                // 🙈
            }

            return null;
        }

        private string ExtractDialogue(string s)
        {
            // just grab everything after the last > or }
            var lastGator = s.LastIndexOf('>');
            var lastCurly = s.LastIndexOf('}');
            var endOfShenanigans = Mathf.Max(lastGator, lastCurly); // will be -1 if both missing, that is ok.
            var dialogue = s[(endOfShenanigans+1)..];
            return dialogue;
        }

        private string ExtractMusic(string s) {
            var sceneEdits = ExtractSceneEdits(s);
            // TODO when we have music kek
            return null;
        }

        private List<GameObject> ExtractObjectsToEnable(string s) {
            var sceneEdits = ExtractSceneEdits(s);
            // TODO
            return null;
        }

        private static string ExtractSceneEdits(string s)
        {
            try {
                return s.Substring(s.IndexOf('{') + 1, s.IndexOf('}') - s.IndexOf('{') - 1); 
            }
            catch (Exception) {
                // 🙈
            }

            return null;
        }
    }

    internal class StorySegment
    {
        /// <summary>
        /// Value should link to an ID in the Speakers list of the Set object
        /// </summary>
        public string SpeakerId;
        public string Dialogue; // includes the newline operator
        public List<GameObject> objectsToEnable;
        public string musicToPlay;
    }
}