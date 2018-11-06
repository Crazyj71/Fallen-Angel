using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{
    public float friction;
    public float runForce;
    public float jumpForce;
    public float maxRunSpeed = 20f;
    public bool hasSword;
    public bool facingRight;
    public float health = 100f;
    private float maxHealth;
    public Text healthText;
    public Text lavaText;
    public GameObject death;
    private bool paused;

    private Animator anim;
    private Rigidbody2D rb2d;
    public bool grounded;
    private bool running;
    private bool attacking;
    public bool jump = false;
    private GameObject movingPlatform;
    private GameObject SwordSwing;
    public GameObject Sword;
    public GameObject Arm;
    public GameObject Wings;
    public GameObject Halo;
    private GameObject DangerText;

    private bool hasWings;
    public float dangerTime;
    public AudioSource soundCollectSword;
    public AudioSource soundCollectHeart;
    public AudioSource soundCollectWings;
    public AudioSource soundCollectHalo;
    public AudioSource soundRunning;
    public AudioSource soundJumping;
    public AudioSource snakeBite;
    public AudioSource ghostAttack;
    public AudioSource swoosh;

    public Image healthbar;

    // Use this for initialization
    void Animate()
    {
        if (hasSword)
        {
            if (grounded == true && running == false)
            {
                anim.SetBool("isRunning", false);
                soundRunning.Pause();

            }
            if (grounded == true && running == true)
            {
                anim.SetBool("isRunning", true);
                if(!soundRunning.isPlaying)
                soundRunning.Play();
            }
            if (grounded == false)
            {
                soundRunning.Pause();
                anim.SetBool("isGrounded", false);
            }
            if (grounded == true)
            {
                anim.SetBool("isGrounded", true);
            }
            if (attacking == true)
            {
                anim.SetBool("isSwingingSword", true);

            }
            if (attacking == false)
            {
                anim.SetBool("isSwingingSword", false);
            }
        }
        else
        {
            if (grounded == true && running == false)
            {
                soundRunning.Pause();
                anim.SetBool("isRunning", false);

            }
            if (grounded == true && running == true)
            {
                if (!soundRunning.isPlaying)
                    soundRunning.Play();
                anim.SetBool("isRunning", true);

            }
            if (grounded == false)
            {
                soundRunning.Pause();
                anim.SetBool("isGrounded", false);
            }
            if (grounded == true)
            {
                anim.SetBool("isGrounded", true);
            }
            if (attacking == true)
            {
                anim.SetBool("isPunching", true);
            }
            if (attacking == false)
            {
                anim.SetBool("isPunching", false);
            }
        }


    }

    void Movement()
    {
        //IF USER IS PRESSING ARROW KEYS
        if ((Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow)))
        {

            if (Input.GetKey(KeyCode.RightArrow)){
                facingRight = true;

            }
            else 
             if (Input.GetKey(KeyCode.LeftArrow)){
                facingRight = false;
            }


            if (grounded) running = true;
            Vector2 velocity = rb2d.velocity;
            float moveHorizontal = Input.GetAxis("Horizontal");
            Vector2 movement = new Vector2(moveHorizontal, 0f);
            float currentspeed = moveHorizontal * rb2d.velocity.x;
            if (currentspeed < maxRunSpeed)
            {
                rb2d.AddForce(movement * runForce);
            }

            SetGrounded();
        }
        else if (grounded == true && !Input.GetKey(KeyCode.Space))
        {
            rb2d.velocity = friction * rb2d.velocity;
        }
        else running = false;

        
    }

     


    void Jump()
    {
        rb2d.velocity = new Vector2(rb2d.velocity.x, 0);
        rb2d.AddForce(new Vector2(0f, jumpForce));
        jump = false;

        soundJumping.Play();
    }

    IEnumerator TakeDamageColor()
    {
        GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(.1f);
        GetComponent<SpriteRenderer>().color = Color.white;

    }

    void Damage(int damage, Collision2D other, int force)
    {
        Vector3 dir = other.transform.position - transform.position;
        dir = -dir.normalized;
        rb2d.AddForce(dir * force);
        health -= damage;
        healthbar.fillAmount = health / maxHealth;
        healthbar.color = Color.green;

        if (health/maxHealth < .5)
        {
            healthbar.color = Color.yellow;
        }
        if (health / maxHealth < .25)
        {
            healthbar.color = Color.red;
        }

        StartCoroutine(TakeDamageColor());
    }

    void TriggerDamage(int damage, Collider2D other, int force)
    {
        Vector3 dir = other.transform.position - transform.position;
        dir = -dir.normalized;
        rb2d.AddForce(dir * force);
        health -= damage;
        healthbar.fillAmount = health / maxHealth;
        healthbar.color = Color.green;

        if (health / maxHealth < .5)
        {
            healthbar.color = Color.yellow;
        }
        if (health / maxHealth < .25)
        {
            healthbar.color = Color.red;
        }

        StartCoroutine(TakeDamageColor());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("FireBall"))
        {
            TriggerDamage(5, collision, 100);
            collision.gameObject.SetActive(false);

        }
    }

    void DamageTrigger(int damage, Collider2D other, int force)
    {
        Vector3 dir = other.transform.position - transform.position;
        dir = -dir.normalized;
        rb2d.AddForce(dir * force);
        health -= damage;

    }


    void CollectSword()
    {
        hasSword = true;
        anim.SetBool("hasSword", true);
    }

    void Attack()
    {
        attacking = true;
        if (hasSword)
        {
                
                StartCoroutine(AttackDelay());
        }
        else
        {
            StartCoroutine(PunchDelay());
        }
    }

    void Start()
    {
        paused = false;
        hasWings = false;
        hasSword = false;
        DangerText = GameObject.Find("DANGER");
        anim = GetComponentInParent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        Wings = GameObject.Find("PlayerWings");
        Halo = GameObject.Find("PlayerHalo");
        Sword.SetActive(false);
        Arm.SetActive(false);
        Wings.SetActive(false);
        Halo.SetActive(false);
        healthText.text = health.ToString();
        anim.SetBool("hasSword", false);
        maxHealth = health;
        
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            paused = !paused;
            if (paused == true)
            {
                Time.timeScale = 0.0f;
            }
            else
            {

                Time.timeScale = 1.0f;
            }
        }

        if (paused == false)
        {
            dangerTime -= Time.deltaTime;
            if (dangerTime < 0)
            {
                DangerText.SetActive(false);
            }

            if (Input.GetKeyDown(KeyCode.Space) && grounded == true)
            {
                jump = true;
            }

            if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow))
            {
                running = false;
            }

            if (rb2d.IsTouchingLayers(LayerMask.NameToLayer("Platforms")))
            {
                anim.SetBool("isGrounded", true);
            }

            if (facingRight == false)
            {
                transform.rotation = Quaternion.Euler(0, 180f, 0);
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }

            if (Input.GetKeyDown(KeyCode.X) && attacking == false)
            {
                Attack();
            }



            //Update Health
            if (health > maxHealth) health = maxHealth;
            healthText.text = health.ToString();

            //Update Lava Distance
            lavaText.text = Mathf.CeilToInt(transform.position.y - death.transform.position.y - 25).ToString();

            if (health <= 0)
            {
                SceneManager.LoadScene(2);
            }

            //Let user press escape to go to the main menu
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Time.timeScale = 1;
                Cursor.visible = true;
                SceneManager.LoadScene(0);
            }


        }
    }

    IEnumerator AttackDelay()
    {
        swoosh.Play();
        yield return new WaitForSeconds(.1f);
        Sword.SetActive(true);
        yield return new WaitForSeconds(.05f);
        Sword.SetActive(false);
        yield return new WaitForSeconds(.2f);
        attacking = false;
    }

    IEnumerator PunchDelay()
    {
        yield return new WaitForSeconds(.1f);
        Arm.SetActive(true);
        yield return new WaitForSeconds(.05f);
        Arm.SetActive(false);
        yield return new WaitForSeconds(.1f);
        attacking = false;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("BasicPlatform"))
        {
            
            grounded = true;
        }
        else if (other.gameObject.CompareTag("DisPlatform"))
        {
            grounded = true;
        }
        else if (other.gameObject.CompareTag("FallingPlatform"))
        {
            grounded = true;
        }
        else if (other.gameObject.CompareTag("MovingPlatform"))
        {
            grounded = true;
        }
        else if (other.gameObject.CompareTag("Lava"))
        {
            //Damage(100, other, 0);
            SceneManager.LoadScene(2);

        }
        else if (other.gameObject.CompareTag("EnemyWalker"))
        {
            Damage(10, other, 300);
            snakeBite.Play();

        }
        else if (other.gameObject.CompareTag("EnemyFlyer"))
        {
            Damage(5, other, 300);
            ghostAttack.Play();

        }
        
        else if (other.gameObject.CompareTag("EnemyAttacker"))
        {
            Damage(10, other, 300);


        }
        else if (other.gameObject.CompareTag("Boss"))
        {
            Damage(10, other, 500);


        }
        else if (other.gameObject.CompareTag("EnemySword"))
        {
            Damage(10, other, 300);

        }
        else if (other.gameObject.CompareTag("SwordPickup"))
        {

            soundCollectSword.Play();
            CollectSword();
            other.gameObject.SetActive(false);
        }

        else if (other.gameObject.CompareTag("Heart"))
        {
            soundCollectHeart.Play();
            other.gameObject.SetActive(false);
            health += 50f;
            healthbar.fillAmount = health / maxHealth;
            healthbar.color = Color.green;

        }
        else if (other.gameObject.CompareTag("WingsPickup"))
        {
            hasWings = true;
            soundCollectWings.Play();
            other.gameObject.SetActive(false);
            Wings.SetActive(true);
            rb2d.gravityScale = 2.5f;
        }
        else if (other.gameObject.CompareTag("HaloPickup"))
        {
            soundCollectHalo.Play();
            other.gameObject.SetActive(false);
            Halo.SetActive(true);
        }

        if (grounded == true)
        {
            anim.SetBool("isGrounded", true);
        }
    }
    
    void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("BasicPlatform"))
        {
            grounded = true;
        }
    }


    void OnCollisionExit2D(Collision2D other)
    {

        if (other.gameObject.CompareTag("BasicPlatform"))
        {
            grounded = false;
        }
        else if (other.gameObject.CompareTag("DisPlatform"))
        {
            grounded = false;
        }
        else if (other.gameObject.CompareTag("FallingPlatform"))
        {
            grounded = false;
        }
        else if (other.gameObject.CompareTag("MovingPlatform"))
        {
            grounded = false;
        }

    }

   void SetGrounded(){

        if (rb2d.IsTouchingLayers(LayerMask.GetMask("Platforms")))
        {
            anim.SetBool("isGrounded", true);
            grounded = true;
        }
        else
        {
            grounded = false;
        }
        if (grounded)
        {
            anim.SetBool("isGrounded", true);
        }
    }
    

    void FixedUpdate()
    {
        
        SetGrounded();

        Animate();
        Movement();

        if (jump)
        {
            Jump();
           // grounded = false;
           // anim.SetBool("isGrounded", false);
        }

    }
}

