using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disappear_Platform : MonoBehaviour {
    public float DisDelay;
    public float RespawnDelay;
    private Rigidbody2D rb;
    private Collider2D c2d;
    private Animator anim;
    private Vector2 Position;
    void Start()
    {
        anim = GetComponent<Animator>();
        c2d = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        rb.gameObject.SetActive(true);
        Position = transform.position;
        anim.SetBool("Disappear", false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")){
            StartCoroutine(Delay());
        }
    }

    IEnumerator Delay()
    {
        anim.SetBool("Disappear", true);
        yield return new WaitForSeconds(DisDelay);
        transform.position += transform.position * 10;
        yield return new WaitForSeconds(RespawnDelay);
        anim.SetBool("Disappear", false);
        transform.position = Position;

    }
}
