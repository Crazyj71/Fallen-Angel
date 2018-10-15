using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerScript : MonoBehaviour
{
    public float friction;
    public float runForce;
    public float jumpForce;
    public float maxRunSpeed = 20f;
    public bool hasSword;
    public bool facingRight;
    public float health = 100f;

    private Animator anim;
    private Rigidbody2D rb2d;
    private Collider2D c2d;
    public bool grounded;
    private bool running;
    private bool attacking;
    public bool jump = false;
    private GameObject movingPlatform;
    private GameObject SwordSwing;
    public GameObject Sword;

    // Use this for initialization

    void Animate()
    {
        if (hasSword)
        {
            if (grounded == true && running == false)
            {
                anim.SetBool("isRunning", false);

            }
            else if (grounded == true && running == true)
            {
                anim.SetBool("isRunning", true);
            }
            else if (grounded == false)
            {
                anim.SetBool("isGrounded", false);
            }
            else if (grounded == true)
            {
                anim.SetBool("isGrounded", true);
            }
            else if (attacking == true)
            {
          //      this.GetComponent<Animator>().runtimeAnimatorController = animSwingingSword as RuntimeAnimatorController;

            }
        }
        else
        {
            if (grounded == true && running == false)
            {
                anim.SetBool("isRunning", false);

            }
            else if (grounded == true && running == true)
            {
                anim.SetBool("isRunning", true);

            }
            else if (grounded == false)
            {
                anim.SetBool("isGrounded", false);
            }
            else if (grounded == true)
            {
                anim.SetBool("isGrounded", true);
            }
            else if (attacking == true)
            {
                //this.GetComponent<Animator>().runtimeAnimatorController = animPunching as RuntimeAnimatorController;

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

        if (hasSword)
        {
          //  this.GetComponent<Animator>().runtimeAnimatorController = animJumpingWithSword as RuntimeAnimatorController;

        }
        else
        {
            //this.GetComponent<Animator>().runtimeAnimatorController = animJumping as RuntimeAnimatorController;

        }

    }

    void Damage(int damage, Collision2D other, int force)
    {
        Vector3 dir = other.transform.position - transform.position;
        dir = -dir.normalized;
        rb2d.AddForce(dir * force);
        health -= damage;

    }

    void CollectSword()
    {
        hasSword = true;
    }

    void Attack()
    {
        attacking = true;
        if (hasSword)
        {
                Sword.SetActive(true);
                
                StartCoroutine(AttackDelay());
        }
    }

    void Start()
    {
        anim = GetComponentInParent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        c2d = GetComponent<PolygonCollider2D>();
        Sword.SetActive(false);
    }


    // Update is called once per frame
    void Update()
    {
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
        

    }

    IEnumerator AttackDelay()
    {
        yield return new WaitForSeconds(.25f);
        Sword.SetActive(false);
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
            Time.timeScale = 0;
        }
        else if (other.gameObject.CompareTag("EnemyWalker"))
        {
            Damage(10, other, 300);
            

        }else if (other.gameObject.CompareTag("SwordPickup"))
        {
            CollectSword();
            other.gameObject.SetActive(false);
        }

        if (grounded == true)
        {
            anim.SetBool("isGrounded", true);
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
    
    void FixedUpdate()
    {

        Animate();
        Movement();

        if (jump)
        {
            Jump();
            grounded = false;
            anim.SetBool("isGrounded", false);
        }
    }
}

