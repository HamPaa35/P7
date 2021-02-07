using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoorScript : MonoBehaviour
{
    [SerializeField]
    private Transform DoorPosition;

    private GameObject audioManagerObject;
    private AudioManager _audioManager;

    private bool isOpen;

    [SerializeField] private Transform closed;
    [SerializeField] private Transform open;

    private void Start()
    {
        audioManagerObject = GameObject.Find("AudioManager");
        _audioManager = audioManagerObject.GetComponent<AudioManager>();
        Debug.Log("start " + _audioManager);
    }

    [ContextMenu("Open the Door.")]
    public void OpenDoor()
    {
        if (isOpen)
        {
            return;
        }
        DoorPosition.position = open.position;
        DoorPosition.rotation = open.rotation;
        Debug.Log("open door " +_audioManager);
        _audioManager.Play("DoorOpen");
        isOpen = true;
    }

    [ContextMenu("Close the door.")]
    public void CloseDoor()
    {
        if (!isOpen)
        {
            return;
        }
        DoorPosition.position = closed.position;
        DoorPosition.rotation = closed.rotation;
        _audioManager.Play("FrontDoor");
        isOpen = false;
    }
}
