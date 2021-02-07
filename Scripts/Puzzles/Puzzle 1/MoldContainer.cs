using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MoldContainer : MonoBehaviour
{
    [SerializeField] private float moldHealth;
    private float maxMoldHealth;

    public UnityEvent MoldDead;
    public UnityEvent MoldIsSpreyed;
    public UnityEvent MoldIsSpreyesWhileLightIsLooking;
    
    private bool isFocus = false;
    private bool lightFocus = false;
    
    private Vector3 originalScale;
    private GameObject moldMesh;
    
    // Start is called before the first frame update
    void Start()
    {
        maxMoldHealth = moldHealth;
        moldMesh = gameObject.transform.GetChild(0).gameObject;
        originalScale = moldMesh.transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        if(moldHealth == 0) MoldDead.Invoke();
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

    public void Hit(int damage)
    {
        if (moldHealth > 0)
        {
            moldHealth -= damage;
            moldMesh.transform.localScale = originalScale * (moldHealth / maxMoldHealth);
        }
    }

    public void RegenHealth(int regen)
    {
        if (moldHealth > 0 && maxMoldHealth > moldHealth)
        {
            moldHealth += regen;
            moldMesh.transform.localScale = originalScale * (moldHealth / maxMoldHealth);
        }
    }
    
}
