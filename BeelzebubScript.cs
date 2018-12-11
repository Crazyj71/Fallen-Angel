using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeelzebubScript : MonoBehaviour {
    public GameObject Particles;
    public GameObject Spirit;
    public PlayerScript P;
    public GameObject LightningEffect;
    public GameObject Player;
    public GameObject LightningStrike;
    public float health;
    public float swordDamage;
    public float punchDamage;
    public int hitForce;
    private Rigidbody2D rb2d;
    public Image healthbar;
    private float initHealth;
    private Animator anim;
    private GameObject death;
    public GameObject lightningball;
    public AudioSource slash1;
    public AudioSource slash2;
    public AudioSource slash3;
    public AudioSource slash4;
    public AudioSource slashFinal;
    public AudioSource Stomp;
    public AudioSource fireSound;
    public AudioSource Music;
    public AudioSource MusicBoss;
    public AudioSource vocal1;
    private Vector2 fireDirection;
    public GameObject background;
    public AudioSource MusicDefeat;
    public GameObject Sprite;
    public AudioSource AOEsound;
    public AudioSource AOEFIRE;
    public AudioSource Noo;
    public Text PowerUp;
    public Text SpiritText;
    private int SpiritCount;

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
        Particles.SetActive(false);
        Spirit.SetActive(false);
        SpiritCount = PlayerPrefs.GetInt("HellSpiritCount") + PlayerPrefs.GetInt("EarthSpiritCount") + PlayerPrefs.GetInt("HeavenSpiritCount");
        SpiritText.text = SpiritCount.ToString();
        death = GameObject.Find("BeelzebubDeathAnimation");
        death.SetActive(false);
        rb2d = GetComponent<Rigidbody2D>();
        initHealth = health;
        anim = GetComponentInParent<Animator>();
        LightningStrike.SetActive(false);
        lightningball.SetActive(false);
        anim.SetBool("Firing", false);
        anim.SetBool("AOE", false);

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
            MusicBoss.Pause();
            Noo.Play();
            LightningStrike.SetActive(false);
            MusicDefeat.Play();
            AOEFIRE.Pause();
            AOEsound.Pause();
            P.DevilDefeated = true;
            Spirit.SetActive(false);
            Particles.SetActive(false);
            LightningEffect.SetActive(false);
            background.GetComponent<SpriteRenderer>().color = Color.white;
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
        Sprite.GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(.1f);
        Sprite.GetComponent<SpriteRenderer>().color = Color.white;

    }

    bool playedSound = false;
    bool playedSound2 = false;

    bool isShooting = false;
    IEnumerator FireAttack()
    {
        isShooting = true;
        anim.SetBool("Firing", true);
        yield return new WaitForSeconds(.3f);
        fireDirection = Player.transform.position - transform.position;
        fireDirection = fireDirection.normalized;
        fireDirection += new Vector2(0,-.2f);
        lightningball.SetActive(true);
        fireSound.Play();
        lightningball.transform.position = transform.position + new Vector3(dir * 2, 3, 0);
        anim.SetBool("Firing", false);
        Debug.Log(lightningball.transform.position);
        yield return new WaitForSeconds(1f);
        lightningball.SetActive(false);
        isShooting = false;
    }

    bool AOE = false;
    IEnumerator AOEattack()
    {

        AOE = true;
        AOEsound.Play();
        yield return new WaitForSeconds(.5f);
        anim.SetBool("AOE", true);
        AOEFIRE.Play();
        yield return new WaitForSeconds(.5f);
        anim.SetBool("AOE", false);
        Vector3 pos = new Vector3(Player.transform.position.x, transform.position.y + 2);
        yield return new WaitForSeconds(1f);
        LightningStrike.transform.position = pos;
        LightningStrike.SetActive(true);
        yield return new WaitForSeconds(2f);
        LightningStrike.SetActive(false);
        AOE = false;


    }



    IEnumerator UseSpirit()
    {
        Particles.SetActive(true);
        Spirit.SetActive(true);
        P.takingDamage = true;
        yield return new WaitForSeconds(10f);
        P.takingDamage = false;
        Particles.SetActive(false);
        Spirit.SetActive(false);
    }


    IEnumerator PowerUpText()
    {
        yield return new WaitForSeconds(10f);
        PowerUp.text = "Press Z to Use Collected Spirits";
        yield return new WaitForSeconds(5f);
        PowerUp.text = "";
    }

    int dir = -1;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z) && SpiritCount > 0)
        {
            //P.health += 50f;
            //P.healthbar.fillAmount = P.health / P.maxHealth;
            //P.healthbar.color = Color.white;
            SpiritCount--;
            StartCoroutine(UseSpirit());
        }

        if (Player.transform.position.x > transform.position.x && dir == -1)
        {
            transform.rotation = Quaternion.Euler(0, 180f, 0);
            dir = 1;
            //transform.position = new Vector3(transform.position.x - 4, transform.position.y);
        }
        else if ((Player.transform.position.x < transform.position.x && dir == 1))
        { transform.rotation = Quaternion.Euler(0, 0f, 0); dir = -1; transform.position = new Vector3(transform.position.x + 4, transform.position.y); }

        if (rb2d.transform.position.x - Player.transform.position.x < 30f
               && rb2d.transform.position.x - Player.transform.position.x > -30f && (rb2d.transform.position.y - Player.transform.position.y < 30f
               && rb2d.transform.position.y - Player.transform.position.y > -30f))
        {
            if (Camera.main.orthographicSize < 12)
            {
                Camera.main.orthographicSize += 1;
            }
            //zoomedOut = false;

            if (!vocal1.isPlaying && playedSound2 == false)
            {
                vocal1.Play();
                playedSound2 = true;
                if (!MusicBoss.isPlaying)
                {
                    StartCoroutine(PowerUpText());
                    Music.Pause();
                    MusicBoss.Play();
                }
            }

            if (rb2d.transform.position.x - Player.transform.position.x < 15f
               && rb2d.transform.position.x - Player.transform.position.x > -15f && (rb2d.transform.position.y - Player.transform.position.y < 15f
               && rb2d.transform.position.y - Player.transform.position.y > -15f))
            {
                if (isShooting == false && AOE == false) StartCoroutine(AOEattack());

            }
            else
            {
                if (isShooting == false && AOE == false) StartCoroutine(FireAttack());
            }


        }
        else
        {
            if (Camera.main.orthographicSize > 7)
            {
                Camera.main.orthographicSize -= 1;
            }
            //zoomedOut = true;
        }
        SpiritText.text = SpiritCount.ToString();
        lightningball.GetComponent<Rigidbody2D>().velocity = (fireDirection * 40);
        LightningStrike.transform.position += new Vector3(dir * .05f, 0); 
    }
}
