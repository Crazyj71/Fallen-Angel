using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour {

    public float rightLimit = 2.5f;
    public float leftLimit = -2.5f;
    public float speed = 2.0f;
    private int direction = 1;
    Vector2 movement;
    void Update()
    {
        if (transform.position.x > rightLimit)
        {
            direction = -1;
        }
        else if (transform.position.x < leftLimit)
        {
            direction = 1;
        }
        movement = Vector2.right * direction * speed * Time.deltaTime;
        transform.Translate(movement);
    }
}
