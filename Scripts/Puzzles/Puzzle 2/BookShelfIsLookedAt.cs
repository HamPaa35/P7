using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Tobii.Gaming;
using UnityEngine.Events;

[RequireComponent(typeof(GazeAware))]
[RequireComponent(typeof(BookShelfInteraction))]

public class BookShelfIsLookedAt : MonoBehaviour
{
    [SerializeField] private float lookAwayTimer;
    [SerializeField] private LightManager _lightManager;
    [SerializeField] private float minDistractionWait = 5;
    [SerializeField] private float maxDistractionWait = 10f;
    [SerializeField] private EvalCondition _evalCondition;

    private GazeAware _gazeAwareComponent;
    private BookShelfInteraction _bookShelfInteraction;

    public UnityEvent bookThrown;

    private bool lookingAtBooks;
    private bool notLookingAtBooks;
    private bool firstBookAdded = false;
    private bool stopAtOneBook = false;
    private bool win;
    private bool evalFalseRunOne = false;
    
    private float timer;
    private float randomTimer;
    private float randomWaitTime;

    public List<Book> ListOfBooks;
    
    // Start is called before the first frame update
    void Start()
    {
        _gazeAwareComponent = GetComponent<GazeAware>();
        _bookShelfInteraction = GetComponent<BookShelfInteraction>();
        randomWaitTime = Random.Range(5f, 10f);
    }

    // Update is called once per frame
    void Update()
    {
        if (_evalCondition.EyeTracking)
        {
            if (!_gazeAwareComponent.HasGazeFocus) //&& !notLookingAtBooks)
            {
                timer += Time.deltaTime;
                notLookingAtBooks = true;
                lookingAtBooks = false;
                if (timer > lookAwayTimer && !stopAtOneBook && !win)
                {
                    //Debug.Log(timer);
                    if (ListOfBooks.Count > 0)
                    {
                        ListOfBooks[ListOfBooks.Count - 1].throwOfShelf();
                        bookThrown.Invoke();
                    }
                    _bookShelfInteraction.interactionEventRun = false;
                    timer = 0;
                    stopAtOneBook = true;
                }
            }
            else if (_gazeAwareComponent.HasGazeFocus && firstBookAdded)
            {
                randomTimer += Time.deltaTime;
                if (randomTimer > randomWaitTime)
                {
                    _lightManager.SetGazetypeResetAndCurrent(LookAt.LookTypes.alternateObjects,
                        LookAt.LookTypes.distractingGaze);
                    _lightManager.ForceInstantMove();
                    randomTimer = 0;
                    randomWaitTime = Random.Range(minDistractionWait, maxDistractionWait);
                }

                notLookingAtBooks = false;
                lookingAtBooks = true;
                timer = 0;
            }
        }
        else if (!_evalCondition.EyeTracking && !evalFalseRunOne)
        {
            //DOES IT NEED TO DISTRACT?
        }
    }

    public void AddBook(Book book)
    {
        if (!firstBookAdded) firstBookAdded = true;
        stopAtOneBook = false;
        ListOfBooks.Add(book);
    }

    public void RemoveBook(Book book)
    {
        ListOfBooks.Remove(book);
    }

    public bool Win
    {
        get => win;
        set => win = value;
    }

    public bool StopAtOneBook => stopAtOneBook;
    public bool FirstBookAdded => firstBookAdded;
}