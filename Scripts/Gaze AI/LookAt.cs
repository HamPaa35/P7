using System;
using System.Collections;
using System.Collections.Generic;
using Tobii.GameIntegration.Net;
using Tobii.Gaming;
using UnityEngine;
using UnityEngine.Assertions.Comparers;
using UnityEngine.Events;
using UnityEngine.UIElements;
using GazePoint = Tobii.Gaming.GazePoint;
using UnityEngine.Rendering.HighDefinition;
using Random = UnityEngine.Random;

public class LookAt : MonoBehaviour
{
    
    // General
    private Transform thisTrans;
    private Camera cam;
    [SerializeField][Tooltip("Where the light will look when it is supposed to look at the player.")]
    private Transform PlayerCharacter;
    [SerializeField] private MainEventManager _manager;
    private float altLookTimer = 0;
    [SerializeField] [Tooltip("The number of iterations an alternate mode will last before the light resets.")]
    private float altContinueTime = 2f;

    
    // Conditions
    [SerializeField]
    private EvalCondition conditionScript;
    private bool _eyetracking;

    // Disabler!
    [SerializeField]
    private bool _disable = false;
    
    // Switch variables
    public enum LookTypes
    {
        followGaze,
        stareAtGaze,
        idle,
        stareAtObject,
        distractingGaze,
        alternateObjects,
        lookAtPlayer
    }
    [Header("State Variables")]
    [SerializeField][Tooltip("The current 'mode' of the light.")]
    private LookTypes currentLookAt = LookTypes.followGaze;
    [SerializeField][Tooltip("The 'mode' the light will reset to after using an 'alt mode'.")]
    private LookTypes resetLookAt = LookTypes.followGaze;
    [SerializeField][Tooltip("Object the light will focus on during LookTypes.stareAtObject")]
    private Transform currentObject;
    private bool altGazeActive = false;
    
    // Light variables
    private HDAdditionalLightData thisLightHD;
    private Light thisLight;
    
    // Focus variables ------------------------------------
    private float innerAngle; // The variable set to the other focus variables.
    private float outerAngle;
  
    [Header("Focus Angles")]
    // Standard
    [SerializeField][Tooltip("Outer angle for light cone.")][Range(0.0f, 180.0f)]
    private float standardOuterAngle = 25f;
    [SerializeField][Tooltip("Inner angle for light cone. A percentage of the outer angle.")][Range(0.0f, 100.0f)]
    private float standardInnerAngle = 40f; 
    // Focused.
    [SerializeField][Tooltip("Outer angle for light cone while focused(staring).")][Range(0.0f, 180.0f)]
    private float focusedOuterAngle = 15;
    [SerializeField][Tooltip("Inner angle for light cone while focused(staring). A percentage of the outer angle.")][Range(0.0f, 100.0f)]
    private float focusedInnerAngle = 80;
    // Unfocused
    [SerializeField][Tooltip("Outer angle for light cone while unfocused(idle).")][Range(0.0f, 180.0f)]
    private float unfocusedOuterAngle = 30;
    [SerializeField][Tooltip("Inner angle for light cone while unfocused(idle). A percentage of the outer angle.")][Range(0.0f, 100.0f)]
    private float unfocusedInnerAngle = 20; 
    // Moving
    //[SerializeField][Tooltip("Outer angle for light cone while moving.")][Range(0.0f, 180.0f)]
    private float movingOuterAngle;
    //[SerializeField][Tooltip("Inner angle for light cone while moving. A percentage of the outer angle.")][Range(0.0f, 100.0f)]
    private float movingInnerAngle; 
    
    private bool changingFocus = false;
    private float foscusTimeScalar = 0.4f; // Modifies focus time depending on turn time
    
    // Saccades
    private float varyAmount = 10f; // Vary rate

