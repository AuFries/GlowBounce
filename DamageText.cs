using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageText : MonoBehaviour
{

    public float lifeSpan;
    public float movementSpeed;

    private Vector2 directionToMove;
    private TMP_Text tapText;


    public float startAlpha = 1;
    public float endAlpha = 0;

    float changeRate = 0;
    float timeSoFar = 0;

    // Start is called before the first frame update
    void Start()
    {
        directionToMove = Random.insideUnitCircle.normalized;
        tapText = transform.GetChild(0).GetComponent<TMP_Text>();
        //startAlpha = tapText.alpha;
        StartCoroutine(FadeText());
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += (Vector3)(directionToMove * movementSpeed * Time.deltaTime);
    }


    IEnumerator FadeText()
    {
        changeRate = (endAlpha - startAlpha) / lifeSpan;
        SetAlpha(startAlpha);

        while (true)
        {
            timeSoFar += Time.deltaTime;

            if (timeSoFar > lifeSpan)
            {
                SetAlpha(endAlpha);
                Destroy(gameObject);
                yield break;
            }
            else
            {
                SetAlpha(tapText.alpha + (changeRate * Time.deltaTime));
            }

            yield return null;
        }
    }
    public void SetAlpha(float alpha)
    {
        tapText.alpha = Mathf.Clamp(alpha, 0, 1);
    }
}
