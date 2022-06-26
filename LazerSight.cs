using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazerSight : MonoBehaviour
{

    private LazerEnemy lazerEnemy;
    private LineRenderer LR;

    private float timeBetweenBlinks = 1.5f;
    private float startIntensity = 1f;
    private float endIntensity = 0f;
    private float intensity;

    // Start is called before the first frame update
    void Start()
    {
        lazerEnemy = transform.GetComponentInParent<LazerEnemy>();
        LR = GetComponent<LineRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision) //Start blinking when finds player
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(BlinkLazer());
            lazerEnemy.StartOpeningEyes();
        }
    }

    public IEnumerator BlinkLazer()
    {
        while (lazerEnemy.seekingPlayer)
        {
            float elapsedTime = 0f;

            while (elapsedTime < timeBetweenBlinks)
            {
                elapsedTime += Time.deltaTime;
                intensity = Mathf.Lerp(startIntensity, endIntensity, elapsedTime / timeBetweenBlinks);
                Color lineColor = new Color(LR.startColor.a,LR.startColor.b,LR.startColor.g,intensity);
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
        Destroy(gameObject);
    }
}
