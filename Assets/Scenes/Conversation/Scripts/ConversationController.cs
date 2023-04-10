using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace Scenes.Conversation.Scripts
{
    public class ConversationController : MonoBehaviour
    {
        private List<StorySegment> conversation = new();
        private StorySegment curDialogue;

        [SerializeField] private TMP_Text userText;
        [SerializeField] private TMP_Text dialogueText;
        [SerializeField] private GameObject nextIndicator;
        [SerializeField] private float letterDelay = 0.1f;
        private float totalDialogueTime;
        private float timeTriggeredText = float.MinValue;
        private int convoIndex = -1;
        private bool textSpawning = false;

        [SerializeField] private Transform setsParent;
        private Transform setSpeakers;

        private StoryId thisConversationId;

        private void Awake() {
            userText.text = string.Empty;
        }

        private void Start() {
            thisConversationId = (StoryId)PlayerPrefs.GetInt(StoryController.EnteringStoryIdPrefName); 
            var rawDialogue = GameController.Instance.StoryController.GetConversation(thisConversationId).Dialogue;
            conversation = CreateConversation(rawDialogue);
            PrepareBgSet(thisConversationId);
            TriggerNextDialogue();
        }

        private void PrepareBgSet(StoryId storyId) {
            // disable all sets
            foreach (Transform set in setsParent) {
                set.gameObject.SetActive(false);
            }
            
            // enable the set for this story
            var sets = setsParent.GetComponentsInChildren<ConversationSet>(includeInactive: true);
            var relevantSet = sets.First(s => s.StoryIds == storyId);
            setSpeakers = relevantSet.SpeakersParent;
            relevantSet.gameObject.SetActive(true);
        }

        private void Update() {

            if (Input.GetKeyDown(KeyCode.Space)) {
                if (convoIndex < conversation.Count - 1) {
                    if (!textSpawning)
                        TriggerNextDialogue();
                    else
                        timeTriggeredText = float.MinValue; // insta-complete the current spawning words
                }
                else {
                    EndConversation();
                }
            }

            if (curDialogue == null) return;
            var timePassed = Time.time - timeTriggeredText;
            textSpawning = timePassed < totalDialogueTime;
            var percThroughText = Mathf.Clamp01(timePassed / totalDialogueTime);
            dialogueText.text = GetPercChars(curDialogue.Dialogue, percThroughText);
            nextIndicator.SetActive(!textSpawning);
        }

        private void EndConversation() {
            GameController.Instance.StoryController.SetLastStoryId(thisConversationId);
            GameController.Instance.SceneFader.FadeToScene(GameController.CitySceneName);
        }

        private static string GetPercChars(string s, float percThroughText) { 
            var numChars = Mathf.FloorToInt(s.Length * percThroughText);
            return s[..numChars];
        }

        private void TriggerNextDialogue() {
            timeTriggeredText = Time.time;
            convoIndex++;
            curDialogue = conversation[convoIndex];
            userText.text = curDialogue.SpeakerId == null ? userText.text : MapSpeakerIdToName(curDialogue.SpeakerId);
            userText.gameObject.SetActive(!curDialogue.Dialogue.StartsWith('[')); // remove speaker name for internal speech

            totalDialogueTime = curDialogue.Dialogue.Length * letterDelay;
        }

        private static string MapSpeakerIdToName(string speakerId) {
            return speakerId switch {
                "Me" => "The Protagonist",
                "Wd" => "The Client",
                "Kd" => "Weird kid",
                _ => "[" + speakerId + "]"
            };
        }

        List<StorySegment> CreateConversation(string rawDialogue)
        {
            var convo = new List<StorySegment>();

            var sections = rawDialogue.Split("||");
            
            foreach (var s in sections)
            {
                var storySegment = new StorySegment();
                convo.Add(storySegment); 
                storySegment.SpeakerId = ExtractSpeaker(s);
                storySegment.Dialogue = ExtractDialogue(s);
                storySegment.objectsToEnable = ExtractObjectsToEnable(s); 
                storySegment.musicToPlay = ExtractMusic(s);
            }

            return convo;
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

        private static string ExtractDialogue(string s)
        {
            // just grab everything after the last > or }
            var lastGator = s.LastIndexOf('>');
            var lastCurly = s.LastIndexOf('}');
            var endOfShenanigans = Mathf.Max(lastGator, lastCurly); // will be -1 if both missing, that is ok.
            var dialogue = s[(endOfShenanigans+1)..];
            dialogue = dialogue.Replace("|", Environment.NewLine);
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