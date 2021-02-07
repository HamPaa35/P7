using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvalCondition : MonoBehaviour
{
    [SerializeField] private bool eyeTracking;
    [SerializeField] private LightManager _lightManager;

    private bool eyeTrackTrueRunOnce = false;
    private bool eyeTrackFalseRunOnce = false;

    [SerializeField] private LightLookingAtMold _lightLookingAtMold;
    
    // Start is called before the first frame update
    void Start()
    {
        if (!eyeTracking)
        {
            _lightLookingAtMold.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (eyeTracking && !eyeTrackTrueRunOnce)
        {
            //Should we even do anything here?
            eyeTrackTrueRunOnce = true;
        } 
        else if (!eyeTracking && !eyeTrackFalseRunOnce)
        {
            //Set the light to idle
            eyeTrackFalseRunOnce = true;
        }
    }

    public bool EyeTracking
    {
        get => eyeTracking;
        set => eyeTracking = value;
    }
}
