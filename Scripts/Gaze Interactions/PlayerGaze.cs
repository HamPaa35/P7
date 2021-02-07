using System.Collections;
using System.Collections.Generic;
using Tobii.Gaming;
using UnityEngine;

public class PlayerGaze : MonoBehaviour
{
    private static Camera cam = Camera.main;
    public static Vector3 FindPlayerGaze() // Finds the gazepoint of the player (Consider making a static function)
    {
        Ray ray;
        
        // Get gazepoint from tobii and create ray
        GazePoint gazePoint = TobiiAPI.GetGazePoint();
        if (gazePoint.IsValid && gazePoint.IsRecent())
        {
            Vector3 gazePosition = new Vector3(gazePoint.Screen.x, gazePoint.Screen.y, 0);
            ray = cam.ScreenPointToRay(gazePosition);
        }else // Use mouse position if gaze position is unavailable
        {
            ray = cam.ScreenPointToRay(Input.mousePosition);
        }
             
        // Find first intersection from light in direction of mouse in world space
        RaycastHit hit;
        Physics.SphereCast(ray.origin, 0.2f, ray.direction, out hit);
        return hit.point;
    }
}
