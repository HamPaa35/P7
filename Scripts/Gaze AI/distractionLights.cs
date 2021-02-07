using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class MyDistractionEvent : UnityEvent<Vector3>
{
}

public class distractionLights : MonoBehaviour
{
    [SerializeField] [Tooltip("List of lights used for distraction")]
    private Transform[] distractionLightList;

    [SerializeField][Tooltip("Turn time for distractionlights")]
    private float turnTimeDistract = 0.2f;

    [SerializeField] [Tooltip("Time before the lights will reset after distracting the player.")]
    private float resetTimer = 2f;
    
    private Quaternion targetRot;

    public void RotateDistractionLights(Vector3 pos) // Calculates rotation
    {
        // Rotation calc
        
        
        foreach (var light in distractionLightList)
        {
            // Rotation calc
             var tempRot = light.rotation;
             light.LookAt(pos);
             targetRot = light.rotation;
             light.rotation = tempRot;
            StartCoroutine(RotateSingleLight(light, light.rotation, targetRot, true));
        }
        
    } // Calls RotateSingleLight()

    private IEnumerator RotateSingleLight(Transform light, Quaternion from, Quaternion to, bool shouldContinue)
    {
        // Rotate
        float t = 0f;
        float timeAdd = 1/turnTimeDistract;
        while (t < 1)
        {
            light.rotation = Quaternion.Slerp(from, to, t);
            t += timeAdd * Time.deltaTime;
            yield return null;
        }

        light.rotation = to;
        if (shouldContinue)
        {
            yield return new WaitForSeconds(resetTimer);
            StartCoroutine(RotateSingleLight(light, to, from, false));
        }
        
    }

}
