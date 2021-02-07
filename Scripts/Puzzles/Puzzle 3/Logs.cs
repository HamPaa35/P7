using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Logs : MonoBehaviour
{
    [SerializeField] private GameObject logInHand;
    [SerializeField] private GameObject logInFire;
    [SerializeField] private GameObject firePlace;
    [SerializeField] private LightManager _lightManager;
    [SerializeField] private LookAtlight _lookAtlight;
    private bool hasBeenPickedUp = false;
    private bool hasLookedAtRun = false;

    private FirePlaceInteraction _firePlaceInteraction;
    private ManageLogs _manageLogs;
    private WinStateManager _winStateManager;
    private InteractableObject _interactableObject;

    private FirePlaceInteraction firePlaceInFocus = null;

    private Renderer _renderer;
    private Collider _collider;

    private Camera cam;
    
    // Start is called before the first frame update
    void Start()
    {
        _renderer = gameObject.GetComponent<Renderer>();
        _collider = gameObject.GetComponent<Collider>();
        _firePlaceInteraction = firePlace.GetComponent<FirePlaceInteraction>();
        _manageLogs = firePlace.GetComponent<ManageLogs>();
        _winStateManager = firePlace.GetComponent<WinStateManager>();
        _winStateManager.RegisterPuzzleObject();
        _firePlaceInteraction.RegisterLog();
        _interactableObject = gameObject.GetComponent<InteractableObject>();
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
        }

        Debug.DrawRay(transform.position, -transform.right, Color.green);
        RaycastHit hit;
        if (Physics.SphereCast(cam.transform.position, 0.1f, cam.transform.forward,out hit, 100))
        {
            FirePlaceInteraction log = hit.collider.gameObject.GetComponent<FirePlaceInteraction>();

            if (log != null && log != firePlaceInFocus  && hasBeenPickedUp) //&& mold != moldInFocus
            {
                SetFocus(log);
            }
            else if (log == null || !hasBeenPickedUp)
            {
                
                RemoveFocus();
            }
        }

        void SetFocus(FirePlaceInteraction newFocus)
        {
            //if (newFocus != moldInFocus)
            //{
            if (firePlaceInFocus != null)
            {
                firePlaceInFocus.OnDefocus();
            }

            firePlaceInFocus = newFocus;
            firePlaceInFocus.OnFocus();
            //}
        }

        void RemoveFocus()
        {
            if (firePlaceInFocus != null)
            {
                firePlaceInFocus.OnDefocus();
            }

            firePlaceInFocus = null;
        }

        if (_lookAtlight.LookedAt && !hasLookedAtRun && hasBeenPickedUp)
        {
            hasLookedAtLight();
            _firePlaceInteraction.LookedAtLight = true;
            hasLookedAtRun = true;
        }
    }

    public void onPickup()
    {
        logInHand.SetActive(true);
        //gameObject.SetActive(false);
        _renderer.enabled = false;
        _collider.enabled = false;
        hasBeenPickedUp = true;
        _firePlaceInteraction.logInHand = true;
        _firePlaceInteraction.interactionEventRun = false;
        _manageLogs.RemoveWoodenLog(this.transform);
        _interactableObject.setHoldingObject(true);
        _firePlaceInteraction.LookedAtLight = false;
        _manageLogs.StopRepeating();
        _lightManager.SetGazetypeResetAndCurrent(LookAt.LookTypes.lookAtPlayer,LookAt.LookTypes.lookAtPlayer);
        _lookAtlight.Blink = true;
        //bookInHand.GetComponent<Book>().hasBeenPickedUp = true;

        //THIS SHOULD WAIT FOR GAZE ON THE LIGHT
        //_manageLogs.StopRepeating();
        //_lightManager.SetCurrentLightGazeToObject(firePlace.transform, false);

    }

    public void putInFireplace()
    {
        if (hasBeenPickedUp)
        {
            logInFire.SetActive(true);
            logInHand.SetActive(false);
            hasBeenPickedUp = false;
            _firePlaceInteraction.logInHand = false;
            _manageLogs.LookBetweenLogAndFireplace();
            _winStateManager.CompletedPuzzleObject();
            _firePlaceInteraction.CompletedLog();
            _interactableObject.setHoldingObject(false);
        }
    }

    public void throwOfShelf()
    {
        logInFire.SetActive(false);
        logInHand.SetActive(false);
        //gameObject.SetActive(true);
        _renderer.enabled = true;
        _collider.enabled = true;
        _manageLogs.AddAltWoodenLog(this.transform);
        _interactableObject.setHoldingObject(false);
    }

    public void hasLookedAtLight()
    {
        _manageLogs.StopRepeating();
        _lightManager.SetCurrentLightGazeToObject(firePlace.transform, false);
    }
}
