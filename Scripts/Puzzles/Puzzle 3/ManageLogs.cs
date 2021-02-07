using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageLogs : MonoBehaviour
{
    [SerializeField][Tooltip("The logs that the player should return to the fireplace.")] 
    private List<Transform> woodenLogs;

    [SerializeField] [Tooltip("The LightManager controlling the lights.")]
    private LightManager _lightManager;

    [Header("GazeTimer")]
    [Tooltip("The time for which the light looks at the player, log, and fireplace when it guides the player.")]
    [SerializeField] private float timePlayer = 4f;
    [SerializeField] private float timeLog = 2f;
    [SerializeField] private float timeFire = 2f;

    private bool continueRepeating = false;
    
    
    // Private functions --------------------------------
    private IEnumerator LookBetween_Coroutine() // Makes the light look at the player, a log, and then the fireplace
    {
        if (woodenLogs.Count < 1)
        {
            yield break;
        }
        
        int rng = Random.Range(0, woodenLogs.Count-1);
        continueRepeating = true;
        while (continueRepeating)
        {
            _lightManager.SetGazetypeResetAndCurrent(LookAt.LookTypes.lookAtPlayer,LookAt.LookTypes.lookAtPlayer);
            _lightManager.ForceInstantMove();
            yield return new WaitForSeconds(timePlayer);// Consider swapping this timer with a reaction to the player's gaze.
            if (woodenLogs.Count < 1 || !continueRepeating)
            {
                yield break;
            }
            
            _lightManager.SetCurrentLightGazeToObject(woodenLogs[rng], false);
            _lightManager.ForceInstantMove();
            yield return new WaitForSeconds(timeLog);
            if (!continueRepeating)
            {
                break;
            }
            
            _lightManager.SetCurrentLightGazeToObject(this.transform, false);
            _lightManager.ForceInstantMove();
            yield return new WaitForSeconds(timeFire);
        }
        // _lightManager.SetGazetypeResetAndCurrent(LookAt.LookTypes.stareAtObject, LookAt.LookTypes.stareAtObject);
        // _lightManager.ForceInstantMove();
    }

    // Public functions ---------------------------
    public void LookBetweenLogAndFireplace()
    {
        StartCoroutine(LookBetween_Coroutine());
    } // Calls LookBetween_Coroutine()

    public void StopRepeating() // Stops loop in LookBetween_Coroutine()
    {
        continueRepeating = false;
    }
    
    public void AddAltWoodenLog(Transform transIn)
    {
        woodenLogs.Add(transIn);
    }

    public void RemoveWoodenLog(Transform transIn)
    {
        woodenLogs.Remove(transIn);
    }
}