    [Header("Saccade vary amount")][Tooltip("The amount by which the light will divert from " +
                                            "its focus point in saccades. " +
                                            "The variable used depends on currentLookAt")]
    [SerializeField]
    private float varyAmountFollowGaze = 25;
    [SerializeField]
    private float varyAmountStareAtGaze = 2;
    [SerializeField]
    private float varyAmountIdle = 40;
    [SerializeField]
    private float varyAmountStareAtObject = 2;
    
    // Gaze evaluation variables
    private Vector3 currentLightTarget;
    private float lastMoveTime = 0;
    
    private Vector3 tempTarget;
    private float tempTargetTimer;
    
    // Main move - Decides how often the light can move
    [Header("Main movement")]
    [SerializeField][Tooltip("The minimum distance for registering new gaze position.")] 
    private float moveThreshold = 2f; 
    private bool lookingAtObject;
    [SerializeField][Tooltip("Baseline for how often the light can move.")]
    private float mainBaseTimer = 0.3f;   // Constant baseline for timer
    private float mainTimer; // Set to mainBaseTimer in start()
    private float minTimer = -0.2f;            // The minimum that can be added to the baseline (can be negative)
    private float maxTimer = 0.2f;             // The maximum that can be added to the baseline
    [SerializeField][Tooltip("Time the player has to focus before the light reacts.")]
    private float lightReactionTime = 0.5f; // Time the player has to focus before the light reacts
    private Quaternion targetRot;
    Vector3 pos = new Vector3(0,0,0);
    
    // Turn speed
    [Header("Turn speed")]
    [SerializeField] private float resetTurnTime = 0.2f;
    [SerializeField] private float idleTurnTime = 0.4f;
    private float turnTime;
    private bool moving = false;
    
    // Segment 2 variables --------------------------------------------------------------------------
    [Header("Segment 2")]
    [SerializeField]
    private Transform bookshelf;

    [SerializeField]
    [Tooltip("Margin for how close to the edge of the viewport the distraction will take place.")]
    [Range(0.01f, 0.99f)]
    private float distractionMarginMin = 0.1f;

    [SerializeField]
    [Tooltip("Margin for how close to the edge of the viewport the distraction will take place.")]
    [Range(0.01f, 0.99f)]
    private float distractionMarginMax = 0.9f;

    [SerializeField] [Tooltip("Event which tells distraction lights to distract.")]
    private MyDistractionEvent distractEvent;

    private bool distracting;

    [SerializeField] [Tooltip("List of objects that the light will alternatingly look at during segment 2.")]
    private List<Transform> altenateObjects;

    // Segment change focus positions
    [Header("Positions the lights will guide towards during transitioning elements.")]
    [SerializeField]
    [Tooltip("Position guiding to 1st segment.")]
    private Transform zeroToOne;
    [SerializeField]
    [Tooltip("Position for 1st to 2nd segment.")]
    private Transform oneToTwo;
    [SerializeField]
    [Tooltip("Position for 2nd to 3rd segment.")]
    private Transform twoToThree;
    [SerializeField]
    [Tooltip("Position for 3rd to end segment.")]
    private Transform threeToEnd;
    
    
    // Screen dimensions
    private float width;
    private float height;

    // -------------- Start ----------------
    void Start()
    {
        // Retrieve gameobjects / components
        thisTrans = gameObject.transform;
        thisLight = gameObject.GetComponent<Light>();
        thisLightHD = gameObject.GetComponent<HDAdditionalLightData>();
        cam = Camera.main;
        _eyetracking = conditionScript.EyeTracking;
        
        // Set initial variables
        mainTimer = mainBaseTimer;
        turnTime = resetTurnTime;
    }

