using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Platform : MonoBehaviour
{

    public float jumpForce = 10f;
    public float maxY;

    public bool dissolves = false;

    public bool boosts = false;

    public bool movingVertical = false;
    public bool movingHorizontal = false;

    public bool wobbles = false;

    public bool SloMo = false;

    public bool grayScale = false;

    public bool lazer = false;

    private AudioManager AM;

    private ParticleSystem playerParticles;

    private Color color;

    private float Ymin, Ymax;
    private bool goingUp;
    private float Xmin, Xmax;
    private bool goingRight;
    private ParticleSystem movingParticles;
    private ParticleSystem.MainModule movingParticlesMain;
    private ParticleSystem.ShapeModule movingParticlesShape;

    private float movementSpeed;
    private float movementDistance;

    private float maxRotation;
    private float rotationSpeed;

    private Player player;

    private GameObject breakParticlesPrefab;
    private GameObject sloMoParticlesPrefab;

    private TimeManager TM;
    private Grayscale GS;

    private void Start()
    {
        player = FindObjectOfType<Player>();
        AM = FindObjectOfType<AudioManager>();
        playerParticles = FindObjectOfType<Player>().GetComponent<ParticleSystem>();
        color = GetComponent<SpriteRenderer>().color;
        breakParticlesPrefab = Resources.Load("Prefabs/Break Particles") as GameObject;

        if (movingVertical)
        {
            movementDistance = Random.Range(0.5f, 3f);
            Ymin = transform.position.y - movementDistance/2;
            Ymax = transform.position.y + movementDistance/2;
            movementSpeed = Random.Range(0.1f,1f);
            goingUp = Random.value < 0.5f;
            maxY += Ymax;

            InitializeParticleSystem();
            
            if (goingUp)
            {
                movingParticlesShape.rotation = new Vector3(0, 0, 180f);
                movingParticlesShape.position = new Vector3(0, -0.5f, 0);
            } else
            {
                movingParticlesShape.rotation = new Vector3(0, 0, 0);
                movingParticlesShape.position = new Vector3(0, 0.5f, 0);
            }
        } else if (movingHorizontal)
        {
            movementDistance = Random.Range(0.5f, 3f);
            Xmin = transform.position.x - movementDistance/2;
            Xmax = transform.position.x + movementDistance/2;
            movementSpeed = Random.Range(0.1f, 1f);
            goingRight = Random.value < 0.5f;

            InitializeParticleSystem();
            
            if (goingRight) 
            {
                movingParticlesShape.rotation = new Vector3(0, 0, 90f);
                movingParticlesShape.position = new Vector3(-0.5f, 0, 0);
            } else
            {
                movingParticlesShape.rotation = new Vector3(0, 0, 270f);
                movingParticlesShape.position = new Vector3(0.5f, 0, 0);
            }
        }

        if (wobbles)
        {
            maxRotation = Random.Range(5f,20f);
            rotationSpeed = Random.Range(1f,3f);
        }

        if (SloMo)
        {
            TM = FindObjectOfType<TimeManager>();
            sloMoParticlesPrefab = Resources.Load("Prefabs/Slo Mo Particles") as GameObject;
        }

        if (grayScale)
        {
            GS = FindObjectOfType<Grayscale>();
        }

    }

    private void InitializeParticleSystem()
    {
        movingParticles = GetComponent<ParticleSystem>();
        movingParticlesMain = movingParticles.main;
        movingParticlesShape = movingParticles.shape;

        movingParticlesMain.startSpeedMultiplier = movementSpeed;
    }

    private void FixedUpdate()
    {
        if(movingVertical)
        {
            if (goingUp)
            {
                transform.Translate(Vector2.up * movementSpeed * Time.deltaTime);
            }
            else
            {
                transform.Translate(Vector2.down * movementSpeed * Time.deltaTime);
            }

            if (transform.position.y >= Ymax)
            {
                goingUp = false;
                movingParticlesShape.rotation = new Vector3(0, 0, 0);
                movingParticlesShape.position = new Vector3(0, 0.5f, 0);

            } else if (transform.position.y < Ymin)
            {
                goingUp = true;
                movingParticlesShape.rotation = new Vector3(0, 0, 180f);
                movingParticlesShape.position = new Vector3(0, -0.5f, 0);

            }
            
        } else if (movingHorizontal)
        {
            if (goingRight)
            {
                transform.Translate(Vector2.right * movementSpeed * Time.deltaTime);
            } else
            {
                transform.Translate(Vector2.left * movementSpeed * Time.deltaTime);
            }

            if (transform.position.x >= Xmax)
            {
                goingRight = false;
                movingParticlesShape.rotation = new Vector3(0,0,270f);
                movingParticlesShape.position = new Vector3(0.5f, 0, 0);
            } else if (transform.position.x < Xmin)
            {
                goingRight = true;
                movingParticlesShape.rotation = new Vector3(0, 0, 90f);
                movingParticlesShape.position = new Vector3(-0.5f, 0, 0);
            }
        }

        if (wobbles)
        {
            transform.rotation = Quaternion.Euler(0f,0f, maxRotation * Mathf.Sin(Time.time * rotationSpeed));
        }
        
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.relativeVelocity.y <= 0f)
        {
            if (player.controllable) //Platforms automatically remove 1 HP every bounce
            {
                player.HP -= 1;
            }

            Rigidbody2D RB = collision.collider.GetComponent<Rigidbody2D>();

            player.StopWobble();
            if (wobbles)
            {
                Vector2 wobbleVelocity = transform.up * jumpForce;
                RB.velocity = transform.up * jumpForce;
                player.Wobble(wobbleVelocity.x);
            } else
            {
                if (!player.controllable)
                {
                    player.GetComponent<MenuPlayer>().moveToNextPlatform();
                }
                RB.velocity = new Vector2(RB.velocity.x, jumpForce);
                
            }

            if (SloMo)
            {
                TM.DoSlowMotion();
                GameObject particles = Instantiate(sloMoParticlesPrefab, player.transform);
                Destroy(particles, 2f);
                //StartCoroutine(DestroyParticles(Instantiate(sloMoParticlesPrefab, player.transform)));
            }

            Animator anim = collision.collider.GetComponent<Animator>();
            anim.SetTrigger("Bounce");
            
            if (SceneManager.GetActiveScene().name == "Main") //Only play if currently in the game
            {
                AM.Play("Bounce");
            }
            

            if (boosts)
            {
                player.Boost();
                AM.Play("Boost");
            }

            if (lazer)
            {
                GetComponent<LazerPlatform>().StartShootingLazer();
            }

            ParticleSystem.MainModule psMain = playerParticles.main;
            psMain.startColor = color;
            playerParticles.Play();

            if (dissolves)
            {
                GetComponent<Dissolve>().Begin();
            } else if (grayScale)
            {
                GS.Begin();
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.GetComponent<Rigidbody2D>().velocity.y <= 0f && collision.CompareTag("Player"))
        {
            AM.Play("Crumble");
            StartCoroutine(DestroyParticles(Instantiate(breakParticlesPrefab, transform.position, Quaternion.identity)));
            Destroy(gameObject);
        }
            
    }

    public IEnumerator DestroyParticles(GameObject particles)
    {
        yield return new WaitForSeconds(2f);
        Destroy(particles);
    }
}
