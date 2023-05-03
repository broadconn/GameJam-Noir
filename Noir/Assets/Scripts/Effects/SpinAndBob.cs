using System.Collections;
using System.Collections.Generic;
using UnityEngine; 

public class SpinAndBob : MonoBehaviour
{
    [SerializeField] private float BobSpeed = 0;
    [SerializeField] private float BobDist = 0;
    [SerializeField] private float YSpinSpeed = 0;

    private Transform myTransform;
    private float startYPos;
    private Vector3 startPos;
    
    // Start is called before the first frame update
    void Start()
    {
        myTransform = transform;
        startPos = myTransform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // bob
        var yOffset = Mathf.Sin(Time.time * BobSpeed) * BobDist; 
        var position = myTransform.position;
        position = new Vector3(position.x, startPos.y + yOffset, position.z);
        myTransform.position = position;
        
        // spin
        var newYAng = (Time.time * YSpinSpeed) % 360;
        var eulerAngles = myTransform.eulerAngles;
        eulerAngles = new Vector3(eulerAngles.x, newYAng, eulerAngles.z);
        myTransform.eulerAngles = eulerAngles;
    }
}
