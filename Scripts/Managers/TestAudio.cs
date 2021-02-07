using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAudio : MonoBehaviour
{

    public AudioManager instance; 
    
    // Start is called before the first frame update
    void Start()
    {
        instance.FadeSoundIn("Ambience", 1);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            instance.PlayRandomFromTag("spray");
        } else if (Input.GetKeyDown(KeyCode.X))
        {
            if (!instance.CheckIfPlaying("Fireplace"))
            {
                instance.FadeSoundIn("Fireplace", 3);
            }
        } else if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            instance.PlayVoiceOverInQueue("Intro");
        } else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            instance.PlayVoiceOverInQueue("Puzzle1Intro");
        } else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            instance.PlayVoiceOverInQueue("Puzzle1Hint");
        } else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            instance.PlayVoiceOverInQueue("Puzzle2Intro");
        } else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            instance.PlayVoiceOverInQueue("Puzzle2Hint");
        } else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            instance.PlayVoiceOverInQueue("Puzzle3Intro");
        } else if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            instance.PlayVoiceOverInQueue("Outro");
        }
    }
}
