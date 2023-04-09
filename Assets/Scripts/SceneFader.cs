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

    private void Awake() {
        SceneManager.sceneLoaded += SceneManagerOnsceneLoaded;
    }

    private void SceneManagerOnsceneLoaded(Scene arg0, LoadSceneMode arg1) { 
        _fadeDir = FadeDirection.In;
        _timeTriggeredTransition = Time.time;
    }

    // Start is called before the first frame update
    void Start()
    {
        // fade in
        // _fadeDir = FadeDirection.In;
        // _timeTriggeredTransition = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSceneTransition();
    }

    public void FadeToScene(string sceneName)
    {
        _sceneToGoTo = sceneName;
        _fadeDir = FadeDirection.Out;
        _timeTriggeredTransition = Time.time;
    }

    private void UpdateSceneTransition() {
        var timePassed = Time.time - _timeTriggeredTransition;
        var transitionPercent = Mathf.Clamp01(timePassed / fadeTime);
        
        if (_fadeDir == FadeDirection.In)
            transitionPercent  = 1 - transitionPercent; // invert fade amount if fading in

        blackFadeImg.color = new Color(blackFadeImg.color.r, blackFadeImg.color.g, blackFadeImg.color.b,
            transitionPercent);

        // handle the scene change after we fade out
        if (timePassed > fadeTime && _fadeDir != FadeDirection.In)
            StartCoroutine(LoadAndGoToScene());
    }

    private IEnumerator LoadAndGoToScene() {
        yield return null;
        
        var sceneLoadOperation = SceneManager.LoadSceneAsync(_sceneToGoTo);
        sceneLoadOperation.allowSceneActivation = false; 
        
        while (!sceneLoadOperation.isDone) {  
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
