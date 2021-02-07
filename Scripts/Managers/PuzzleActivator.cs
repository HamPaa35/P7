using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PuzzleActivator : MonoBehaviour
{
    public UnityEvent ActivatePuzzle;
    private bool hasBeenActivated;
    [SerializeField] private bool needsToWait;
    private bool active;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasBeenActivated)
        {
            if (needsToWait && active)
            {
                hasBeenActivated = true;
                ActivatePuzzle.Invoke();
            }

            
            if (!needsToWait)
            {
                hasBeenActivated = true;
                ActivatePuzzle.Invoke();
            }
        }
    }

    public void toggleNeedsToWait()
    {
        needsToWait = !needsToWait;
    }

    public void toggleActive()
    {
        Debug.Log("toggled");
        active = !active;
    }
}