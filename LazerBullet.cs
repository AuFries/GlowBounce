using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazerBullet : MonoBehaviour
{
    public int damageMin, damageMax;
    public GameObject damageParticles;

    public float movementSpeed = 30f;

    private AudioManager AM;

    // Start is called before the first frame update
    void Start()
    {
        AM = FindObjectOfType<AudioManager>();
        Destroy(gameObject, 10f);
        gameObject.GetComponent<Rigidbody2D>().velocity = transform.up * movementSpeed;
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
