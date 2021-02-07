//-----------------------------------------------------------------------
// Copyright 2016 Tobii AB (publ). All rights reserved.
//-----------------------------------------------------------------------

using UnityEngine;
using Tobii.Gaming;

/// <summary>
/// Changes the color of the game object's material, when the the game object 
/// is in focus of the user's eye-gaze.
/// </summary>
/// <remarks>
/// Referenced by the Target game objects in the Simple Gaze Selection example scene.
/// </remarks>
[RequireComponent(typeof(GazeAware))]
public class IsLookedOn : MonoBehaviour
{

	private GazeAware _gazeAwareComponent;

	/// <summary>
	/// Set the lerp color
	/// </summary>
	void Start()
	{
		_gazeAwareComponent = GetComponent<GazeAware>();
	}

	/// <summary>
	/// Lerping the color
	/// </summary>
	void Update()
	{
		// Change the color of the cube
		if (_gazeAwareComponent.HasGazeFocus)
		{
			Debug.Log("kig");
		}

	}
}
