using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CameraShake : MonoBehaviour
{
    private Vector3 _originalPos;
    private bool _isShaking = false;
    private bool shakingIn = false;
    private float slideMagnitude;
    private float slideMagnitudeAtOut;
    
    public float slideInDuration = 3f;
    public float slideOutDuration = 2f;
    
    public IEnumerator ShakeWithDuration(float duration, float magnitude)
    {
        _originalPos = transform.localPosition;
        
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;
            float z = Random.Range(-1f, 1f) * magnitude;
            
            transform.localPosition = new Vector3(_originalPos.x + x, _originalPos.y + y, _originalPos.z + z);

            elapsed += Time.deltaTime;

            yield return null;
        }
        transform.localPosition = _originalPos;
    }

    public IEnumerator StartShake(float magnitude)
    {
        if (!_isShaking)
        {
            _originalPos = transform.localPosition;
            _isShaking = true;
        }

        StartCoroutine(SlidingMagnitude(slideInDuration, magnitude, "In"));
        
        while (_isShaking)
        {
            float x = Random.Range(-1f, 1f) * slideMagnitude;
            float y = Random.Range(-1f, 1f) * slideMagnitude;
            float z = Random.Range(-1f, 1f) * slideMagnitude;
            
            transform.localPosition = new Vector3(_originalPos.x + x, _originalPos.y + y, _originalPos.z + z);

            yield return null;
        }
    }

    public IEnumerator StopShake(float magnitude)
    {
        StartCoroutine(SlidingMagnitude(slideOutDuration, magnitude, "Out"));

        yield return null;
    }

    public IEnumerator SlidingMagnitude(float slideDuration, float maxMagnitude, string InOut)
    {
        if (InOut == "In")
        {
            shakingIn = true;
        }
        else
        {
            shakingIn = false;
        }
        
        float percent = 0;
        
        while (percent < 1)
        {
            percent += Time.deltaTime * 1 / slideDuration;
            if (InOut == "In" && shakingIn)
            {
                slideMagnitude = Mathf.Lerp(0, maxMagnitude, percent);
                slideMagnitudeAtOut = slideMagnitude;
            } else if (InOut == "Out")
            {
                slideMagnitude = Mathf.Lerp(slideMagnitudeAtOut, 0, percent);
                if (slideMagnitude <= 0f)
                {
                    transform.localPosition = _originalPos;
                    _isShaking = false;
                    slideMagnitudeAtOut = 0f;
                }
            }
            yield return null;
        }
    }
}
