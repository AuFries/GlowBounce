using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinningEnemy : MonoBehaviour
{

    public int damageMin, damageMax;

    public GameObject damageParticles;

    private bool rotatingClockwise;

    private Rigidbody2D RB;
    private Vector3 eulerRotationVelocity;

    // Start is called before the first frame update
    void Start()
    {
        rotatingClockwise = Random.value <= 0.5f;
        RB = GetComponent<Rigidbody2D>();

        if (rotatingClockwise)
        {
            eulerRotationVelocity = new Vector3(0, 0, Random.Range(25,30f));
        } else
        {
            eulerRotationVelocity = new Vector3(0, 0, Random.Range(-30, -25f));
        }
    }

    private void FixedUpdate()
    {
        Quaternion deltaRotation = Quaternion.Euler(eulerRotationVelocity * Time.fixedDeltaTime);
        RB.MoveRotation(transform.rotation * deltaRotation);
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();
            if (player.invincible == false)
            {
                Vector3 closestPos = collision.bounds.ClosestPoint(transform.position);
                Destroy(Instantiate(damageParticles, closestPos, Quaternion.identity, transform), 3f);
                player.TakeDamage(DamageCalculator.DamageBasedOnHeight(Random.Range(damageMin, damageMax)));
            }
        }
    }


    
}
