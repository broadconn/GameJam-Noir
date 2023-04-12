using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class MistressCityToken : MonoBehaviour {
    [SerializeField] private SplineContainer splinePath;
    [SerializeField] private float moveSpeed = 0.2f;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update() {
        var t = (Time.time * moveSpeed) % 1f;
        transform.position = splinePath.Spline.EvaluatePosition(t);
    }
}
