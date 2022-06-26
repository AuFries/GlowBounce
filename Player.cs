using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    public GameObject wobblingParticlesPrefab;

    public float speed = 10f;
    public float maxSpeedX = 10f;

    public int HP;
    public int maxHP;

    private float movement;
    public bool invincible = false;

    public bool controllable = true;

    Rigidbody2D RB;

    TrailRenderer TR;

    AudioManager AM;

    Animator anim;

    private bool wobbling = false;
    private float additiveXVelocity;

    private GameObject wobblingParticles;

    private AdsManager Ads;
    private UIhandler UIH;

    public Slider HealthSlider;

    private Image sliderImage;

    public Gradient gradient;

    public GameObject damageTextCanvas;

    private int defense;

    private float cameraWidth;

    // Start is called before the first frame update
    void Start()
    {
        cameraWidth = Camera.main.orthographicSize / 2;
        RB = GetComponent<Rigidbody2D>();
        TR = GetComponent<TrailRenderer>();
        AM = FindObjectOfType<AudioManager>();
        anim = GetComponent<Animator>();
        Ads = FindObjectOfType<AdsManager>();
        UIH = FindObjectOfType<UIhandler>();
        CustomizeHandler.ChangePlayerColors(transform.GetChild(0).gameObject, TR);
        if (controllable)
        {
            if (!PlayerPrefs.HasKey("PlayerHealth"))
            {
                PlayerPrefs.SetInt("PlayerHealth", 100);
                HP = 100;
                maxHP = 100;
            } else
            {
                HP = PlayerPrefs.GetInt("PlayerHealth");
                maxHP = PlayerPrefs.GetInt("PlayerHealth");
            }

            if (!PlayerPrefs.HasKey("PlayerDefense"))
            {
                PlayerPrefs.SetInt("PlayerDefense", 0);
                defense = 0;
            } else
            {
                defense = PlayerPrefs.GetInt("PlayerDefense");
            }

            HealthSlider.maxValue = maxHP;
            sliderImage = HealthSlider.gameObject.transform.GetChild(1).transform.GetChild(0).GetComponent<Image>();
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (controllable)
        {
            movement = Input.GetAxis("Horizontal");

            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);


                if (touch.position.x < Screen.width / 2)
                {
                    movement = -1f;
                }
                else
                {
                    movement = 1f;
                }
            }

            if (wobbling)
            {
                RB.velocity = new Vector2(movement * speed + additiveXVelocity, RB.velocity.y);
            }
            else
            {
                RB.velocity = new Vector2(movement * speed, RB.velocity.y);
            }

            if (RB.velocity.x > maxSpeedX) //Clamps max speed (so it doesnt go wild)
            {
                RB.velocity = new Vector2(maxSpeedX, RB.velocity.y);
            }
            else if (RB.velocity.x < -maxSpeedX)
            {
                RB.velocity = new Vector2(-maxSpeedX, RB.velocity.y);
            }

            if (transform.position.x > cameraWidth) //Warps player to other side of screen
            {
                transform.position = new Vector3(-cameraWidth, transform.position.y, 0f);
                TR.Clear();
            }
            else if (transform.position.x < -cameraWidth)
            {
                transform.position = new Vector3(cameraWidth, transform.position.y, 0f);
                TR.Clear();
            }

            if (transform.position.y <= Camera.main.transform.position.y - Camera.main.orthographicSize - 2f) //Player dies if falls off map
            {
                Die();
            }

            if (HP <= 0)
            {
                Die();
            }
            UpdateHealthSlider();

            if ((float)HP / maxHP <= 0.1f)
            {
                LightManager.red = true;
            } else
            {
                LightManager.red = false;
            }
        }


    }

    private void UpdateHealthSlider()
    {
        float percentage = ((float) HP ) / maxHP;

        sliderImage.color = gradient.Evaluate(percentage);
        HealthSlider.value = (int) Mathf.Clamp(HP,0,HealthSlider.maxValue);
    }


    public void Wobble(float wobbleVelocity)
    {
        additiveXVelocity = wobbleVelocity;
        wobblingParticles = Instantiate(wobblingParticlesPrefab,transform);
        StartCoroutine(Wobbling());
    }

    public void StopWobble()
    {
        if (wobblingParticles != null)
        {
            Destroy(wobblingParticles);
        }
        wobbling = false;
        StopCoroutine(Wobbling());
    }

    public IEnumerator Wobbling()
    {
        wobbling = true;
        yield return new WaitForSeconds(1f);
        wobbling = false;
        Destroy(wobblingParticles);
    }

    public void TakeDamage(int amount)
    {
        if (!invincible)
        {
            StartCoroutine(Invincible(1f));
            AM.Play("Damage");
            int damageToGive = amount - ((int)(amount * (0.02f * defense)));
            HP -= damageToGive;

            GameObject tapText = Instantiate(damageTextCanvas, transform.position, Quaternion.identity);

            tapText.transform.GetChild(0).GetComponent<TMP_Text>().text = damageToGive.ToString();

            if (HP <= 0)
            {
                UpdateHealthSlider();
                Die();
            }

        }
        
    }

    public void Die() //Calls when player dies
    {
        AM.Play("Player Death");
        Destroy(gameObject);
        Ads.TryShowAd();
        UIH.ShowGameOverPanel();


        ScoreManager SM = FindObjectOfType<ScoreManager>();
        StatsManager.AddCredits(SM.credits);
        StatsManager.Score(SM.score);
        StatsManager.AddDeath();
    }

    public void Boost()
    {
        StartCoroutine(InvincibleBoost());
    }

    public IEnumerator InvincibleBoost()
    {
        invincible = true;
        anim.SetBool("Invincible", true);
        while (RB.velocity.y >= 1f)
        {
            yield return null;
        }
        invincible = false;
        anim.SetBool("Invincible", false);
    }

    public IEnumerator Invincible(float time)
    {
        invincible = true;
        anim.SetBool("Invincible", true);
        yield return new WaitForSeconds(time);
        invincible = false;
        anim.SetBool("Invincible", false);
    }
}
