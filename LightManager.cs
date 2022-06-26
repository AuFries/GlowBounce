using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class LightManager : MonoBehaviour
{
    public static bool red = false;


    private Light2D light2d;


    // Start is called before the first frame update
    void Start()
    {
        light2d = gameObject.GetComponent<Light2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (red)
        {
            light2d.color = new Color(255f, 10f, 10f);
            light2d.intensity = 0.04f;
        } 
    }
}
