using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_movement : MonoBehaviour {
    private Rigidbody2D rb2d;
    private Collider2D c2d;

    private float move = 10f;
    // Use this for initialization
    void Start () {
        rb2d = GetComponent<Rigidbody2D>();
        c2d = GetComponent<Collider2D>();
    }
	
	// Update is called once per frame
	void Update () {
        Vector2 movement = new Vector2(move, 0f);
        rb2d.AddForce(movement);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        move *= -1;
    }
}
