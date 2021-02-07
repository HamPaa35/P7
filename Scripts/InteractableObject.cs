using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractableObject : MonoBehaviour
{
    /// <summary>
    /// This scripts handles the interactable object in the game, this script should be attached to object that are
    /// interactable. The object is interactable when the player is within the radius, and looking at the object.
    /// </summary>
    
    
    [SerializeField] private float radius;
    [SerializeField] string interactKey = "e";
    [SerializeField] string putDownKey = "q";
    [SerializeField] private GameObject UICanvas;
    [SerializeField] private bool quitInteraction = false;
    [SerializeField] private CharacterInteraction _characterInteraction;

    Transform player;
    
    bool isFocus = false;
    bool hasInteracted = false;
    bool UIActive = false;
    bool inHand = false;
    private bool UIEventCalled = false;
    private UI_Controller UIController;
    [SerializeField] private CastToHighlight _highlighter;

    public UnityEvent onInteraction;
    public UnityEvent onQuitInteraction;
    public UnityEvent activeUI;
    
    // Start is called before the first frame update
    void Start()
    {
        UIController = UICanvas.GetComponent<UI_Controller>();
        _highlighter = CastToHighlight.instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_characterInteraction.HoldingObject)
        {
            if (isFocus)
            {
                //Is player in the radius
                float distance = Vector3.Distance(player.position, transform.position);
                if (distance <= radius && !UIActive)
                {
                    UIActive = true;
                    //UIController.ActivateUiElement(UI_Controller.UI_element_names.Interact);
                }
                //Is The player out of focus
                else if (distance >= radius && UIActive)
                {
                    UIActive = false;
                    //UIController.DeactivateUiElement(UI_Controller.UI_element_names.Interact);
                }

                //player pressed for interaction
                if (Input.GetKey(interactKey) && UIActive && !hasInteracted)
                {
                    hasInteracted = true;
                    inHand = true;
                    onInteraction.Invoke();
                    UIActive = false;
                    _characterInteraction.HoldingObject = true;
                    UIController.DeactivateUiElement(UI_Controller.UI_element_names.Interact);
                    if (quitInteraction) UIController.ActivateUiElement(UI_Controller.UI_element_names.PutDown);
                }
            }
            
        }
        //Player pressed for quit interaction
        if (inHand && Input.GetKey(putDownKey))
        {
            PutDown();
        }

        if (UIActive && !UIEventCalled)
        {
            activeUI.Invoke();
            UIEventCalled = true;
            UIController.ActivateUiElement(UI_Controller.UI_element_names.Interact);
        }
        else if (!UIActive && UIEventCalled)
        {
            UIEventCalled = false;
            UIController.DeactivateUiElement(UI_Controller.UI_element_names.Interact);
        }
    }
    // Put down
    public void PutDown()
    {
        onQuitInteraction.Invoke();
        _characterInteraction.HoldingObject = false;
        inHand = false;
        UIController.DeactivateUiElement(UI_Controller.UI_element_names.PutDown);
    }
    
    
    //When looked at
    public void OnFocus(Transform playerTransform)
    {
        isFocus = true;
        //hasInteracted = false;
        player = playerTransform;
        _highlighter.HighlightObject(this.gameObject);
    }
    
    //When unlooked at
    public void OnDefocus()
    {
        isFocus = false;
        hasInteracted = false;
        UIActive = false;
        //UIController.DeactivateUiElement(UI_Controller.UI_element_names.Interact);
        player = null;
        _highlighter.DeHighlightObject();
    }
    
    //Just for debug
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    public void setHoldingObject(bool holdingobject)
    {
        _characterInteraction.HoldingObject = holdingobject;
    }

    public bool isUIActive()
    {
        return UIActive;
    }
}
