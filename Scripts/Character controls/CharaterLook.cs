using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharaterLook : MonoBehaviour
{
    [SerializeField] Transform cam;
    [SerializeField] float sensitivity;
    [SerializeField] float headRotationLimit = 90f;
    float headRotation = 0f;

    public Transform playerBody;
    
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float x = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        float y = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime * -1f;
        headRotation += y;
        headRotation = Mathf.Clamp(headRotation, -headRotationLimit, headRotationLimit);
        cam.localEulerAngles = new Vector3(headRotation, 0f, 0f);
        
        playerBody.Rotate(Vector3.up * x);
    }
}
