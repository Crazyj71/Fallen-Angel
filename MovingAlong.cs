using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingAlong : MonoBehaviour {
    private GameObject target = null;
    private Vector3 offset;
    void Start()
    {
        target = null;
    }
    void OnCollisionStay2D(Collision2D col)
    {
        target = col.gameObject;
        offset = target.transform.position - transform.position;
    }
    void OnCollisionExit2D(Collision2D col)
    {
        target = null;
    }
    void LateUpdate()
    {
        if (target != null)
        {
            target.transform.position = transform.position + offset;
        }
    }
}
