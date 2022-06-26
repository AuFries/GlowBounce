using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public float slowDownFactor = 0.2f;
    public float slowDownDuration = 5f;

    UnityEngine.Rendering.VolumeProfile profile;
    UnityEngine.Rendering.Universal.ChromaticAberration CA;

    private float startIntensity = 1f;
    private float endIntensity = 0f;
    private float intensity;

    private ScoreManager SM;

    private UIhandler UIH;

    private void Start()
    {
        profile = GameObject.Find("Global Volume").GetComponent<UnityEngine.Rendering.Volume>().profile;
        profile.TryGet(out CA);
        SM = FindObjectOfType<ScoreManager>();
        UIH = FindObjectOfType<UIhandler>();
    }
    void Update()
    {
        if (!UIH.gamePaused) //Change timescale if game isn't paused
        {
            Time.timeScale += (1f / slowDownDuration) * Time.unscaledDeltaTime;
            Time.fixedDeltaTime += (0.01f / slowDownDuration) * Time.unscaledDeltaTime;
            Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, SM.currentTimeScale);
            Time.fixedDeltaTime = Mathf.Clamp(Time.fixedDeltaTime, 0f, 0.01f);

        }
    }

    public void DoSlowMotion()
    {
        Time.timeScale = slowDownFactor;
        Time.fixedDeltaTime = Time.fixedDeltaTime * slowDownFactor;
        StartCoroutine(ChangeChromaticAbberation());
    }


    public IEnumerator ChangeChromaticAbberation()
    {
        float elapsedTime = 0f;

        while (elapsedTime < slowDownDuration)
        {
            elapsedTime += Time.deltaTime;
            intensity = Mathf.Lerp(startIntensity, endIntensity, elapsedTime / slowDownDuration);
            CA.intensity.Override(intensity);
            yield return null;
        }
    }
}
