using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MoldManager : MonoBehaviour
{
    public UnityEvent AllMoldDead;

    private int numberOfChildren;
    private int numberOfDeadChildren;
    
    // Start is called before the first frame update
    void Start()
    {
        numberOfChildren = transform.childCount;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AllMoldDeadCheck()
    {
        numberOfDeadChildren++;
        
        /*foreach (Transform child in transform)
        {
            if (!child.gameObject.activeSelf)
            {
                numberOfDeadChildren += 1;
            }
        }*/

        if (numberOfChildren == numberOfDeadChildren)
        {
            AllMoldDead.Invoke();
            Debug.Log("ALL MOLD IS DEAD");
        }
    }
}
