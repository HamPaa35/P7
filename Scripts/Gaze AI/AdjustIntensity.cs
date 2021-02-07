using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class AdjustIntensity : MonoBehaviour
{
    private HDAdditionalLightData thisLightHD;

    private float resetIntensity;
    // Start is called before the first frame update
    void Start()
    {
        thisLightHD = gameObject.GetComponent<HDAdditionalLightData>();
        resetIntensity = thisLightHD.intensity;
    }

    public void SetLightIntensity(float intensityIn)
    {
        thisLightHD.intensity = intensityIn;

    }

    [ContextMenu("Reset")]
    public void ResetLightIntensity()
    {
        thisLightHD.intensity = resetIntensity;
    }
}
