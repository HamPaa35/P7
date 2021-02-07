using System.Collections;
using System.Collections.Generic;
using System.Security.AccessControl;
using UnityEngine;
using UnityEngine.Events;

public class WinStateManager : MonoBehaviour
{
    private int numberOfPuzzleObjects;
    private int numberOfCompletedPuzzleObjects;
    private bool puzzleCompleted = false;
    private bool winEventRun = false;

    public UnityEvent winEvent;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (numberOfPuzzleObjects <= numberOfCompletedPuzzleObjects && !winEventRun)
        {
            Debug.Log("WIN STATE");
            winEvent.Invoke();
            winEventRun = true;
            puzzleCompleted = true;
        }
    }
    
    public void RegisterPuzzleObject()
    {
        numberOfPuzzleObjects++;
    }

    public void CompletedPuzzleObject()
    {
        numberOfCompletedPuzzleObjects++;
    }

    public void RollBackPuzzleCompleted()
    {
        numberOfCompletedPuzzleObjects--;
    }

    public bool PuzzleCompleted => puzzleCompleted;
}
