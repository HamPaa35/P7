using UnityEngine.Audio;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class AudioManager : MonoBehaviour
{
    
    public Subtitles _SubtitlesInstance;
    public Sound[] sounds;
    
    
    public static AudioManager instance;

    private List<Sound> voiceOverQueue = new List<Sound>();

    private bool QueueIsRunning = false;
    
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            foreach (Sound s in sounds)
            {
                if (s.alternativeGameObject == null)
                {
                    s.source = gameObject.AddComponent<AudioSource>();
                }
                else
                {
                    s.source = s.alternativeGameObject.AddComponent<AudioSource>();
                }
                s.source.clip = s.clip;
                
                s.source.volume = s.volume;
                s.source.pitch = s.pitch;
                s.source.loop = s.loop;
                s.source.spatialBlend = s.spatialBlend;
                s.source.minDistance = s.spatialMinDist;
                s.source.maxDistance = s.spatialMaxDist;
            }
        }
    }

    private void Start()
    {
        
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        //Debug.Log("Playing Sound: " + s.name + ".");
        s.source.Play();

    }

    public void Play(Sound s)
    {
        s.source.Play();
    }

    public void StopIfPlaying(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }

        if (s.source.isPlaying)
        {
            s.source.Stop();
        }
    }

    public void PlayVoiceOverInQueue(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + s.name + " not found!");
            return;
        }
        Debug.Log("Sound Queued: " + s.name + ".");
        voiceOverQueue.Add(s);
        if (QueueIsRunning == false)
        {
            StartCoroutine(IterateVOQueue());
            QueueIsRunning = true;
        }
    }

    public void PlayRandomFromTag(string soundTag)
    {
        Sound[] s = Array.FindAll(sounds, sound => sound.tag == soundTag);
        if (s.Length == 0)
        {
            Debug.LogWarning("Sounds with tag: " + soundTag + " not found!");
            return;
        }
        s[UnityEngine.Random.Range(0, s.Length)].source.Play();
    }

    public void FadeSoundIn(string name, float fadeInDuration = 2)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        
        StartCoroutine(AnimateSoundFade(s, fadeInDuration, "In"));
        s.source.Play();
    }

    public void FadeSoundOut(string name, float fadeOutDuration = 2)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        
        StartCoroutine(AnimateSoundFade(s, fadeOutDuration, "Out"));
    }

    IEnumerator AnimateSoundFade(Sound s, float duration, string InOut)
    {
        float percent = 0;
        
        while (percent < 1)
        {
            percent += Time.deltaTime * 1 / duration;
            if (InOut == "In")
            {
                s.source.volume = Mathf.Lerp(0, s.volume, percent);
                yield return null;

            } else if (InOut == "Out")
            {
                s.source.volume = Mathf.Lerp(s.volume, 0, percent);
                if (s.source.volume <= 0f)
                {
                    s.source.Stop();
                }
                yield return null;
            }
        }
    }

    IEnumerator IterateVOQueue()
    {
        while (voiceOverQueue.Count != 0)
        {
            voiceOverQueue[0].source.Play();
            if (voiceOverQueue[0].voSection != Subtitles.Section.None)
            {
                _SubtitlesInstance.StartSubtitles(voiceOverQueue[0].voSection);
            }
            yield return new WaitForSeconds(voiceOverQueue[0].source.clip.length + 1f);
            voiceOverQueue.RemoveAt(0);
        }

        QueueIsRunning = false;
        yield return null;
    }

    public bool CheckIfPlaying(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return false;
        }
        return s.source.isPlaying;
    }
}
