using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveVerticalPlatform : MonoBehaviour
{

    public float bottomLimit;
    public float topLimit;
    public float speed = 2.0f;
    private int direction = 1;
    Vector2 movement;
    private float pos;
    void Start()
    {
        pos = transform.localPosition.y;
    }

    void Update()
    {
        if (transform.localPosition.y > pos + topLimit)
        {
            direction = -1;
        }
        else if (transform.localPosition.y < pos - bottomLimit)
        {
            direction = 1;
        }
        movement = Vector2.up * direction * speed * Time.deltaTime;
        transform.Translate(movement);
    }
}
