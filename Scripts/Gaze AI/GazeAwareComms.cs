using System.Collections;
using System.Collections.Generic;
using Tobii.Gaming;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class MyCommsEvent : UnityEvent<GameObject, GazeAwareComms>
{
}
public class GazeAwareComms : MonoBehaviour
{
    [SerializeField] private MyCommsEvent commsEvent;
    
    [SerializeField] private UnityEvent LookedAt;
    [SerializeField] private UnityEvent notLookedAt;

    private GazeAware _gazeAware;
    private bool _disable = false;
    private bool stillLookedAt = false;
    // Start is called before the first frame update
    void Start()
    {
        _gazeAware = gameObject.GetComponent<GazeAware>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_disable)
        {
            return;
        }
        
        // This object is looked at
        if (_gazeAware.HasGazeFocus && !stillLookedAt)
        {
            stillLookedAt = true;
            LookedAt.Invoke();
            commsEvent.Invoke(gameObject, this);
        }

        // This object is not looked at
        if (stillLookedAt && !_gazeAware.HasGazeFocus)
        {
            stillLookedAt = false;
            notLookedAt.Invoke();
        }
    }

    // Public functions
    public void Disable()
    {
        stillLookedAt = false;
        _disable = true;
    }

    public void Enable()
    {
        _disable = false;
    }
}
