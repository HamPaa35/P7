using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class SprayBottle : MonoBehaviour
{
    private MoldContainer moldInFocus = null;
    [SerializeField]private VisualEffect VisualEffect;

    public AudioManager audioInstance;
    
    private void Awake()
    {
        audioInstance = GameObject.FindObjectOfType<AudioManager>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            VisualEffect.Play();
            audioInstance.PlayRandomFromTag("spray");
        }

        Debug.DrawRay(transform.position, transform.up, Color.green);
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.up, out hit, 100))
        {
            MoldContainer mold = hit.collider.gameObject.GetComponent<MoldContainer>();

            if (mold != null && Input.GetMouseButtonDown(0)) //&& mold != moldInFocus
            {
                SetFocus(mold);
            }
            else if (mold == null)
            {
                RemoveFocus();
            }
        }

        void SetFocus(MoldContainer newFocus)
        {
            //if (newFocus != moldInFocus)
            //{
                if (moldInFocus != null)
                {
                    moldInFocus.OnDefocus();
                }

                moldInFocus = newFocus;
                moldInFocus.OnFocus();
            //}
        }

        void RemoveFocus()
        {
            if (moldInFocus != null)
                moldInFocus.OnDefocus();
            moldInFocus = null;
        }
    }
}