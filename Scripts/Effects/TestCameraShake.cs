using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCameraShake : MonoBehaviour
{

    public CameraShake cameraShake;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            StartCoroutine(cameraShake.StartShake(0.05f));
        }
        else if (Input.GetKeyDown(KeyCode.H))
        {
            StartCoroutine(cameraShake.StopShake(0.05f));
        }
    }
}
