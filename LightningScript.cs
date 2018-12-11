using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LightningScript : MonoBehaviour
{
    public GameObject lightning;
    public bool isStriking;
    public AudioSource LightingSound;
    public GameObject background;
    private Color OriginalColor;
    public GameObject Player;
    public float windForce;


    void Start()
    {
        lightning.SetActive(false);
        isStriking = false;
        OriginalColor = background.GetComponent<SpriteRenderer>().color;
    }


    IEnumerator DelayedSound()
    {
        yield return new WaitForSeconds(1f);
        LightingSound.Play();

    }

    IEnumerator LightningStrike()
    {
        isStriking = true;
        float seconds = Random.Range(3.0f, 15.0f);
        float xOffset = Random.Range(-30, 30);
        yield return new WaitForSeconds(seconds);
        lightning.SetActive(true);
        background.GetComponent<SpriteRenderer>().color = Color.white;
        lightning.transform.position = new Vector2(Camera.main.transform.position.x + xOffset, Camera.main.transform.position.y);
        StartCoroutine(DelayedSound());
        yield return new WaitForSeconds(.1f);
        lightning.SetActive(false);
        background.GetComponent<SpriteRenderer>().color = OriginalColor;
        isStriking = false;
    }

    // Update is called once per frame
    void Update()
    {

        if (isStriking == false)
        {
            StartCoroutine(LightningStrike());
        }
    }


    void FixedUpdate()
    {
        Player.GetComponent<Rigidbody2D>().AddForce(new Vector2(windForce, 0));
    }

}