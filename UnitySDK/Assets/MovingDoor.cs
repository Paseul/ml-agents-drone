using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingDoor : MonoBehaviour {

    Vector3 velocity = new Vector3(0.0f, 1.0f, 0.0f);
    float floorHeight = 0.0f;

    // Use this for initialization
    void Start ()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position += velocity * Time.fixedDeltaTime;
        if (transform.position.y >= 6.5f)
        {
            velocity.y = -velocity.y;
        }
        if (transform.position.y <= -3.5f)
        {
            velocity.y = -velocity.y;
        }
    }
}
