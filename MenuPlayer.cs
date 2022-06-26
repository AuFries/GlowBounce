using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPlayer : MonoBehaviour
{
    private bool moveLeft;
    float xSpeed;

    private Rigidbody2D RB;

    // Start is called before the first frame update
    void Start()
    {
        RB = GetComponent<Rigidbody2D>();
        moveLeft = false;
        xSpeed = 3.5f;
        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (moveLeft)
        {
            if (transform.position.x <= -1.5f)
            {
                RB.velocity = new Vector2(0f, RB.velocity.y);
            }
            else
            {
                RB.velocity = new Vector2(-xSpeed, RB.velocity.y);
            }
        }
        else
        {
            if (transform.position.x >= 1.5f)
            {
                RB.velocity = new Vector2(0f, RB.velocity.y);
            }
            else
            {
                RB.velocity = new Vector2(xSpeed, RB.velocity.y);
            }
        }
    }

    public void moveToNextPlatform()
    {
        moveLeft = !moveLeft;
    }
}
