using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class LightFlickering : MonoBehaviour
{
    public bool IsFlickering = true;

    public float MaxReduction = 0.2f;
    public float MaxIncrease = 0.2f;
    public float RateDamping = 0.1f;
    public float Strength = 300.0f;

    public Light2D Light;

    float _BaseIntensity;
    bool _isFlickering = false;

    // Start is called before the first frame update
    void Start()
    {
        if (Light == null)
        {
            IsFlickering = false;
            return;
        }

        _BaseIntensity = Light.intensity;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsFlickering && _isFlickering == false)
        {
            StartCoroutine(DoFlicker());
        }
    }

    IEnumerator DoFlicker()
    {
        _isFlickering = true;
        while (IsFlickering)
        {
            Light.intensity = Mathf.Lerp(Light.intensity, Random.Range(_BaseIntensity - MaxReduction, _BaseIntensity + MaxIncrease), Strength * Time.deltaTime);
            yield return new WaitForSeconds(RateDamping);
        }
        _isFlickering = false;
    }
}
