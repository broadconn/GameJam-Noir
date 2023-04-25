using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneFader : MonoBehaviour
{
    [Header("Scene transition fade")]
    [SerializeField] private Image blackFadeImg;
    [SerializeField] private float fadeTime = 1; 
    private float _timeTriggeredTransition = float.MinValue;
    private string _sceneToGoTo;
    private FadeDirection _fadeDir = FadeDirection.In;
    private bool _isTransitioning = false;
    private bool _isLoadingLevel = false;

    private void Awake() {
        SceneManager.sceneLoaded += SceneManagerOnsceneLoaded;
    }

    private void SceneManagerOnsceneLoaded(Scene arg0, LoadSceneMode arg1) { 
        _fadeDir = FadeDirection.In;
        _timeTriggeredTransition = Time.time;
        _isLoadingLevel = false;
    }
 
    void Update()
    {
        var timePassed = Time.time - _timeTriggeredTransition;
        var transitionPercent = Mathf.Clamp01(timePassed / fadeTime);
        
        UpdateSceneTransition(timePassed, transitionPercent);

        if (_fadeDir == FadeDirection.In && transitionPercent >= 1) {
            _isTransitioning = false; // enable the next scene change
        }
    }

    public void FadeToScene(string sceneName) {
        if (_isTransitioning) return;
        _isTransitioning = true;
        
        _sceneToGoTo = sceneName;
        _fadeDir = FadeDirection.Out;
        _timeTriggeredTransition = Time.time;
        
        GameController.Instance.MusicController.FadeOutMusic();
    }

    private void UpdateSceneTransition(float timePassed, float transitionPercent) {
        if (_fadeDir == FadeDirection.In)
            transitionPercent  = 1 - transitionPercent; // invert fade amount if fading in

        blackFadeImg.color = new Color(blackFadeImg.color.r, blackFadeImg.color.g, blackFadeImg.color.b,
            transitionPercent);

        // handle the scene change after we fade out
        if (timePassed < fadeTime || _fadeDir == FadeDirection.In || _isLoadingLevel) return;
        _isLoadingLevel = true;
        StartCoroutine(LoadAndGoToScene());
    }

    private IEnumerator LoadAndGoToScene() {
        yield return null;
        
        var sceneLoadOperation = SceneManager.LoadSceneAsync(_sceneToGoTo);
        sceneLoadOperation.allowSceneActivation = false; 
        
        while (!sceneLoadOperation.isDone) {  
            // can use sceneLoadOperation.progress for a loading indicator
            if (sceneLoadOperation.progress >= 0.9f) { // progress stops at 0.9 if allowSceneActivation = false
                sceneLoadOperation.allowSceneActivation = true;
            }
            yield return null;
        }
    }
}

internal enum FadeDirection
{
    In,
    Out
}
