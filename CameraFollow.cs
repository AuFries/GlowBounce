using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public Transform target;
    public float smoothTime = 0.3f;
    public float yOffset;

    private Vector3 currentVelocity;

    // Update is called once per frame
    void LateUpdate()
    {
        if (target != null)
        {
            if (target.position.y + yOffset > transform.position.y)
            {
                Vector3 newPos = new Vector3(0f, target.position.y + yOffset, -10f);
                transform.position = Vector3.SmoothDamp(transform.position, newPos, ref currentVelocity, smoothTime);
            }
        }
    }


}
