using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LightManager : MonoBehaviour
{
    
    // Light gaze
    private Transform currentLightGaze;
    [SerializeField][Tooltip("How long the light will stare at a gazeAware object.")]
    private float objectStareTimer = 8; // How long the light will stare at an object
    
    // Game state
    public enum Rooms
    {
        Kitchen = 0,
        Office = 1,
        LivingRoom = 2,
        Entrance = 3,
        HallwayMid = 4,
        HallwayEnd = 5
    }
    [SerializeField][Tooltip("Current room. Decides which lights and gazeAwareManagers are active.")]
    private Rooms gameRoom = Rooms.Entrance;
    
    // Lights
    [Header("Lights")][Tooltip("Lights for each room.")]
    [SerializeField] private LookAt puzzle_1_KitchenLight;
    [SerializeField] private LookAt puzzle_2_OfficeLight;
    [SerializeField] private LookAt puzzle_3_LivingRoomLight;
    [SerializeField] private LookAt entranceLight;
    [SerializeField] private LookAt hallway_1_MidLight;
    [SerializeField] private LookAt hallway_2_EndLight;

    private List<LookAt> allLights = new List<LookAt>();
    
    // Areas of Interest
    [Header("GazeAwareManagers")][Tooltip("Areas of interest in each room.")]
    [SerializeField] private GazeAwareManager puzzle_1_KitchenGazeAwareManager;
    [SerializeField] private GazeAwareManager puzzle_2_OfficeGazeAwareManager;
    [SerializeField] private GazeAwareManager puzzle_3_LivingRoomGazeAwareManager;
    [SerializeField] private GazeAwareManager entranceGazeAwareManager;
    [SerializeField] private GazeAwareManager hallway_1_MidGazeAwareManager;
    [SerializeField] private GazeAwareManager hallway_2_EndGazeAwareManager;

    private List<GazeAwareManager> allGazeManagers = new List<GazeAwareManager>();

    // Live variables
    private LookAt currentLight;
    private GazeAwareManager currentGazeManager;
    private LookAt.LookTypes resetType = LookAt.LookTypes.idle;

    // Start
    void Start()
    {
        currentLight = puzzle_1_KitchenLight;
        
        allLights.Add(puzzle_1_KitchenLight);
        allLights.Add(puzzle_2_OfficeLight);
        allLights.Add(puzzle_3_LivingRoomLight);
        allLights.Add(entranceLight);
        allLights.Add(hallway_1_MidLight);
        allLights.Add(hallway_2_EndLight);
        
        allGazeManagers.Add(puzzle_1_KitchenGazeAwareManager);
        allGazeManagers.Add(puzzle_2_OfficeGazeAwareManager);
        allGazeManagers.Add(puzzle_3_LivingRoomGazeAwareManager);
        allGazeManagers.Add(entranceGazeAwareManager);
        allGazeManagers.Add(hallway_1_MidGazeAwareManager);
        allGazeManagers.Add(hallway_2_EndGazeAwareManager);
        
        StartCoroutine(LateStart());
    }
    private IEnumerator LateStart()
    {
        yield return new WaitForSeconds(2);
        ChangeRoom();
    }

    // Private functions
    [ContextMenu("Change the room.")]
    private void ChangeRoom() // Changes the lights and AoI depending on the room
    {
        // Disable all lights and gazeManagers
        foreach (var iLight in allLights)
        {
            iLight.Disable();
        }
        foreach (var iGazeManager in allGazeManagers)
        {
            iGazeManager.Disable();
        }
        
        switch (gameRoom)
        {
            case Rooms.Kitchen:
                // Set local variables
                currentLight = puzzle_1_KitchenLight;
                currentGazeManager = puzzle_1_KitchenGazeAwareManager;

                // Enable current light and gazeManagers
                currentLight.Enable();
                currentGazeManager.Enable();
                break;
            case Rooms.Office:
                // Set local variables
                currentLight = puzzle_2_OfficeLight;
                currentGazeManager = puzzle_2_OfficeGazeAwareManager;

                // Enable current light and gazeManagers
                currentLight.Enable();
                currentGazeManager.Enable();
                break;
            case Rooms.LivingRoom:
                // Set local variables
                currentLight = puzzle_3_LivingRoomLight;
                currentGazeManager = puzzle_3_LivingRoomGazeAwareManager;

                // Enable current light and gazeManagers
                currentLight.Enable();
                currentGazeManager.Enable();
                break;
            case Rooms.Entrance:
                // Set local variables
                currentLight = entranceLight;
                currentGazeManager = entranceGazeAwareManager;

                // Enable current light and gazeManagers
                currentLight.Enable();
                currentGazeManager.Enable();
                break;
            case Rooms.HallwayMid:
                // Set local variables
                currentLight = hallway_1_MidLight;
                currentGazeManager = hallway_1_MidGazeAwareManager;

                // Enable current light and gazeManagers
                currentLight.Enable();
                currentGazeManager.Enable();
                break;
            case Rooms.HallwayEnd:
                // Set local variables
                currentLight = hallway_2_EndLight;
                currentGazeManager = hallway_2_EndGazeAwareManager;

                // Enable current light and gazeManagers
                currentLight.Enable();
                currentGazeManager.Enable();
                break;
        }
    }

    private IEnumerator StopStaringAtObject(LookAt lightIn) // Resets gazetype to idle
    {
        yield return new WaitForSeconds(objectStareTimer);
        lightIn.SetGazeTypeCurrent(resetType);
        lightIn.SetGazeTypeReset(resetType);
    }

    // Public functions
    public void SetCurrentLightGazeToObject(Transform transIn, bool reset)
    {
        currentLightGaze = transIn;
        currentLight.SetCurrentObject(transIn);
        currentLight.SetGazeTypeReset(LookAt.LookTypes.stareAtObject);
        currentLight.SetGazeTypeCurrent(LookAt.LookTypes.stareAtObject);
        if (reset)
        {
            StartCoroutine(StopStaringAtObject(currentLight));
        }
        
    } // Calls private coroutine StopStaringAtObject() if(reset)

    public void SetGazetypeResetAndCurrent(LookAt.LookTypes reset, LookAt.LookTypes current) // Sets the current lights LookTypes
    {
        currentLight.SetGazeTypeReset(reset);
        currentLight.SetGazeTypeCurrent(current);
    }

    public void SetCurrentObject(Transform transIn)
    {
        currentLight.SetCurrentObject(transIn);
    }
    
    public void SetRoom(Rooms newRoom) // Sets the new room
    {
        gameRoom = newRoom;
        ChangeRoom();
    }
    public void UpdateRoom() // Updates the room state
    {
        ChangeRoom();
    }

    public GameObject GetCurrentLight() // Returns gameobject of currentLight
    {
        return currentLight.gameObject;
    }
    
    // Forces the light to move immediately after the current movement instead of waiting based on the light's mainTimer.
    public void ForceInstantMove() 
    {
        currentLight.MoveLightNow();
    } // Calls MoveLightNow() in currentLight (LookAt)

    // Adds / removes an object from alternateObjects in currentLight which controls where the light will look in the
    // "LookAt.LookTypes.AlternateObjects" mode.
    public void AddAltObject_Manager(Transform transIn)
    {
        currentLight.AddAltObject(transIn);
    } // Calls AddAltObject() in currentLight (LookAt)
    public void RemoveAltObject_Manager(Transform transIn)
    {
        currentLight.RemoveAltObject(transIn);
    } // Calls RemoveAltObject() in currentLight (LookAt)
    
    public void ResetToFollowGazeAfterObject() // Sets the look type reset to after staring at object.
    {
        resetType = LookAt.LookTypes.followGaze;
    }
    public void ResetToIdleGazeAfterObject()  // Sets the look type reset to after staring at object
    {
        resetType = LookAt.LookTypes.idle;
    }
    
    
}
