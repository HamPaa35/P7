using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable] class UI_Element
{
    public UI_Element(UI_Controller.UI_element_names x, CanvasRenderer y)
    {
        name = x;
        element_renderer = y;
    }

    public UI_Controller.UI_element_names name;
    public CanvasRenderer element_renderer;
}

public class UI_Controller : MonoBehaviour
{
    
    [SerializeField] private CanvasRenderer interactElement;
    [SerializeField] private CanvasRenderer putDownElement;
    [SerializeField] private CanvasRenderer lightFireElement;
    [SerializeField] private CanvasRenderer pickUpElement;
    [SerializeField] private CanvasRenderer placeElement;

    private List<UI_Element> elements = new List<UI_Element>();

    public enum UI_element_names
    {
        Interact,
        PutDown,
        LightFire,
        PickUp,
        Place
    }

    private void Start() // Adds 
    {
        UI_Element interact = new UI_Element(UI_element_names.Interact, interactElement);
        UI_Element putDown = new UI_Element(UI_element_names.PutDown, putDownElement);
        UI_Element lightFire = new UI_Element(UI_element_names.LightFire, lightFireElement);
        UI_Element pickUp = new UI_Element(UI_element_names.PickUp, pickUpElement);
        UI_Element place = new UI_Element(UI_element_names.Place, placeElement);
        elements.Add(interact);
        elements.Add(putDown);
        elements.Add(lightFire);
        elements.Add(pickUp);
        elements.Add(place);
        StartCoroutine(DelayDeactivate());
    }
    
    // Private functions
    private IEnumerator DelayDeactivate()
    {
        yield return new WaitForSeconds(1);
        DeactivateAllUiElements();
    }
    private void DeactivateAllUiElements() // Deactivates all UI elements.
    {
        foreach (var iElement in elements)
        {
            iElement.element_renderer.cull = true;
        }
    }
    
    // Public functions
    public void ActivateUiElement(UI_element_names elementType) // Activates all UI elements with the input name.
    {
        foreach (var iElement in elements)
        {
            if (iElement.name == elementType)
            {
                iElement.element_renderer.cull = false;
            }
        }
    } 
    
    public void DeactivateUiElement(UI_element_names elementType) // Deactivates all UI elements with the input name.
    {
        foreach (var iElement in elements)
        {
            if (iElement.name == elementType)
            {
                iElement.element_renderer.cull = true;
            }
        }
    }
    
    
    
}