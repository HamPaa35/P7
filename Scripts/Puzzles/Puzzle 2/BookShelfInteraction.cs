using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BookShelfInteraction : MonoBehaviour
{
    [SerializeField] private float radius;
    [SerializeField] private Transform player;
    [SerializeField] private GameObject UICanvas;
    
    [SerializeField] string interactKey = "e";
    
    public UnityEvent bookShelfInteraction;
    
    private bool isFocus = false;
    public bool interactionEventRun = false;
    private UI_Controller UIController;
    public bool bookInHand;

    // Start is called before the first frame update
    void Start()
    {
        UIController = UICanvas.GetComponent<UI_Controller>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isFocus)
        {
            //Is player in the radius
            float distance = Vector3.Distance(player.position, transform.position);
            if (distance <= radius && bookInHand)// &&// !UIActive)
            {
                //UIActive = true;
                UIController.ActivateUiElement(UI_Controller.UI_element_names.Place);
                if (!interactionEventRun && Input.GetKey(interactKey))
                {
                    bookShelfInteraction.Invoke();
                    interactionEventRun = true;
                }
            }
            //Is The player out of focus
            else if(distance >= radius)// && UIActive)
            {
                //UIActive = false;
                UIController.DeactivateUiElement(UI_Controller.UI_element_names.Place);
            }
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
}
