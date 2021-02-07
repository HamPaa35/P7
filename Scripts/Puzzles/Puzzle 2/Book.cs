using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Book : MonoBehaviour
{
    [SerializeField] private GameObject bookInHand;
    [SerializeField] private GameObject bookOnShelf;
    [SerializeField] private GameObject throwToPoint;
    [SerializeField] private GameObject bookShelf;
    [SerializeField] private LightManager _lightManager;
    [SerializeField] private float bookThrowAnimationTime = 0.1f;
    private Vector3 height;
    private bool hasBeenPickedUp = false;
    private bool throwBookAnimationRunning;

    private BookShelfInteraction _interaction;
    private BookShelfIsLookedAt _lookedAt;
    private WinStateManager _winStateManager;
    private InteractableObject _interactableObject;

    public UnityEvent FirstBookPickedUp;
    
    private BookShelfInteraction bookShelfInFocus = null;

    private Renderer _renderer;
    private Collider _collider;
    
    // Start is called before the first frame update
    void Start()
    {
        height = new Vector3(0, gameObject.transform.lossyScale.y / 2, 0);
        _renderer = gameObject.GetComponent<Renderer>();
        _collider = gameObject.GetComponent<Collider>();
        _interaction = bookShelf.GetComponent<BookShelfInteraction>();
        _lookedAt = bookShelf.GetComponent<BookShelfIsLookedAt>();
        _winStateManager = bookShelf.GetComponent<WinStateManager>();
        _winStateManager.RegisterPuzzleObject();
        _interactableObject = gameObject.GetComponent<InteractableObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
        }

        Debug.DrawRay(transform.position, -transform.right, Color.green);
        RaycastHit hit;
        if (Physics.SphereCast(Camera.main.transform.position, 0.1f, Camera.main.transform.forward,out hit, 100))
        {
            BookShelfInteraction bookShelf = hit.collider.gameObject.GetComponent<BookShelfInteraction>();

            if (bookShelf != null && bookShelf != bookShelfInFocus  && hasBeenPickedUp) //&& mold != moldInFocus
            {
                SetFocus(bookShelf);
            }
            else if (bookShelf == null || !hasBeenPickedUp)
            {
                
                RemoveFocus();
            }
        }

        void SetFocus(BookShelfInteraction newFocus)
        {
            //if (newFocus != moldInFocus)
            //{
            if (bookShelfInFocus != null)
            {
                bookShelfInFocus.OnDefocus();
            }

            bookShelfInFocus = newFocus;
            bookShelfInFocus.OnFocus();
            //}
        }

        void RemoveFocus()
        {
            if (bookShelfInFocus != null)
            {
                bookShelfInFocus.OnDefocus();
            }

            bookShelfInFocus = null;
        }
    }

    public void onPickup()
    {
        bookInHand.SetActive(true);
        //gameObject.SetActive(false);
        _renderer.enabled = false;
        _collider.enabled = false;
        hasBeenPickedUp = true;
        _interaction.bookInHand = true;
        _interaction.interactionEventRun = false;
        _lightManager.RemoveAltObject_Manager(this.transform);
        if (!_lookedAt.FirstBookAdded)
        {
            FirstBookPickedUp.Invoke();
            Debug.Log("BOOOOOOOOOK");
        }
        //bookInHand.GetComponent<Book>().hasBeenPickedUp = true;
        _interactableObject.setHoldingObject(true);

    }

    public void putOnShelf()
    {
        if (hasBeenPickedUp)
        {
            bookOnShelf.SetActive(true);
            bookInHand.SetActive(false);
            gameObject.transform.position = bookOnShelf.transform.position;
            _lookedAt.AddBook(this);
            hasBeenPickedUp = false;
            _interaction.bookInHand = false;
            _winStateManager.CompletedPuzzleObject();
            //bookInHand.GetComponent<Book>().hasBeenPickedUp = true;
            _interactableObject.setHoldingObject(false);
        }
    }

    public void throwOfShelf()
    {
        bookOnShelf.SetActive(false);
        bookInHand.SetActive(false);
        _lookedAt.RemoveBook(this);
        //gameObject.SetActive(true);
        _renderer.enabled = true;
        _collider.enabled = true;
        if (!throwBookAnimationRunning) StartCoroutine(AnimateBookThrow(bookThrowAnimationTime));
        _lightManager.AddAltObject_Manager(this.transform);
        _winStateManager.RollBackPuzzleCompleted();
        _interactableObject.setHoldingObject(false);
    }
    
    IEnumerator AnimateBookThrow(float duration)
    {
        throwBookAnimationRunning = true;
        float percent = 0;
        
        while (percent < 1)
        {
            percent += Time.deltaTime * 1 / duration;
            gameObject.transform.position = Vector3.Lerp(bookOnShelf.transform.position, throwToPoint.transform.position, percent);
            yield return null;
        }
        throwBookAnimationRunning = false;
    }
}