    // ------------- Update -----------------
    void Update()
    {
        if (_disable)
        {
            currentLookAt = LookTypes.idle;
        }

        switch (currentLookAt) // Checks which type of gaze the light should use
        {
            case LookTypes.followGaze:
                // Look at gaze
                varyAmount = varyAmountFollowGaze;
                innerAngle = standardInnerAngle;
                outerAngle = standardOuterAngle;
                turnTime = resetTurnTime;
                pos = PlayerGaze.FindPlayerGaze();
                pos = DecideTargetGaze(pos);
                break;
            case LookTypes.stareAtGaze:
                // Stare at gaze (No / less saccades)
                varyAmount = varyAmountStareAtGaze;
                innerAngle = focusedInnerAngle;
                outerAngle = focusedOuterAngle;
                turnTime = resetTurnTime;
                pos = PlayerGaze.FindPlayerGaze();
                pos = DecideTargetGaze(pos);
                break;
            case LookTypes.idle:
                // Idle
                varyAmount = varyAmountIdle;
                innerAngle = unfocusedInnerAngle;
                outerAngle = unfocusedOuterAngle;
                turnTime = idleTurnTime;
                pos = DecideIdlePoint();
                break;
            case LookTypes.stareAtObject:
                // Look at object
                varyAmount = varyAmountStareAtObject;
                innerAngle = focusedInnerAngle;
                outerAngle = focusedOuterAngle;
                turnTime = resetTurnTime;
                pos = currentObject.position;
                break;
            case LookTypes.distractingGaze:
                // Stare at spot in player FoV furthest from the bookshelf to distract them
                if (altGazeActive) // Note: Might cause problems by causing the lights to use old information.
                {
                    break;
                }
                varyAmount = varyAmountStareAtObject;
                innerAngle = focusedInnerAngle;
                outerAngle = focusedOuterAngle;
                turnTime = resetTurnTime;
                pos = DecideDistractionPoint();
                distracting = true;
                altLookTimer = Time.time;
                altGazeActive = true;
                break;
            case LookTypes.alternateObjects:
                // Look at bookshelf and books alternatingly
                varyAmount = varyAmountStareAtObject;
                innerAngle = standardInnerAngle;
                outerAngle = standardOuterAngle;
                turnTime = resetTurnTime;
                pos = DecideObjectAlternateing(pos);
                break;
            case LookTypes.lookAtPlayer:
                // Look at the player character
                mainTimer = 0;
                varyAmount = varyAmountStareAtObject;
                innerAngle = focusedInnerAngle;
                outerAngle = focusedOuterAngle;
                turnTime = resetTurnTime;
                pos = PlayerCharacter.position;
                if (!altGazeActive)
                {
                    altLookTimer = Time.time;
                }
                altGazeActive = true;
                break;
            default:
                pos = PlayerGaze.FindPlayerGaze();
                break;
        }

        // Select target. Rotate to it. Saccade around it.
        if (Time.time - mainTimer > lastMoveTime && !moving)
        {
            CalcDoRotation(pos);
            // Resets the gaze mode/type
            if (Time.time - altLookTimer > altContinueTime && altGazeActive)
            {
                altGazeActive = false;
                altLookTimer = 0;
                currentLookAt = resetLookAt;
            }

        }
        
        var position = thisTrans.position;
        Debug.DrawRay(position, pos - position,
                         Color.yellow);
        
    }
    
    // Private Functions ------------------------------------------------------------------
    private Vector3 DecideTargetGaze(Vector3 pos) // Controls where to look
    {
        // Decide target
        if (Vector3.Distance(pos, currentLightTarget) < moveThreshold) // Is the new gaze point sufficiently far away?
        {
            return currentLightTarget;
        }
        if (Vector3.Distance(pos, tempTarget) > moveThreshold) // Are they looking at a new point?
        {
            tempTarget = pos; 
            tempTargetTimer = Time.time;
            return currentLightTarget;
        }
        if (Time.time - tempTargetTimer > lightReactionTime) // Have they looked at the same point long enough?
        {
            currentLightTarget = pos;
            return tempTarget;
        }
        
        return currentLightTarget;
    }

    private Vector3 DecideIdlePoint() // Controls the baseline direction for the idle state
    {
        Ray ray = new Ray(thisTrans.position, Vector3.down);
        RaycastHit hit;
        Physics.Raycast(ray, out hit);
        return hit.point;
    }

