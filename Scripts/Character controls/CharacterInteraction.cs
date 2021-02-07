using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInteraction : MonoBehaviour
{
    /// <summary>
    /// This needs to be on the character for it to be able to interact with objects.
    /// </summary>
    
    public InteractableObject focus = null;

    private bool holdingObject;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        
        if(Physics.SphereCast(Camera.main.transform.position, 0.1f, Camera.main.transform.forward,out hit, 100))
        {
            Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward, Color.green);

            //Null if no object is hit
            InteractableObject interactableObject = hit.collider.gameObject.GetComponent<InteractableObject>();
            
            //Checks if the current focus object is interactable
            if (interactableObject != null && interactableObject != focus)
            {
                SetFocus(interactableObject);
            }
            else if(interactableObject == null && focus != null)
            {
                //Debug.Log("remove focus");
                RemoveFocus();
            }
        }
    }

    void SetFocus(InteractableObject newFocus)
    {
        //This does not need to run if the object is the same
        if (newFocus != focus)
        {
            if (focus != null)
            {
                //Debug.Log("focus is not null");
                focus.OnDefocus();
            }

            focus = newFocus; 
            focus.OnFocus(this.transform);
        }
    }

    void RemoveFocus()
    {
        if(focus != null)
            focus.OnDefocus();
        focus = null;
    }

    public bool HoldingObject
    {
        get => holdingObject;
        set => holdingObject = value;
    }
}
