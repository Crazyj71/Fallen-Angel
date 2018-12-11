using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class EnemyFlyerScript : MonoBehaviour {

    public GameObject Player;
    public float runForce;
    public float attackForce;
    public float maxRunSpeed;
    public float maxMoveTime;
    public float health;
    public float swordDamage;
    public float punchDamage;
    public int hitForce;
    public float dist;
    private Rigidbody2D rb2d;
    private float movementDirection = 1.0f;
    private float moveTime;
    private bool playerSpotted;
    public Image healthbar;
    private float initHealth;
    private Animator anim;
    private GameObject death;
    private GameObject fireball;
    public AudioSource slash1;
    public AudioSource slash2;
    public AudioSource slash3;
    public AudioSource slash4;
    public AudioSource slashFinal;
    public AudioSource ghostlyAttack;
    public AudioSource ghostShooting;
    private bool isShooting;
    private Vector2 fireDirection;
    // Use this for initialization
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


    void Start()
    {
        death = GameObject.Find("DeathAnimation");
        fireball = GameObject.Find("FireBall");
        death.SetActive(false);
        fireball.SetActive(false);
        playerSpotted = false;
        rb2d = GetComponent<Rigidbody2D>();
        moveTime = 0.0f;
        initHealth = health;
        anim = GetComponentInParent<Animator>();
    }

    void Movement()
    {

        moveTime += Time.deltaTime;

        if (moveTime > maxMoveTime) {
            moveTime = 0.0f;
            movementDirection *= -1.0f;
            rb2d.velocity = new Vector2(0.0f, 0.0f);
        }


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
            if (currentspeed > maxRunSpeed * -1)
            {
                rb2d.AddForce(new Vector2(movementDirection * runForce, 0.0f));
            }
        }


    }

    void MoveTowardsPlayer()
    {
        Vector2 attackDirection = (Player.transform.position - this.transform.position);

        if (rb2d.velocity.x < maxRunSpeed && rb2d.velocity.y < maxRunSpeed)
            rb2d.AddForce(attackDirection * attackForce);
    }
    void AddScore(int points)
    {
        PlayerPrefs.SetInt("CurrentScore", PlayerPrefs.GetInt("CurrentScore") + points);
    }

    IEnumerator Shoot()
    {
        isShooting = true;
        fireDirection = Player.transform.position-transform.position;
        fireDirection = fireDirection.normalized;
        anim.SetBool("firing", true);
        fireball.SetActive(true);
        ghostShooting.Play();
        fireball.transform.position = transform.position;
        
        Debug.Log(fireball.transform.position);
        yield return new WaitForSeconds(3f);
        fireball.SetActive(false);
        anim.SetBool("firing", false);
        isShooting = false;
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        /*
        if (other.gameObject.CompareTag("Player"))
        {
           playerSpotted = true;
        }*/
        if (other.gameObject.CompareTag("Blast"))
        {
            Damage(punchDamage, other.GetComponentInParent<Collider2D>(), hitForce);

        }

        if (other.gameObject.CompareTag("Sword"))
        {
            Damage(swordDamage, other.GetComponentInParent<Collider2D>(), hitForce);
        }

        if (other.gameObject.CompareTag("Arm"))
        {
            Damage(punchDamage, other.GetComponentInParent<Collider2D>(), hitForce);

        }
        if (other.gameObject.CompareTag("Boulder"))
        {
            Damage(100, other.GetComponentInParent<Collider2D>(), hitForce);

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
        if (health <= 0)
        {
            slashFinal.Play();
            death.SetActive(true);
            death.transform.position = transform.position;
            AddScore(250);
            fireball.SetActive(false);
            rb2d.gameObject.SetActive(false);
        }
        else
        {
            PlaySlash();
        }
        StartCoroutine(TakeDamageColor());

    }

    IEnumerator TakeDamageColor()
    {
        GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(.1f);
        GetComponent<SpriteRenderer>().color = Color.white;

    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerSpotted = false;
        }
    }


    bool playedSound = false;

    void FixedUpdate()
    {
        if (Mathf.Abs(transform.position.x - Player.transform.position.x) < dist && Mathf.Abs(transform.position.y - Player.transform.position.y) < dist)
        {
            MoveTowardsPlayer();
            if(!ghostlyAttack.isPlaying && playedSound == false)
            ghostlyAttack.Play();
            playedSound = true;
        }
        else
        {
            Movement();
            playedSound = false;
        }
        if (Mathf.Abs(transform.position.x - Player.transform.position.x) < dist + 4 && Mathf.Abs(transform.position.y - Player.transform.position.y) < dist + 4)
        {
            if (isShooting == false) StartCoroutine(Shoot());
        }
       


        fireball.GetComponent<Rigidbody2D>().AddForce(fireDirection * 10);
        

    }
    // Update is called once per frame
    void Update()
    {
        if (rb2d.velocity.x < 0)
        {
            transform.rotation = Quaternion.Euler(0, 180f, 0);
        }
        else transform.rotation = Quaternion.Euler(0, 0f, 0);

        if (fireball.GetComponent<Rigidbody2D>().velocity.x < 0)
        {
            fireball.transform.rotation = Quaternion.Euler(0, 180f, 0);
        }
        else fireball.transform.rotation = Quaternion.Euler(0, 0f, 0);
    }
}