    private Vector3 DecideDistractionPoint() // Controls where the light looks to distract the player
    {
        height = cam.pixelHeight;
        width = cam.pixelWidth;
        GazePoint gazePoint = TobiiAPI.GetGazePoint();
        Vector2 avoidPosition = cam.WorldToScreenPoint(bookshelf.position); 
        //  might be able to check if it is in frame: device.IsInsideFrame()

        // Calculate distance to corners of game view
        float UpperLeftDist = Vector2.Distance(new Vector2(0, height), avoidPosition);
        float BottomLeftDist = Vector2.Distance(new Vector2(0, 0), avoidPosition);
        float UpperRightDist = Vector2.Distance(new Vector2(width, height), avoidPosition);
        float BottomRightDist = Vector2.Distance(new Vector2(width, 0), avoidPosition);
        
        // Select corner furthest away
        Vector2 furthestCorner;
        float maxDist = Mathf.Max(UpperLeftDist, BottomLeftDist, UpperRightDist, BottomRightDist);
        if (Math.Abs(maxDist - UpperLeftDist) < 0.1)
        {
            furthestCorner = new Vector2(0, height);
        }else if (Math.Abs(maxDist - UpperRightDist) < 0.1)
        {
            furthestCorner = new Vector2(width, height);
        }else if (Math.Abs(maxDist - BottomLeftDist) < 0.1)
        {
            furthestCorner = new Vector2(0, 0);
        }else if (Math.Abs(maxDist - BottomRightDist) < 0.1)
        {
            furthestCorner = new Vector2(width, 0);
        }
        else
        {
            furthestCorner = new Vector2(width, height);
        }
        
        // Clamp distraction point
        var x = Mathf.Clamp(furthestCorner.x, width*distractionMarginMin,width*distractionMarginMax);
        var y = Mathf.Clamp(furthestCorner.y, height*distractionMarginMin,height*distractionMarginMax);
        var pos = new Vector2(x,y);
        
        // Cast ray through camera to find point in world coordinates
        RaycastHit hit;
        Ray ray = cam.ScreenPointToRay(pos);
        Physics.Raycast(ray, out hit);

        return hit.point;
    }

    private Vector3 DecideObjectAlternateing(Vector3 posIn) // Randomly select a transform from altenateObjects and return its position
    {
        if (altenateObjects.Count < 1)
        {
            return posIn;
        }
        int rng = Random.Range(0, altenateObjects.Count);
        return altenateObjects[rng].position;
    }

    private void CalcDoRotation(Vector3 pos) // Calculates rotation and new timer
    {
        if (distracting)
        {
            distractEvent.Invoke(pos);
            distracting = false;
        }
        
        // Rotation calc
        var tempRot = thisTrans.rotation;
        thisTrans.LookAt(pos);
        targetRot = thisTrans.rotation;
        thisTrans.rotation = tempRot;
        targetRot *= Saccades();

        // Timer calc
        float timerAdd = 0;
        //if (Random.Range(0,10)>8){timerAdd = Random.Range(1f, 3f);} // hard coded variables! Make variables at top later!
        mainTimer = mainBaseTimer + Random.Range(minTimer+timerAdd, maxTimer+timerAdd);
        lastMoveTime = Time.time;

        // Turn towards our target rotation.
        moving = true;
        StartCoroutine(Rotate(thisTrans, thisTrans.rotation, targetRot));
    } // Calls Rotate()
    
    private IEnumerator Rotate(Transform rotatingObject, Quaternion from, Quaternion to) // Interpolates between quaternions
    {
        // Unfocus light
        movingInnerAngle = Mathf.Clamp(innerAngle - 40,0,100); // hardcoded :(
        movingOuterAngle = Mathf.Clamp(outerAngle + 5, 0, 180);
        StartCoroutine(ChangeFocus(movingInnerAngle, movingOuterAngle));
        
        // Rotate
        float t = 0f;
        float timeAdd = 1/turnTime;
        while (t < 1)
        {
            rotatingObject.rotation = Quaternion.Slerp(from, to, t);
            t += timeAdd * Time.deltaTime;
            yield return null;
        }
        // Focus light
        StartCoroutine(ChangeFocus(innerAngle, outerAngle));
        
        rotatingObject.rotation = to;
        moving = false;
    } // Calls ChangeFocus() twice

