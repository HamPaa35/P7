using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillZone : MonoBehaviour
{

	private Vector3 startPosition;
	CharacterController cc;

	void Awake(){
		startPosition = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
	}
    // Start is called before the first frame update
    void Start()
    {
	    cc = gameObject.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.y <= -10){
		gameObject.transform.position = startPosition;
		cc.enabled = false;
 		gameObject.transform.position = gameObject.transform.position;
 		cc.enabled = true;
		
		}
    }
}
