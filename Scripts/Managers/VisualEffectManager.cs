using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class VisualEffectManager : MonoBehaviour
{
    [SerializeField]
    float changeDuration = 2;
    [SerializeField]
    float changeAmount = 5;

    Volume volume;
    private Fog fog;
    private bool fogAttinuationRunning; 
    // Start is called before the first frame update
    void Start()
    {
        volume = gameObject.GetComponent<Volume>();
        Fog temp;
        if (volume.profile.TryGet<Fog>(out temp))
        {
            fog = temp;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("o"))
        {
            //StartCoroutine(AnimateFogHeight(2, 50));
            AnimateFog();
        }
    }
    /*public void ModLensDistortion(bool state)
    {
        if(volume.profile.TryGet(out fog))
        {
            fog.active = state;
        }
    }*/
    
    [ContextMenu("Animate Fog")]
    public void AnimateFog()
    {
        float currentAttenuation = (float)fog.meanFreePath;
        if (!fogAttinuationRunning)
        {
            StartCoroutine(AnimateFogAttenuation(changeDuration, currentAttenuation + changeAmount));
        }
    }
    
    IEnumerator AnimateFogHeight(float duration, float targetHeight)
    {
        float percent = 0;
        float currentMaxHeight = (float)fog.maximumHeight;
        
        while (percent < 1)
        {
            percent += Time.deltaTime * 1 / duration;
            if (volume.profile.TryGet(out fog))
            {
                fog.maximumHeight.value = Mathf.Lerp(currentMaxHeight, targetHeight, percent);
            }
            yield return null;
        }
    }
    
    IEnumerator AnimateFogAttenuation(float duration, float targetTickness)
    {
        fogAttinuationRunning = true;
        float percent = 0;
        float currentAttenuation = (float)fog.meanFreePath;
        
        while (percent < 1)
        {
            percent += Time.deltaTime * 1 / duration;
            if (volume.profile.TryGet(out fog))
            {
                fog.meanFreePath.value = Mathf.Lerp(currentAttenuation, targetTickness, percent);
                yield return null;
            }
        }
        fogAttinuationRunning = false;
    }
}
