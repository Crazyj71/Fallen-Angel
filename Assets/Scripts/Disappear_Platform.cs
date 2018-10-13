using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disappear_Platform : MonoBehaviour {
    public float DisDelay;
    private Rigidbody2D rb;
    public PlayerScript player;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gameObject.SetActive(true);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")){
            StartCoroutine(Delay());
        }
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(DisDelay);
        player.grounded = false;

        GetComponent<Rigidbody2D>().Sleep();
    }
}
