using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{
    public bool DevilDefeated;
    public bool takingDamage;
    public float friction;
    public float runForce;
    public float jumpForce;
    public float maxRunSpeed = 20f;
    public bool hasSword;
    public bool facingRight;
    public float health = 100f;
    public float maxHealth;
    public Text healthText;
    public Text lavaText;
    public GameObject death;
    private bool paused;
    public bool hasHalo;
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

    public bool hasWings;
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
    public AudioSource BlastSound;
    public AudioSource soundCollectSpirit;
    public int jumpCount;
    public int jumpCountMax;
    public Image healthbar;
    private GameObject pauseScreen;
    public GameObject Blast;
    private bool isShooting;
    private Vector2 fireDirection;
    public GameObject PlayerGlow;
    private GameObject Platform;
    public Text SpiritText;
    private int SpiritCount;
    public int SpiritMax;
    private GameObject Score;
    private Text scoreText;
    private Scene Current;
    private int TotalSpirits;
    private float BlastVelocity;
    private float BlastDuration;
    public Text DialogueBox;



    void Start()
    {
        DevilDefeated = false;
        BlastDuration = 1.0f;
        BlastVelocity = 10.0f;
        TotalSpirits = 0;
        TotalSpirits += (PlayerPrefs.GetInt("HellSpiritCount") + PlayerPrefs.GetInt("EarthSpiritCount") + PlayerPrefs.GetInt("HeavenSpiritCount"));
        Current = SceneManager.GetActiveScene();
        Score = GameObject.Find("Score");
        scoreText = Score.GetComponent<Text>();
        scoreText.text = "Score: 0";
        PlayerPrefs.SetInt("CurrentScore", 0);
        SpiritCount = 0;
        SpiritText.text = SpiritCount.ToString() + " of " + SpiritMax.ToString();
        Blast = GameObject.Find("Blast");
        Blast.SetActive(false);
        //anim.SetBool("hasHalo", false);
        pauseScreen = GameObject.Find("PauseScreen");
        pauseScreen.SetActive(false);
        jumpCount = 0;
        jumpCountMax = 1;
        PlayerPrefs.SetString("lastLoadedScene", SceneManager.GetActiveScene().name);
        takingDamage = false;
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

    void UpdateSpirits()
    {
        if (SpiritCount > PlayerPrefs.GetInt(Current.name + "SpiritCount"))
        {
            if (SpiritCount > SpiritMax) { SpiritCount = SpiritMax; }

            PlayerPrefs.SetInt(Current.name + "SpiritCount", SpiritCount);
        }
    }



    IEnumerator WinGame()
    {
        yield return new WaitForSeconds(20f);
        SceneManager.LoadScene("Thanks");
    }


    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.P))
        {
            paused = !paused;
            if (paused == true)
            {
                pauseScreen.SetActive(true);
                Time.timeScale = 0.0f;
            }
            else
            {
                pauseScreen.SetActive(false);
                Time.timeScale = 1.0f;
            }
        }

        if (paused == false)
        {
           
            if(DevilDefeated == true && Current.name == "FinalBoss")
            {
                StartCoroutine(WinGame());
            }

            ShowScore();

            if (transform.position.y < -100)
            {
                SceneManager.LoadScene("GameOver");
            }

            if (hasWings)
            {
                Wings.SetActive(true);
                jumpCountMax = 2;

               
            }


            if (Input.GetKeyDown(KeyCode.T))
            {
                this.transform.position = new Vector2(-6, 450);
                CollectSword();
            }
            

            dangerTime -= Time.deltaTime;
            if (dangerTime < 0)
            {
                DangerText.SetActive(false);
            }

            if (Input.GetKeyDown(KeyCode.Space) && grounded == true)
            {
                jump = true;
                jumpCount++;
            }else if (Input.GetKeyDown(KeyCode.Space) && jumpCount < jumpCountMax-1 && grounded == false && hasWings)
            {
                Jump();
                jumpCount++;
            }

            if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow))
            {
                running = false;
            }

            if (rb2d.IsTouchingLayers(LayerMask.NameToLayer("Platforms")) || rb2d.IsTouchingLayers(LayerMask.NameToLayer("MovingPlatform")))
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
                SceneManager.LoadScene("GameOver");
            }

            //Let user press escape to go to the main menu
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Time.timeScale = 1;
                Cursor.visible = true;
                SceneManager.LoadScene("MainMenu");
            }


        }
    }


    IEnumerator BlastScript()
    {
        isShooting = true;
        BlastSound.Play();
        if (transform.rotation.y > 0)
        {
            fireDirection = new Vector2(-1, 0);
            Blast.transform.rotation = Quaternion.Euler(0, 180f, 0);
        }
        else
        {
            fireDirection = new Vector2(1, 0);
            Blast.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
            fireDirection = fireDirection.normalized;
        Blast.SetActive(true);
        Blast.transform.position = transform.position;
        Blast.GetComponent<Rigidbody2D>().velocity = new Vector2(fireDirection.x * BlastVelocity + rb2d.velocity.x, 0);
        yield return new WaitForSeconds(BlastDuration);
        Blast.SetActive(false);
        isShooting = false;
    }


    bool kicking = false;
    IEnumerator KickDelay()
    {
        kicking = true;
        swoosh.Play();
        yield return new WaitForSeconds(.5f);
        attacking = false;
        kicking = false;
    }


    IEnumerator AttackDelay()
    {
        swoosh.Play();
        yield return new WaitForSeconds(.1f);
        Sword.SetActive(true);
        yield return new WaitForSeconds(.1f);
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
            Platform = other.gameObject;
           // jumpCount = 0;
            //grounded = true;
        }
        else if (other.gameObject.CompareTag("DisPlatform"))
        {
            Platform = other.gameObject;
           // jumpCount = 0;
            //grounded = true;
        }
        else if (other.gameObject.CompareTag("FallingPlatform"))
        {
            Platform = other.gameObject;

            //jumpCount = 0;
            //grounded = true;
        }
        else if (other.gameObject.CompareTag("MovingPlatform"))
        {

            Platform = other.gameObject;

            //jumpCount = 0;
            //grounded = true;
        }
        else if (other.gameObject.CompareTag("Lava"))
        {
            //Damage(100, other, 0);
            SceneManager.LoadScene("GameOver");

        }
        else if (other.gameObject.CompareTag("EnemyWalker"))
        {
            Dialogue(7);
            Damage(10, other, 300);
            snakeBite.Play();

        }
        else if (other.gameObject.CompareTag("EnemyFlyer"))
        {
            Dialogue(9);
            Damage(10, other, 300);
            ghostAttack.Play();

        }
        
        else if (other.gameObject.CompareTag("EnemyAttacker"))
        {
            Dialogue(8);
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
            Dialogue(4);
            soundCollectSword.Play();
            CollectSword();
            other.gameObject.SetActive(false);
        }

        else if (other.gameObject.CompareTag("Heart"))
        {
            Dialogue(3);
            soundCollectHeart.Play();
            other.gameObject.SetActive(false);
            health += 50f;
            healthbar.fillAmount = health / maxHealth;
            healthbar.color = Color.white;

        }
        else if (other.gameObject.CompareTag("WingsPickup"))
        {
            Dialogue(5);
            hasWings = true;
            soundCollectWings.Play();
            other.gameObject.SetActive(false);
            Wings.SetActive(true);
            //rb2d.gravityScale = 2.5f;
        }
        else if (other.gameObject.CompareTag("HaloPickup"))
        {
            Dialogue(6);
            hasHalo = true;
            soundCollectHalo.Play();
            other.gameObject.SetActive(false);
            Halo.SetActive(true);
            //anim.SetBool("hasHalo", true);
        }
        else if (other.gameObject.CompareTag("Finish"))
        {
            UpdateSpirits();
            PlayerPrefs.SetInt("HellComplete", 1);
            PlayerPrefs.SetString("nextScene", "Earth");
            SceneManager.LoadScene("First Transitional Scene");
        }
        else if (other.gameObject.CompareTag("EarthFinish"))
        {
            UpdateSpirits();
            PlayerPrefs.SetInt("EarthComplete", 1);
            PlayerPrefs.SetString("nextScene", "Heaven");
            SceneManager.LoadScene("Second Transitional Scene");
        }
        else if (other.gameObject.CompareTag("HeavenFinish"))
        {
            UpdateSpirits();
            PlayerPrefs.SetInt("HeavenComplete", 1);
            PlayerPrefs.SetString("nextScene", "FinalBoss");
            SceneManager.LoadScene("FinalBoss");
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
            Platform = other.gameObject;
            SetGrounded();
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

        if (rb2d.IsTouchingLayers(LayerMask.GetMask("Platforms")) || rb2d.IsTouchingLayers(LayerMask.GetMask("MovingPlatform")))
        {
            
            anim.SetBool("isGrounded", true);
            if (Platform.transform.position.y < transform.position.y)
            {
                grounded = true; jumpCount = 0;
            }
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
          
        }

    }


    IEnumerator DialogueDisplay2(string s)
    {
        if (talking == false)
        {
            DialogueBox.text = s;
            yield return new WaitForSeconds(3.0f);
            DialogueBox.text = "";
        }
    }


    bool talking;

    IEnumerator DialogueDisplay(string s, string h, string e)
    {
        talking = true;
        DialogueBox.text = s;
        yield return new WaitForSeconds(3.0f);
        DialogueBox.text = h;
        yield return new WaitForSeconds(10.0f);
        DialogueBox.text = e;
        yield return new WaitForSeconds(3.0f);
        DialogueBox.text = "";
        talking = false;
    }

    void Dialogue(int type)
    {
        int SpiritNumber = SpiritCount;
        if (type == 1)
        {
            if (Current.name == "Hell")
            {
                if (SpiritNumber == 1)
                {
                    StartCoroutine(DialogueDisplay("Gabriel, Thank you!", "Here's a tip for saving me: The number on the bottom of your screen" +
                        " is how close the lava is to you.", "You must hurry and find the others!"));
                }
                if (SpiritNumber == 2)
                {
                    StartCoroutine(DialogueDisplay("Hey Fella!", "If you miss a platform, try jumping while pressed against its side to ledge jump.",
                        "This will come in handy!"));
                }
                if (SpiritNumber == 3)
                {
                    StartCoroutine(DialogueDisplay("Wow, you found me!", "The boss here has limited reach, don't stay too close or too far away" +
                        " for too long!", "Keep Going, Gabriel!"));
                }

            }

            if (Current.name == "Earth")
            {
                if (SpiritNumber == 1)
                {
                    StartCoroutine(DialogueDisplay("I knew you'd make it.", "My 4 friends are all near the maze, here...", "Please find them."));
                }
                if (SpiritNumber == 2)
                {
                    StartCoroutine(DialogueDisplay("Look who it is!!", "Gabriel, the maze is tough, but which direction do they always say to go first in a maze?",
                        "Was is left.... or no maybe it was right...."));
                }
                if (SpiritNumber == 3)
                {
                    StartCoroutine(DialogueDisplay("Yo Wassup", "Aye man, I'll tell you something good, 'kay? The more spirits you collect, something good may happen.",
                        "Aight my guy, good luck."));
                }
                if (SpiritNumber == 4)
                {
                    StartCoroutine(DialogueDisplay("Hi!", "Did you know your highscore will save, even if you die? But, unfortunately, we spirits do not save.","Bye!"));
                }
                if (SpiritNumber == 5)
                {
                    StartCoroutine(DialogueDisplay("Last but not least, baby Gaby!", "Your halo, huh? I think I saw it on the boss platform...", "Later Gaber"));
                }
            }

            if (Current.name == "Heaven")
            {
                if (SpiritNumber == 1)
                {
                    StartCoroutine(DialogueDisplay("Gabriel, be careful.", "Ghosts have manned our towers and fire rampantly!", "You must be quick."));
                }
                if (SpiritNumber == 2)
                {
                    StartCoroutine(DialogueDisplay("This weather sucks...", "But you must have faith that the Eastward winds will carry you.", "I need a vacation..."));
                }
                if (SpiritNumber == 3)
                {
                    StartCoroutine(DialogueDisplay("Behold!", "Woe be to the man that sets fire to the boards among moving platforms...", "You're better off jumping over."));
                }
                if (SpiritNumber == 4)
                {
                    StartCoroutine(DialogueDisplay("Gabriel, My friend!", "Beelzebub's throne is very close, I feel the dark energy rattling my bones...",
                        "...or I guess I would if I had bones..."));
                }
                if (SpiritNumber == 5)
                {
                    StartCoroutine(DialogueDisplay("Let's suit up, pal!", "Get ready, with God on Vacation, we have to show Beelzebub we mean business!", "" +
                        "For Glory!"));
                }

            }
        }else if(type == 2 && SpiritNumber > 0)
        {

            int r = Random.Range(0, 4);

            if(Mathf.CeilToInt(r)==1)
            StartCoroutine(DialogueDisplay2("That's hot!"));
           

        }
        else if (type == 3 && SpiritNumber > 0)
        {

            int r = Random.Range(0, 4);

            if (Mathf.CeilToInt(r) == 1)
                StartCoroutine(DialogueDisplay2("Phew..."));
            if (Mathf.CeilToInt(r) == 2)
                StartCoroutine(DialogueDisplay2("Refreshing!"));

        }
        else if (type == 4 && SpiritNumber > 0)
        {

            StartCoroutine(DialogueDisplay2("Swing it like you mean it!"));


        }
        else if (type == 5 && SpiritNumber > 0)
        {

            StartCoroutine(DialogueDisplay2("Heavenly. Nice work, Pal."));


        }
        else if (type == 6 && SpiritNumber > 0)
        {

            StartCoroutine(DialogueDisplay2("Burn it up, Gabey-baby!"));


        }
        else if (type == 7 && SpiritNumber > 0)
        {

            int r = Random.Range(0, 4);

            if (Mathf.CeilToInt(r) == 1)
                StartCoroutine(DialogueDisplay2("I hope its not poisonous..."));
            if (Mathf.CeilToInt(r) == 2)
                StartCoroutine(DialogueDisplay2("Ouch!"));

        }
        else if (type == 8 && SpiritNumber > 0)
        {

            int r = Random.Range(0, 4);

            if (Mathf.CeilToInt(r) == 1)
                StartCoroutine(DialogueDisplay2("En garde, demon!"));
            if (Mathf.CeilToInt(r) == 2)
                StartCoroutine(DialogueDisplay2("We're not done, yet!"));

        }
        else if (type == 9 && SpiritNumber > 0)
        {

            int r = Random.Range(0, 4);

            if (Mathf.CeilToInt(r) == 1)
                StartCoroutine(DialogueDisplay2("Spooky!"));
            if (Mathf.CeilToInt(r) == 2)
                StartCoroutine(DialogueDisplay2("Just run away!"));

        }

    }

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
                if (!soundRunning.isPlaying)
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
                anim.SetBool("isKicking", false);

            }
            if (attacking == true && grounded == true)
            {
                anim.SetBool("isSwingingSword", true);

            }
            if (attacking == false)
            {
                anim.SetBool("isSwingingSword", false);
                anim.SetBool("isKicking", false);
            }
            if (attacking == true && grounded == false)
            {
                anim.SetBool("isKicking", true);
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
                anim.SetBool("isKicking", false);

            }
            if (attacking == true && grounded == true)
            {
                anim.SetBool("isPunching", true);
            }
            if (attacking == false)
            {
                anim.SetBool("isPunching", false);
                anim.SetBool("isKicking", false);

            }
            if (attacking == true && grounded == false)
            {
                anim.SetBool("isKicking", true);
            }
        }


    }

    void Movement()
    {
        //IF USER IS PRESSING ARROW KEYS
        if ((Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow)))
        {

            if (Input.GetKey(KeyCode.RightArrow))
            {
                facingRight = true;

            }
            else
             if (Input.GetKey(KeyCode.LeftArrow))
            {
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
        anim.SetBool("isGrounded", true);

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

    IEnumerator TakeDamageDelay()
    {
        takingDamage = true;
        yield return new WaitForSeconds(.5f);
        takingDamage = false;
    }

    void Damage(int damage, Collision2D other, int force)
    {
        if (takingDamage == true || kicking == true) return;
        Vector3 dir = other.transform.position - transform.position;
        dir = -dir.normalized;
        rb2d.AddForce(dir * force);
        health -= damage;
        healthbar.fillAmount = health / maxHealth;
        healthbar.color = Color.white;

        if (health / maxHealth < .5)
        {
            healthbar.color = Color.yellow;
        }
        if (health / maxHealth < .25)
        {
            healthbar.color = Color.red;
        }

        StartCoroutine(TakeDamageColor());
        StartCoroutine(TakeDamageDelay());

    }

    void TriggerDamage(int damage, Collider2D other, int force)
    {
        if (takingDamage == true) return;

        Vector3 dir = other.transform.position - transform.position;
        dir = -dir.normalized;
        rb2d.AddForce(dir * force);
        health -= damage;
        healthbar.fillAmount = health / maxHealth;
        healthbar.color = Color.white;

        if (health / maxHealth < .5)
        {
            healthbar.color = Color.yellow;
        }
        if (health / maxHealth < .25)
        {
            healthbar.color = Color.red;
        }

        StartCoroutine(TakeDamageColor());
        StartCoroutine(TakeDamageDelay());

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("FireBall"))
        {
            TriggerDamage(10, collision, 100);
            collision.gameObject.SetActive(false);
            Dialogue(2);

        }
        if (collision.gameObject.CompareTag("Spirit"))
        {
            soundCollectSpirit.Play();
            collision.gameObject.GetComponent<AudioSource>().Pause();
            collision.gameObject.SetActive(false);
            PlayerGlow.SetActive(false);
            PlayerGlow.SetActive(true);
           
            Debug.Log(SpiritCount);
            SpiritCount += 1;
            Debug.Log(SpiritCount);

            SetSpiritCount();
            AddScore(1000);
            //Update Spirit Count
            Dialogue(1);
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
        if (hasHalo)
        {
            if (isShooting == false)
                StartCoroutine(BlastScript());
        }

        if (hasSword && grounded == true)
        {

            StartCoroutine(AttackDelay());
        }
        else if (hasSword==false && grounded==true)
        {
            StartCoroutine(PunchDelay());
        }
        else StartCoroutine(KickDelay());
    }

    void SetSpiritCount()
    {
        SpiritText.text = SpiritCount.ToString() + " of " + SpiritMax.ToString();
    }

    void AddScore(int points)
    {
        PlayerPrefs.SetInt("CurrentScore", PlayerPrefs.GetInt("CurrentScore") + points);


    }

    void ShowScore()
    {
        if (PlayerPrefs.GetInt("CurrentScore") > PlayerPrefs.GetInt(Current.name))
        {
            PlayerPrefs.SetInt(Current.name, PlayerPrefs.GetInt("CurrentScore"));
        }
        scoreText.text = "Highscore: " + PlayerPrefs.GetInt(Current.name) +
            "\nScore: " + PlayerPrefs.GetInt("CurrentScore").ToString();


    }

}

