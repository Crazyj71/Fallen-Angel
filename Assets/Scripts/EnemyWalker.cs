using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class EnemyWalker : MonoBehaviour {

    public float runForce;
    public float maxRunSpeed;
    public float health;
    public float punchDamage;
    public float swordDamage;
    public int hitForce;
    public Image healthbar;
    private float initHealth;
    private GameObject death;

    public AudioSource slash1;
    public AudioSource slash2;
    public AudioSource slash3;
    public AudioSource slash4;
    public AudioSource slashFinal;


    void PlaySlash()
    {
        int a = Random.Range(0, 3);

        if (a == 0)
        {
            slash1.Play();
        }
        else if (a == 1)
        {
            slash2.Play();
        }
        else if (a == 2)
        {
            slash3.Play();
        }
        else slash4.Play();
    }


    private Rigidbody2D rb2d;
    float movementDirection = 1.0f;

    // Use this for initialization
    void Start () {
        rb2d = GetComponent<Rigidbody2D>();
        initHealth = health;
        death = GameObject.Find("DeathAnimation");
        death.SetActive(false);
    }



    void Movement()
    {
        float currentspeed = rb2d.velocity.x;
        if (currentspeed > 0)
        {
            if (currentspeed < maxRunSpeed)
            {
                rb2d.AddForce(new Vector2(movementDirection * runForce, 0.0f));
            }
        }
        else
        {
            if (currentspeed > maxRunSpeed*-1)
            {
                rb2d.AddForce(new Vector2(movementDirection * runForce, 0.0f));
            }
        }

        
    }


    void Damage(float damage, Collider2D other, int force)
    {
        Vector3 dir = other.transform.position - transform.position;
        dir = -dir.normalized;
        rb2d.AddForce(dir * force);
        health -= damage;
        healthbar.fillAmount = health / initHealth;

        if (health / initHealth < .5)
        {
            healthbar.color = Color.yellow;
        }
        if (health / initHealth < .25)
        {
            healthbar.color = Color.red;
        }
        if (health <= 0){
            slashFinal.Play();
            death.SetActive(true);
            death.transform.position = transform.position;
            rb2d.gameObject.SetActive(false);
        }
        else
        {
            PlaySlash();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("BasicPlatform"))
        {
            movementDirection *= -1.0f;
        }
        if(other.gameObject.CompareTag("Sword"))
        {
            Damage(swordDamage, other.GetComponentInParent<Collider2D>(), hitForce);
        }
        if (other.gameObject.CompareTag("Arm"))
        {
            Damage(punchDamage, other.GetComponentInParent<Collider2D>(), hitForce);
        }
    }
    void FixedUpdate()
    {
        Movement();


    }
    // Update is called once per frame
    void Update () {
        if (movementDirection < 0)
        {
            transform.rotation = Quaternion.Euler(0, 180f, 0);
        }
        else transform.rotation = Quaternion.Euler(0, 0f, 0);

    }
}
