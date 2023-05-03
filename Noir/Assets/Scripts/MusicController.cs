using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicController : MonoBehaviour {
    [SerializeField] private List<StoryMusicLink> storyMusicLinks;
    [SerializeField] private AudioSource audioSource;

    private float _overallVolume = 1f; // modified via the settings ui.
    private float _transitionTgtVolume = 1;
    private float _timeTriggeredFadeTransition = float.MinValue;
    private const float FadeTime = 0.8f; // should ideally link this up with the scene fader

    private void Start() { 
        SceneManager.sceneLoaded += SceneManagerOnSceneLoaded; // do this in Start so other GameControllers can be destroyed before registering their events
    }

    private void Update() {
        UpdateVolumeFade();
    }

    private void UpdateVolumeFade() {
        var timePassed = Time.time - _timeTriggeredFadeTransition;
        var percThrough = Mathf.Clamp01(timePassed / FadeTime);

        if (Math.Abs(percThrough - 1) < 0.001f) {
            if (Math.Abs(audioSource.volume - _overallVolume * _transitionTgtVolume) > 0.001f)
                audioSource.volume = _overallVolume * _transitionTgtVolume;
            return;
        }
        
        var tgtVol = Mathf.Lerp(0, 1, percThrough);
        if (_transitionTgtVolume == 0) tgtVol = 1 - tgtVol; // if target is 0 we need to reverse the lerp result
        var actualVolume = Mathf.Clamp01(tgtVol * _overallVolume); // apply volume setting. Clamp because I'm paranoid.
        audioSource.volume = actualVolume;
    }

    private void SceneManagerOnSceneLoaded(Scene scene, LoadSceneMode arg1) {
        FigureOutSongToPlay();
    }

    public void OnLocationChange() {
        FigureOutSongToPlay();
    }

    public void SetVolume(float f) {
        _overallVolume = f;
    }

    private void FadeInMusic() {
        _timeTriggeredFadeTransition = Time.time;
        _transitionTgtVolume = 1;
    }

    public void FadeOutMusic() {
        _timeTriggeredFadeTransition = Time.time;
        _transitionTgtVolume = 0;
    }

    private void FigureOutSongToPlay() {
        // get current location
        var loc = (CityLocation) PlayerPrefs.GetInt(MapController.LocationPrefName);
        if (loc == CityLocation.Undefined) loc = CityLocation.Diner;
 
        Enum.TryParse<GameScene>(SceneManager.GetActiveScene().name, out var scene);
        
        // get current storyId
        var nextStoryId = GameController.Instance.StoryController.GetNextStoryId();
        print("Location:" + loc + "|Scene: " + scene + "|Story entering: " + nextStoryId);

        var audioClip = storyMusicLinks.FirstOrDefault(sml => sml.Location == loc && sml.Scene == scene && sml.Id == nextStoryId) ??
                        storyMusicLinks.FirstOrDefault(sml => sml.Location == loc && sml.Scene == scene && sml.Id == StoryId.Undefined); // fall back to any matches with undefined story ids
        
        if (audioClip == null) {
            Debug.LogWarning("Couldnt find audio for: Location:" + loc + "|Scene: " + scene + "|Story entering: " + nextStoryId);
            return; // i dunno
        }

        audioSource.clip = audioClip.Music;
        audioSource.Play();
        
        FadeInMusic();
    }
}

[Serializable]
public class StoryMusicLink {
    [SerializeField] private string note; // just used as a comment box in the UI
    [SerializeField] private CityLocation location;
    [SerializeField] private GameScene scene;
    [SerializeField] private StoryId storyId; // Use this as an optional level of granularity when choosing which clip to play.
    [SerializeField] private AudioClip music;

    public CityLocation Location => location;
    public GameScene Scene => scene;
    public StoryId Id => storyId;
    public AudioClip Music => music;
}

public enum GameScene {
    City,
    Conversation
}