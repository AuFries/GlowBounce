using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class LazerEnemy : MonoBehaviour
{

    Transform player;

    public float smoothTime = 0.3f;

    public GameObject bulletPrefab;

    private Vector3 currentVelocity;

    private Animator anim;

    

    public bool seekingPlayer = true;

    private bool runawayLeft;

    private Transform spawnPosition;

    private AudioManager AM;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>().transform;
        anim = GetComponent<Animator>();
        
        runawayLeft = (Random.value < 0.5f);
        spawnPosition = transform.Find("Spawn Position");
        AM = FindObjectOfType<AudioManager>();
    }

    private void FixedUpdate()
    {
        if (player != null)
        {
            if (seekingPlayer)
            {
                Vector3 newPos = new Vector3(player.position.x, transform.position.y, 0f);
                transform.position = Vector3.SmoothDamp(transform.position, newPos, ref currentVelocity, smoothTime);
            }
            else //move off screen
            {
                if (runawayLeft)
                {
                    transform.position -= new Vector3(Time.deltaTime, 0f, 0f);
                }
                else
                {
                    transform.position += new Vector3(Time.deltaTime, 0f, 0f);
                }
            }
        }
        
    }


    public void StartOpeningEyes()
    {
        anim.SetBool("Eyes Open", true);
    }

    public void Fire()
    {
        if (seekingPlayer)
        {
            AM.Play("LazerEnemyFire");
            Instantiate(bulletPrefab, spawnPosition.position, Quaternion.identity);
            anim.SetBool("Eyes Open", false);
            seekingPlayer = false;
            Destroy(gameObject, 10f);
        }
    }

}
