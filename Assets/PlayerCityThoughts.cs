using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerCityThoughts : MonoBehaviour {
    [SerializeField] private TMP_Text textBox;
    [SerializeField] private float letterDelay = 0.1f;
    [SerializeField] private float textFadeSpeed = 1f;
    private float _totalDialogueTime;
    private float _timeTriggeredText = float.MinValue;
    
    private string _textToShow;
    private bool _textIsSpawning = false;
    
    // Start is called before the first frame update
    void Start() {
        textBox.text = string.Empty;
    }

    // Update is called once per frame
    void Update()
    {
        FaceCamera();  
        SpawnLetters();
        if (!_textIsSpawning)
            FadeOut();
    }

    private void SpawnLetters() {
        if (_textToShow == null) return;
        var timePassed = Time.time - _timeTriggeredText;
        var percThroughText = Mathf.Clamp01(timePassed / _totalDialogueTime);
        textBox.text = GetPercChars(_textToShow, percThroughText);
        _textIsSpawning = timePassed < _totalDialogueTime;
    }

    private void FadeOut() {
        textBox.alpha = Mathf.MoveTowards(textBox.alpha, 0, Time.deltaTime * textFadeSpeed);
    }

    private void FaceCamera() {
        transform.forward = Camera.main.transform.forward;
    }

    private static string GetPercChars(string s, float percThroughText) { 
        var numChars = Mathf.FloorToInt(s.Length * percThroughText);
        return s[..numChars];
    }

    public void ShowText(string text) {
        _textToShow = text;
        _timeTriggeredText = Time.time;
        _totalDialogueTime = _textToShow.Length * letterDelay;

        textBox.text = string.Empty;
        textBox.alpha = 1;
    }
}
