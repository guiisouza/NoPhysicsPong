using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPaddleManager : MonoBehaviour
{
    [SerializeField] GameObject ball;
    BallManager ballManager;
    public float aISpeed;
    public float verticalLimit; //6.5f
    public float resetPos; //8f

    public bool pause;
    public bool gameOver;

    void Start()
    {
        ball = GameObject.FindGameObjectWithTag("Ball");
        ballManager = ball.GetComponent<BallManager>();
    }

    void Update()
    {
        if (!pause && !gameOver)
        {
            if (ballManager.direction.x > 0f)
            {
                if (transform.position.y > (verticalLimit * -1) &&
                    ball.transform.position.y < transform.position.y)
                {
                    transform.position += new Vector3(0, -aISpeed * Time.deltaTime, 0);
                }

                if (transform.position.y < verticalLimit &&
                    ball.transform.position.y > transform.position.y)
                {
                    transform.position += new Vector3(0, aISpeed * Time.deltaTime, 0);
                }

                if (transform.position.y < verticalLimit &&
                    ball.transform.position.y > transform.position.y)
                {
                    transform.position += new Vector3(0, aISpeed * Time.deltaTime, 0);
                }

                if (transform.position.y > (verticalLimit * -1) &&
                    ball.transform.position.y < transform.position.y)
                {
                    transform.position += new Vector3(0, -aISpeed * Time.deltaTime, 0);
                }
            }
        }
    }

    public void ResetPaddle()
    {
        transform.position = new Vector3(resetPos, 0, 0);
    }
}