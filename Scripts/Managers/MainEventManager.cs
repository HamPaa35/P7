using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MainEventManager : MonoBehaviour
{
    [SerializeField] private string storyForward = "p";
    [SerializeField] private string storyBack = "o";
    [SerializeField] private bool debugMode = false;

    //Events
    public UnityEvent _setup;
    public UnityEvent _intro;
    public UnityEvent _firstSegment;
    public UnityEvent _firstToSecond;
    public UnityEvent _secondSegment;
    public UnityEvent _secondToThird;
    public UnityEvent _thirdSegment;
    public UnityEvent _ThirdToEnd;
    public UnityEvent _theEnd;

    public UnityEvent changeRoom;
    
    //Has been run
    private bool _setupHasBeenRun;
    private bool _introHasBeenRun;
    private bool _firstSegmentHasBeenRun;
    private bool _firstToSecondHasBeenRun;
    private bool _secondSegmentHasBeenRun;
    private bool _secondToThirdHasBeenRun;
    private bool _thirdSegmentHasBeenRun;
    private bool _thirdToEndHasBeenRun;
    private bool _theEndHasBeenRun;
    
    //Variables
    private int storyState = 0;

    // Start is called before the first frame update
    void Start()
    {
        EventState();
    }

    // Update is called once per frame
    void Update()
    {
        if (debugMode)
        {
            if (Input.GetKeyDown(storyForward))
            {
                ProgressStory();
            }

            if (Input.GetKeyDown(storyBack))
            {
                ProgressStoryBack();
            }
        }
        
        else if (!debugMode && storyState == 0)
        {
            if (Input.GetKeyDown(storyForward))
            {
                ProgressStory();
            }
        }
    }

    [ContextMenu("Story Forward")]
    public void ProgressStory()
    {
        storyState++;
        EventState();
        if (storyState>1)
        {
            changeRoom.Invoke();
        }
    }
    [ContextMenu("Story Back")]
    public void ProgressStoryBack()
    {
        if(storyState > 0)storyState--;
        EventState();
    }

    public void EventState()
    {
        switch (storyState)
        {
            case 0:
                if (!_setupHasBeenRun)
                {
                    _setup.Invoke();
                    _setupHasBeenRun = true;
                    Debug.Log("Story State: setup");
                }
                break;
            case 1:
                if (!_introHasBeenRun)
                {
                    _intro.Invoke();
                    _introHasBeenRun = true;
                    Debug.Log("Story State: Intro");
                }
                break;
            case 2:
                if (!_firstSegmentHasBeenRun)
                {
                    _firstSegment.Invoke();
                    _firstSegmentHasBeenRun = true;
                    Debug.Log("Story State: firstSegment");
                }
                break;
            case 3:
                if (!_firstToSecondHasBeenRun)
                {
                    _firstToSecond.Invoke();
                    _firstToSecondHasBeenRun = true;
                    Debug.Log("Story State: firstToSecond");
                }
                break;
            case 4:
                if (!_secondSegmentHasBeenRun)
                {
                    _secondSegment.Invoke();
                    _secondSegmentHasBeenRun = true;
                    Debug.Log("Story State: secondSegment");
                }
                break;
            case 5:
                if (!_secondToThirdHasBeenRun)
                {
                    _secondToThird.Invoke();
                    _secondToThirdHasBeenRun = true;
                    Debug.Log("Story State: secondToThird");
                }
                break;
            case 6:
                if (!_thirdSegmentHasBeenRun)
                {
                    _thirdSegment.Invoke();
                    _thirdSegmentHasBeenRun = true;
                    Debug.Log("Story State: thirdSegment");
                }
                break;
            case 7:
                if (!_thirdToEndHasBeenRun)
                {
                    _ThirdToEnd.Invoke();
                    _thirdToEndHasBeenRun = true;
                    Debug.Log("Story State: thirdToEnd");
                }
                break;
            case 8:
                if (!_theEndHasBeenRun)
                {
                    _theEnd.Invoke();
                    _theEndHasBeenRun = true;
                    Debug.Log("Story State: theEnd");
                }
                break;
            default:
                Debug.Log("Story State: Something went wrong...");
                break;
        }
    }

    public int GetStoryState()
    {
        return storyState;
    }
}
