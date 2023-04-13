using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace Scenes.Conversation.Scripts
{
    public class ConversationController : MonoBehaviour
    {
        private List<StorySegment> _conversation = new();
        private StorySegment _curDialogue;

        [SerializeField] private TMP_Text userText;
        [SerializeField] private TMP_Text dialogueText;
        [SerializeField] private GameObject nextIndicator;
        [SerializeField] private float letterDelay = 0.1f;
        private float _totalDialogueTime;
        private float _timeTriggeredText = float.MinValue;
        private int _convoIndex = -1;
        private bool _textIsSpawning = false;

        [SerializeField] private Transform setsParent;

        private StoryId _thisConversationId;
        private ConversationSet _currentSet;

        private void Awake() {
            userText.text = string.Empty;
        }

        private void Start() {
            _thisConversationId = (StoryId)PlayerPrefs.GetInt(StoryController.EnteringStoryIdPrefName); 
            var rawDialogue = GameController.Instance.StoryController.GetConversation(_thisConversationId).Dialogue;
            _conversation = CreateConversation(rawDialogue);
            PrepareBgSet(_thisConversationId);
            TriggerNextDialogue();
        }

        private List<StorySegment> CreateConversation(string rawDialogue)
        {
            var sections = rawDialogue.Split("||");
            var lastSpeaker = (string) null;

            var c = new List<StorySegment>();
            foreach (var s in sections)
            {
                var storySegment = new StorySegment {
                    SpeakerId = ExtractSpeaker(s) ?? lastSpeaker,
                    Dialogue = ExtractDialogue(s),
                    ThingsToShow = ExtractThingsToShow(s),
                    MusicToPlay = ExtractMusic(s)
                };
                c.Add(storySegment);
                lastSpeaker = storySegment.SpeakerId;
            }

            return c;
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

        /// <summary>
        /// e.g. `show:Me, show:Wd, show:BG, music` 
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private List<StoryThing> ExtractThingsToShow(string s) {
            var result = new List<StoryThing>();
            var sceneEdits = ExtractSceneEdits(s);
            if (sceneEdits == null) 
                return result;
            
            var things = sceneEdits.Split(',').Select(t => t.Trim());
            foreach (var thing in things) {
                if (thing.Contains(':')) {
                    var sceneThing = thing.Split(':');
                    if (sceneThing[0] != "show") continue; // I think this is always 'show'? Finish the script u dingus
                    var thingId = sceneThing[1];
                    result.Add(new StoryThing{ Type = StoryThingType.SceneThing, ID = thingId });
                }
                else 
                    result.Add(new StoryThing{ Type = StoryThingType.Music });
            }
            
            return result;
        }

        private string ExtractMusic(string s) {
            //var sceneEdits = ExtractSceneEdits(s);
            // TODO when we have music kek
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

        private void PrepareBgSet(StoryId storyId) {
            // disable all sets
            foreach (Transform set in setsParent) {
                set.gameObject.SetActive(false);
            }
            
            // enable the set for this story
            var sets = setsParent.GetComponentsInChildren<ConversationSet>(includeInactive: true);
            _currentSet = sets.First(s => s.StoryIds == storyId);
            _currentSet.Prepare();
            _currentSet.gameObject.SetActive(true);
        }

        private void Update() {

            if (Input.GetKeyDown(KeyCode.Space)) {  
                if (_convoIndex < _conversation.Count - 1 || _textIsSpawning) {
                    if (!_textIsSpawning)
                        TriggerNextDialogue();
                    else
                        _timeTriggeredText = float.MinValue; // insta-complete the current spawning words
                }
                else {
                    EndConversation();
                    return;
                }
            }

            if (_curDialogue == null) return;
            
            var timePassed = Time.time - _timeTriggeredText;
            var percThroughText = Mathf.Clamp01(timePassed / _totalDialogueTime);
            dialogueText.text = GetPercChars(_curDialogue.Dialogue, percThroughText);
            
            _textIsSpawning = timePassed < _totalDialogueTime;
            nextIndicator.SetActive(!_textIsSpawning); // the little '>' in the bottom right of the UI
        }

        private void EndConversation() {
            GameController.Instance.StoryController.SetLastStoryId(_thisConversationId);
            GameController.Instance.SceneFader.FadeToScene(GameController.CitySceneName);
        }

        private static string GetPercChars(string s, float percThroughText) { 
            var numChars = Mathf.FloorToInt(s.Length * percThroughText);
            return s[..numChars];
        }

        private void TriggerNextDialogue() {
            _timeTriggeredText = Time.time;
            
            _convoIndex++;
            _curDialogue = _conversation[_convoIndex];
            
            SetSceneElements(_curDialogue);

            _totalDialogueTime = _curDialogue.Dialogue.Length * letterDelay;
        }

        private void SetSceneElements(StorySegment dialogue) {
            var speechIsInternal = SpeechIsInternal(dialogue.Dialogue);
            userText.text = MapSpeakerIdToName(dialogue.SpeakerId);
            userText.gameObject.SetActive(!speechIsInternal); // hide speaker name for internal speech

            foreach (var thing in dialogue.ThingsToShow) {
                _currentSet.ShowThing(thing.ID);
            }
            
            _currentSet.SetPersonSpeaking(speechIsInternal ? null : dialogue.SpeakerId);
        }

        private static bool SpeechIsInternal(string speech) {
            return speech.StartsWith('[');
        }

        private static string MapSpeakerIdToName(string speakerId) {
            return speakerId switch {
                "Me" => "The Protagonist",
                "Wd" => "The Client",
                "Kd" => "Weird kid",
                "Bt" => "Bartender",
                "Br" => "Bert",
                _ => "[" + speakerId + "]"
            };
        }
    }

    internal class StorySegment {
        /// <summary>
        /// Value should link to an ID in the Speakers list of the Set object
        /// </summary>
        public string SpeakerId;
        public string Dialogue; // includes the newline operator '|'
        public List<StoryThing> ThingsToShow;
        public string MusicToPlay;
    }

    internal class StoryThing {
        public string ID;
        public StoryThingType Type;
    }

    internal enum StoryThingType {
        SceneThing,
        Music
    }
}