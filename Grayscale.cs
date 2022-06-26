using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grayscale : MonoBehaviour
{

    public float grayscaleDuration = 2f;

    UnityEngine.Rendering.VolumeProfile profile;
    UnityEngine.Rendering.Universal.ColorAdjustments CA;
    UnityEngine.Rendering.Universal.FilmGrain FG;

    private float startSaturation;
    private float endSaturation;
    private float saturation;

    private float startIntensity;
    private float endIntensity;
    private float intensity;

    // Start is called before the first frame update
    void Start()
    {
        profile = GameObject.Find("Global Volume").GetComponent<UnityEngine.Rendering.Volume>().profile;
        profile.TryGet(out CA);
        profile.TryGet(out FG);
    }

    public void Begin()
    {
        StartCoroutine(DecreaseSaturationIncreaseFG());
    }

    public IEnumerator DecreaseSaturationIncreaseFG() //For grayscale effect.. changes to grayscale then goes back to normal
    {

        startSaturation = CA.saturation.value;
        endSaturation = -100f;

        startIntensity = 0f;
        endIntensity = 1f;

        float elapsedTime = 0f;

        while (elapsedTime < grayscaleDuration)
        {
            elapsedTime += Time.deltaTime;
            saturation = Mathf.Lerp(startSaturation, endSaturation, elapsedTime / grayscaleDuration);
            intensity = Mathf.Lerp(startIntensity, endIntensity, elapsedTime / grayscaleDuration);
            FG.intensity.Override(intensity);
            CA.saturation.Override(saturation);
            yield return null;
        }

        StartCoroutine(IncreaseSaturationDecreaseFG());
    }

    public IEnumerator IncreaseSaturationDecreaseFG()
    {
        startSaturation = CA.saturation.value;
        endSaturation = 0f;

        startIntensity = 1f;
        endIntensity = 0f;

        float elapsedTime = 0f;

        while (elapsedTime < grayscaleDuration)
        {
            elapsedTime += Time.deltaTime;
            saturation = Mathf.Lerp(startSaturation, endSaturation, elapsedTime / grayscaleDuration);
            intensity = Mathf.Lerp(startIntensity, endIntensity, elapsedTime / grayscaleDuration);
            FG.intensity.Override(intensity);
            CA.saturation.Override(saturation);
            yield return null;
        }
    }
}
