using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Transform player;
    Vector3 offset;
    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Drone_W").transform;
        offset = transform.position - player.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newPos = player.position + offset;
        transform.position = Vector3.Slerp(transform.position, newPos, 0.5f);
    }
}
