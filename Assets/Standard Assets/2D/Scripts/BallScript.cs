using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallScript : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
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
