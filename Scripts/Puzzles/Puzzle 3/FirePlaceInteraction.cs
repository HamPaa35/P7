using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FirePlaceInteraction : MonoBehaviour
{
    [SerializeField] private float radius;
    [SerializeField] private Transform player;
    [SerializeField] private GameObject UICanvas;
    
    [SerializeField] string interactKey = "e";
    [SerializeField] private string igniteKey = "f";
    [SerializeField] private AudioManager _audioManager;
    
    public UnityEvent firePlaceInteraction;
    public UnityEvent igniteFireEvent;
    
    private bool isFocus = false;
    public bool interactionEventRun = false;
    private bool igniteEventRun;
    private bool quitUI = false;
    private UI_Controller UIController;
    public bool logInHand;
    private WinStateManager _winStateManager;
    private bool lookedAtLight = false;

    private int numberOfLogs;
    private int numberOfLogsInFire;
    private bool allLogs;

    // Start is called before the first frame update
    void Start()
    {
        UIController = UICanvas.GetComponent<UI_Controller>();
        _winStateManager = gameObject.GetComponent<WinStateManager>();
        _winStateManager.RegisterPuzzleObject();
    }

    // Update is called once per frame
    void Update()
    {
        if (isFocus && lookedAtLight)
        {
            //Is player in the radius
            float distance = Vector3.Distance(player.position, transform.position);
            if (distance <= radius && logInHand)// &&// !UIActive)
            {
                //UIActive = true;
                UIController.ActivateUiElement(UI_Controller.UI_element_names.Place);
                if (!interactionEventRun && Input.GetKey(interactKey))
                {
                    firePlaceInteraction.Invoke();
                    interactionEventRun = true;
                }
            }
            //Is The player out of focus
            else if(distance >= radius && !interactionEventRun)// && UIActive)
            {
                //UIActive = false;
                UIController.DeactivateUiElement(UI_Controller.UI_element_names.Place);
            }
        }
        float distance2 = Vector3.Distance(player.position, transform.position);
        if (distance2 <= radius && allLogs && !igniteEventRun && interactionEventRun)
        {
            UIController.ActivateUiElement(UI_Controller.UI_element_names.LightFire);
            quitUI = false; 
            if (Input.GetKey(igniteKey))
            {
                igniteFireEvent.Invoke();
                _audioManager.FadeSoundIn("Fireplace");
                _winStateManager.CompletedPuzzleObject();
                igniteEventRun = true;
                UIController.DeactivateUiElement(UI_Controller.UI_element_names.LightFire);
            }
        }
        else if (allLogs && distance2 >= radius && !igniteEventRun && !quitUI)
        {
            UIController.DeactivateUiElement(UI_Controller.UI_element_names.LightFire);
            quitUI = true;
        }
        
        if (numberOfLogs <= numberOfLogsInFire)
        {
            allLogs = true;
        }
        
        
    }

    public void OnFocus()
    {
        isFocus = true;
        /*if (!interactionEventRun && Input.GetKey(interactKey))
        {
            bookShelfInteraction.Invoke();
            interactionEventRun = true;
        }*/
    }

    public void OnDefocus()
    {
        isFocus = false;
        UIController.DeactivateUiElement(UI_Controller.UI_element_names.Place);
    }

    public void SetLightFocus(bool newLightFocusState)
    {
        //Debug.Log(newLightFocusState);
        interactionEventRun = newLightFocusState;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    public bool LookedAtLight
    {
        get => lookedAtLight;
        set => lookedAtLight = value;
    }
    
    public void RegisterLog()
    {
        numberOfLogs++;
    }

    public void CompletedLog()
    {
        numberOfLogsInFire++;
    }
}
