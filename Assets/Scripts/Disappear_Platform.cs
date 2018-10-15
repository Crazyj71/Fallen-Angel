using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disappear_Platform : MonoBehaviour {
    public float DisDelay;
    private Rigidbody2D rb;
    private Collider2D c2d;
    public PlayerScript player;
    void Start()
    {
        c2d = GetComponent<Collider2D>();
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
        transform.position += transform.position * 10;


    }
}
