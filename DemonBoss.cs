using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonBoss : MonoBehaviour
{
    private Animator anim;

    private Vector3 currentVelocity;
    public float smoothTime = 0.3f;

    public int damageMin, damageMax;
    public GameObject damageParticles;

    private AudioManager AM;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        OpenEye();
        OpenMouth();
        Destroy(gameObject, 10f);
        AM = FindObjectOfType<AudioManager>();
        AM.Play("Demon Scream");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newPos = new Vector3(transform.position.x, transform.position.y - 0.8f, 0f);
        transform.position = Vector3.SmoothDamp(transform.position, newPos, ref currentVelocity, smoothTime);
    }


    public void OpenEye()
    {
        anim.SetBool("EyeOpen", true);
    }

    public void OpenMouth()
    {
        anim.SetBool("MouthOpen", true);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();
            if (player.invincible == false)
            {
                AM.Play("Damage");
                Vector3 closestPos = collision.bounds.ClosestPoint(transform.position);
                Destroy(Instantiate(damageParticles, closestPos, Quaternion.identity, transform), 3f);
                player.TakeDamage(DamageCalculator.DamageBasedOnHeight(Random.Range(damageMin, damageMax)));
            }
        }
    }
}
