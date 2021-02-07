using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Mold : MonoBehaviour
{
    //[SerializeField] private MoldContainer _moldContainer;
    
    private bool isFocus = false;
    private bool lightFocus = false;

    public UnityEvent MoldIsSpreyed;
    public UnityEvent MoldIsSpreyesWhileLightIsLooking;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnFocus()
    {
        isFocus = true;
        if(!lightFocus) MoldIsSpreyed.Invoke();
        if(lightFocus) MoldIsSpreyesWhileLightIsLooking.Invoke();
        
    }

    public void OnDefocus()
    {
        isFocus = false;
    }

    public void SetLightFocus(bool newLightFocusState)
    {
        //Debug.Log(newLightFocusState);
        lightFocus = newLightFocusState;
    }
}
