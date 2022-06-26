using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazerPlatform : MonoBehaviour
{
    public GameObject line;
    public GameObject lazerBulletPrefab;

    public float bulletChargeTime = 1f;

    private float timeBetweenBlinks = 0.3f;
    private float startIntensity = 1f;
    private float endIntensity = 0f;
    private float intensity;

    private LineRenderer LR;

    private bool chargingShot;

    // Start is called before the first frame update
    void Start()
    {
        LR = line.GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartShootingLazer()
    {
        line.SetActive(true);
        chargingShot = true;

        float theta;
        do
        {
            theta = Random.Range(0f,360f);
        } while ((theta >= 45 && theta <= 135) || (theta >= 225 && theta <= 315));

        transform.eulerAngles = Vector3.forward * theta;

        StartCoroutine(BlinkLazer());
        StartCoroutine(InitiateFiring());
    }


    public IEnumerator InitiateFiring()
    {
        yield return new WaitForSeconds(bulletChargeTime);
        chargingShot = false;
    }

    public IEnumerator BlinkLazer()
    {
        while (chargingShot)
        {
            float elapsedTime = 0f;

            while (elapsedTime < timeBetweenBlinks)
            {
                elapsedTime += Time.deltaTime;
                intensity = Mathf.Lerp(startIntensity, endIntensity, elapsedTime / timeBetweenBlinks);
                Color lineColor = new Color(LR.startColor.a, LR.startColor.b, LR.startColor.g, intensity);
                LR.startColor = lineColor;
                LR.endColor = lineColor;
                yield return null;
            }

            timeBetweenBlinks = Mathf.Clamp(timeBetweenBlinks - 0.05f, 0.1f, 1f);

            float swapIntensity = startIntensity;
            startIntensity = endIntensity;
            endIntensity = swapIntensity;

            yield return null;
        }
        ShootBullet();
        line.SetActive(false);
    }

    private void ShootBullet()
    { 
        Instantiate(lazerBulletPrefab, transform.GetChild(1).transform.position, transform.rotation);
    }
}
