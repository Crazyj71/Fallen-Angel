using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    private Vector3 offset;
    public GameObject Player;
    GameObject player;
    public float smoothTime = 0.3f;
    private Vector3 velocity = Vector3.zero;
    // Use this for initialization
    void Start()
    {
        player = GameObject.Find("Player");
        offset = transform.position - Player.transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        
            Vector3 targetPosition = new Vector3(Player.transform.position.x, Player.transform.position.y, 0) + offset;
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        
    }
}
