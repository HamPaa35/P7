using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GazeAwareManager : MonoBehaviour
{
    // Information from/on children
    private Transform gazeTransform;
    private int stilLookedAt = 0;
    private GazeAwareComms[] gazeAwareChildren;
    private GazeAwareComms[] latestResponders = new GazeAwareComms[100];
    private float reenableDelay = 8;

    // Information deciding interaction with light manager
    private float currentGazeTimer;
    private int gazeID = 0;
    [SerializeField][Tooltip("How long the player has to look at a gazeAware object before the light will stare at it.")] 
    private float objectGazeReactionDelay = 3; // How long the player has to look at an object before the light will stare at it.
    
    // Light manager
    [SerializeField] private LightManager lights;
    
    
    // Start is called before the first frame update
    void Start()
    {
        gazeAwareChildren = gameObject.GetComponentsInChildren<GazeAwareComms>(false);
    }

    // Update is called once per frame
    void Update()
    {
    }

    // Private functions
    private IEnumerator CheckTimer(int id) // Checks if the object has been gazed at long enough
    {
        yield return new WaitForSeconds(objectGazeReactionDelay);
        if (gazeID == id && stilLookedAt > 0) // Check if the focus object is still the same
        {
            latestResponders[id].Disable();
            StartCoroutine(ReenableComms(latestResponders[id], Time.time));
            lights.SetCurrentLightGazeToObject(gazeTransform, true);
        }
    } // Calls ReenableComms()

    private IEnumerator ReenableComms(GazeAwareComms comms, float myTime) // Reenables comms after a delay
    {
        yield return new WaitForSeconds(reenableDelay);
        comms.Enable();
    }
    
    // Public functions
    public void SetCurrentTrans(GameObject awareObject, GazeAwareComms comms) // Called from comms
    {
        stilLookedAt += 1;
        gazeTransform = awareObject.transform;
        gazeID = (gazeID + 1) % 100;
        latestResponders[gazeID] = comms;
        currentGazeTimer = Time.time;
        StartCoroutine(CheckTimer(gazeID));
    } // Calls CheckTimer()
    
    public void ForgetCurrentTrans() // Called from comms
    {
        stilLookedAt -= 1;
    }

    public void Disable()
    {
        stilLookedAt = 0;
        foreach (var i in gazeAwareChildren)
        {
            i.Disable();
        }
    }
    public void Enable()
    {
        foreach (var i in gazeAwareChildren)
        {
            i.Enable();
        }
    }
}
