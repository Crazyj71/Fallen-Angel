using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{

    public float rightLimit;
    public float leftLimit;
    public float speed = 2.0f;
    private int direction = 1;
    Vector2 movement;
    private float pos;
    void Start()
    {
        pos = transform.localPosition.x;
    }

    void Update()
    {
        if (transform.localPosition.x > pos + rightLimit)
        {
            direction = -1;
        }
        else if (transform.localPosition.x < pos - leftLimit)
        {
            direction = 1;
        }
        movement = Vector2.right * direction * speed * Time.deltaTime;
        transform.Translate(movement);
    }
}
