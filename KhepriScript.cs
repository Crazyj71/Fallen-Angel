using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class KhepriScript: MonoBehaviour
{

    public GameObject Player;
    public GameObject StompWave;
    public float health;
    public float swordDamage;
    public float punchDamage;
    public int hitForce;
    private Rigidbody2D rb2d;
    public Image healthbar;
    private float initHealth;
    private Animator anim;
    private GameObject death;
    private GameObject lightningball;
    public AudioSource slash1;
    public AudioSource slash2;
    public AudioSource slash3;
    public AudioSource slash4;
    public AudioSource slashFinal;
    public AudioSource Stomp;
    public AudioSource Swipe;
    public AudioSource Music;
    public AudioSource MusicBoss;
    public AudioSource vocal1;
    private Vector2 fireDirection;
    public GameObject background;
    public GameObject Halo;
    public AudioSource MusicDefeat;

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
        Halo.SetActive(false);
        death = GameObject.Find("KhepriDeathAnimation");
        death.SetActive(false);
        rb2d = GetComponent<Rigidbody2D>();
        initHealth = health;
        anim = GetComponentInParent<Animator>();
        StompWave.SetActive(false);
        anim.SetBool("Swiping", false);
        anim.SetBool("Stomping", false);

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
            Halo.SetActive(true);
            Halo.transform.position = transform.position + new Vector3(0, 4);
            MusicBoss.Pause();
            MusicDefeat.Play();
            background.GetComponent<SpriteRenderer>().color = Color.gray;
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
  
    bool playedSound = false;
    bool playedSound2 = false;

    bool stomping = false;

    IEnumerator StompWaveAttack()
    {
        
        StompWave.SetActive(true);
        StompWave.transform.position = transform.position;
        StompWave.GetComponent<Rigidbody2D>().velocity = new Vector2(20*dir, 0);
        yield return new WaitForSeconds(3f);
        StompWave.SetActive(false);
    }

    IEnumerator StompAttack()
    {

        stomping = true;
        yield return new WaitForSeconds(Random.Range(0, 3));
            anim.SetBool("Stomping", true);
            yield return new WaitForSeconds(.5f);
            Stomp.Play();
        StartCoroutine(StompWaveAttack());
        yield return new WaitForSeconds(.5f);
            anim.SetBool("Stomping", false);
            stomping = false;
        
    }
    bool swiping = false;
    IEnumerator SwipeAttack()
    {
        
            swiping = true;
            yield return new WaitForSeconds(.5f);
            anim.SetBool("Swiping", true);
            Swipe.Play();
            yield return new WaitForSeconds(.5f);
            anim.SetBool("Swiping", false);
            swiping = false;
        
    }

    int dir = -1;
    // Update is called once per frame
    void Update()
    {

        if (Player.transform.position.x > transform.position.x && dir ==-1)
        {
            transform.rotation = Quaternion.Euler(0, 180f, 0);
            dir = 1;
            transform.position = new Vector3(transform.position.x - 4, transform.position.y);
        }
        else if ((Player.transform.position.x < transform.position.x && dir == 1))
        { transform.rotation = Quaternion.Euler(0, 0f, 0); dir = -1; transform.position = new Vector3(transform.position.x + 4, transform.position.y); }

        if (rb2d.transform.position.x - Player.transform.position.x < 20f
               && rb2d.transform.position.x - Player.transform.position.x > -20f && (rb2d.transform.position.y - Player.transform.position.y < 20f
               && rb2d.transform.position.y - Player.transform.position.y > -20f))
        {
            background.GetComponent<SpriteRenderer>().color = Color.red;
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
                    Music.Pause();
                    MusicBoss.Play();
                }
            }

            if (rb2d.transform.position.x - Player.transform.position.x < 8f
               && rb2d.transform.position.x - Player.transform.position.x > -8f && (rb2d.transform.position.y - Player.transform.position.y < 8f
               && rb2d.transform.position.y - Player.transform.position.y > -8f))
            {

                if (stomping == false && swiping == false) StartCoroutine(SwipeAttack());
            }
            else
            {
                if (stomping == false && swiping == false) StartCoroutine(StompAttack());
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


    }

}