    private IEnumerator ChangeFocus(float innerTarget, float outerTarget) // Interpolates between light angles
    {
        float innerSource = thisLightHD.innerSpotPercent;
        float outerSource = thisLight.spotAngle;
        
        float t = 0f;
        float timeAdd = foscusTimeScalar/turnTime;
        while (t < 1)
        {
            thisLightHD.innerSpotPercent = Mathf.Lerp(innerSource, innerTarget, t);
            thisLight.spotAngle = Mathf.Lerp(outerSource, outerTarget, t);
            t += timeAdd * Time.deltaTime;
            yield return null;
        }

        changingFocus = false;
    }
    
    private Quaternion Saccades()
    {
        var xAdd = Random.Range(-varyAmount, varyAmount);
        var yAdd = Random.Range(-varyAmount, varyAmount);
        var zAdd = Random.Range(-varyAmount, varyAmount);

        return Quaternion.Euler( xAdd, yAdd, zAdd);
    }
    
    // Public Functions --------------------------------------------------------------------------
    public void SetGazeTypeCurrent(LookTypes look)
    {
        currentLookAt = look;
    }

    public void SetGazeTypeReset(LookTypes resetLook)
    {
        resetLookAt = resetLook;
    }

    public void SetCurrentObject(Transform transIn)
    {
        currentObject = transIn;
    }

    public void MoveLightNow()
    {
        mainTimer = 0;
    }

    public void AddAltObject(Transform transIn)
    {
        altenateObjects.Add(transIn);
    }

    public void RemoveAltObject(Transform transIn)
    {
        altenateObjects.Remove(transIn);
    }
    
    public void Disable()
    {
        //thisLightHD.ena;
        altGazeActive = false;
        _disable = true;
    }
    
    public void Enable()
    {
        _disable = false;
        LookTypes current = LookTypes.lookAtPlayer;
        if (!_eyetracking)
        {
            current = LookTypes.stareAtObject;
        }
        switch (_manager.GetStoryState()) // Checks what story state the game is in.
        {
            case 1:
                // Start to 1st segment
                SetCurrentObject(zeroToOne);
                SetGazeTypeCurrent(current);
                SetGazeTypeReset(LookTypes.stareAtObject);
                break;
            case 2:
                // 1st segment
                SetCurrentObject(zeroToOne);
                SetGazeTypeCurrent(current);
                SetGazeTypeReset(LookTypes.stareAtObject);
                break;
            case 3:
                // 1st segment to 2nd segment
                SetCurrentObject(oneToTwo);
                SetGazeTypeCurrent(current);
                SetGazeTypeReset(LookTypes.stareAtObject);
                break;
            case 4:
                // 2nd segment
                SetCurrentObject(oneToTwo);
                SetGazeTypeCurrent(current);
                SetGazeTypeReset(LookTypes.stareAtObject);
                break;
            case 5:
                // 2nd segment to 3rd segment
                SetCurrentObject(twoToThree);
                SetGazeTypeCurrent(current);
                SetGazeTypeReset(LookTypes.stareAtObject);
                break;
            case 6:
                // 3rd segment
                SetCurrentObject(twoToThree);
                SetGazeTypeCurrent(current);
                SetGazeTypeReset(LookTypes.stareAtObject);
                break;
            case 7:
                // 3rd segment to end
                SetCurrentObject(threeToEnd);
                SetGazeTypeCurrent(current);
                SetGazeTypeReset(LookTypes.stareAtObject);
                break;
            default:
                // If the lights shouldn't guide the player anywhere specific
                SetCurrentObject(zeroToOne);
                SetGazeTypeCurrent(current);
                SetGazeTypeReset(LookTypes.stareAtObject);
                break;
        }
    }    
}
