using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LightLookingAtMold : MonoBehaviour
{
    public UnityEvent _LightLookingAtMold;
    public UnityEvent LightStoppedLookingAtMold;
    
    [SerializeField] private float regenTimer;
    [SerializeField] private EvalCondition _evalCondition;
    [SerializeField] private AudioManager _audioManager;
    
    private Light light;
    private LookAt _lookAt;
    public MoldContainer currentMoldFocus = null;
    private float timer;
    private bool runOnce;

    public float cameraShakeMagnitude;

    [SerializeField] private CameraShake _cameraShake;
    // Start is called before the first frame update
    void Start()
    {
        light = gameObject.GetComponent<Light>();
        _lookAt = gameObject.GetComponent<LookAt>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        
        _lookAt.SetGazeTypeReset(LookAt.LookTypes.followGaze);
        
        Debug.DrawRay(transform.position, transform.forward, Color.green);
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 100))
        {
            MoldContainer mold = hit.collider.gameObject.GetComponent<MoldContainer>();

            if (mold != null)
            {
                if (!runOnce)
                {
                    light.color = Color.red;
                    _LightLookingAtMold.Invoke();
                    _audioManager.FadeSoundIn("HouseCreak", 1);
                    StartCoroutine(_cameraShake.StartShake(cameraShakeMagnitude));
                    runOnce = true;
                }
                currentMoldFocus = mold;
                currentMoldFocus.SetLightFocus(true);
                if (timer > regenTimer)
                {
                    mold.RegenHealth(20);
                    timer = 0;
                }
            }
            else if (mold == null && currentMoldFocus != null)
            {
                if (runOnce)
                {
                    LightStoppedLookingAtMold.Invoke();
                    _audioManager.FadeSoundOut("HouseCreak", 2);
                    StartCoroutine(_cameraShake.StopShake(cameraShakeMagnitude));
                    runOnce = false;
                }
                currentMoldFocus.SetLightFocus(false);
                light.color = Color.white;
                timer = 0;
            }
        }
    }
}
