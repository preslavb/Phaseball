using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallScript : MonoBehaviour
{

    float constantVelocity = 15f;
    public Rigidbody2D rb;


    void Start()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = constantVelocity * (rb.velocity.normalized);

        if (gameObject.GetComponent<Rigidbody2D>().velocity.magnitude < 10 && BallManager.hasBallBeenTouched)
        {
            gameObject.GetComponent<Rigidbody2D>().gravityScale = 1;
        }

        else
        {
            gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
        }
    }
}
