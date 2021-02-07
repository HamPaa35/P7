using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable] public class MyEnumEvent : UnityEvent<LightManager.Rooms>
{
}
public class StoryMarkerEventManager : MonoBehaviour
{
    public MyEnumEvent StoryMarkerEntered;
    [SerializeField] private LightManager.Rooms _room;
    [SerializeField] private bool needsToWait;
    private bool active;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerTrigger") && !needsToWait)
        {
            StoryMarkerEntered.Invoke(_room);
        }
    }

    public void toggleNeedsToWait()
    {
        needsToWait = !needsToWait;
    }
}
