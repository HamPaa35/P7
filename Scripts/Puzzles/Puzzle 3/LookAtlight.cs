using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tobii.Gaming;

[RequireComponent(typeof(GazeAware))]

public class LookAtlight : MonoBehaviour
{
    [SerializeField] private float lookAtTimer;
    [SerializeField] private float didNotLookTimer;
    [SerializeField] private float blinkDimAmount = 1;
    [SerializeField] private float blinkInterval = 1;
    [SerializeField] private AdjustIntensity lightIntensity;
    [SerializeField] private EvalCondition _evalCondition;
    
    private GazeAware _gazeAwareComponent;
    private float timer;
    private float notLookTimer;
    private float blinkTimer;
    private bool blink = false;
    private bool lookedAt = false;
    private bool dim = true;
    private bool reset = false;
    private bool evalFalseRunOnce = false;

    // Start is called before the first frame update
    void Start()
    {
        _gazeAwareComponent = GetComponent<GazeAware>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_evalCondition.EyeTracking)
        {
            if (!_gazeAwareComponent.HasGazeFocus) //&& !notLookingAtBooks)
            {
                lookedAt = false;
                if (blink)
                {
                    notLookTimer += Time.deltaTime;
                    if (notLookTimer > didNotLookTimer)
                    {
                        lightIntensity.SetLightIntensity(blinkDimAmount);
                        Debug.Log("DIM");
                        blinkTimer += Time.deltaTime;
                        if (blinkTimer > blinkInterval)
                        {
                            lightIntensity.ResetLightIntensity();
                            blinkTimer = 0;
                            notLookTimer = 0;
                            Debug.Log("Reset");
                        }

                        /*if (!dim)
                        {
                            reset = true;
                            dim = false;
                        }
                        else if (!reset)
                        {
                            dim = true;
                            reset = false;
                        }

                        Debug.Log(dim + " " + reset);
                        Debug.Log("HELP!!!!!!!");
                        if (dim)
                        {
                            lightIntensity.SetLightIntensity(blinkDimAmount);
                            dim = false;
                            Debug.Log("Dim" + dim);
                            notLookTimer = 0;
                        }
                        else if (reset)
                        {
                            lightIntensity.ResetLightIntensity();
                            reset = false;
                            Debug.Log("Reset" + reset);
                            notLookTimer = 0;
                        }

                        Debug.Log("IN THE BUTTOM");*/
                    }
                }
            }
            else if (_gazeAwareComponent.HasGazeFocus)
            {
                blink = false;
                timer += Time.deltaTime;
                if (timer > lookAtTimer)
                {
                    lightIntensity.ResetLightIntensity();
                    lookedAt = true;
                    timer = 0;
                }
            }
        }
        else if(!_evalCondition.EyeTracking && !evalFalseRunOnce)
        {
            lookedAt = true;
            evalFalseRunOnce = true;
            Debug.Log("No eye tracking");
        }
    }

    public bool LookedAt => lookedAt;

    public bool Blink
    {
        get => blink;
        set => blink = value;
    }
}
