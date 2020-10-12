using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float gravity = 20.0f;
    public float jumpHeight = 2.5f;

    public AudioSource jumpSound;

    private Vector2 startTouchPosition;
    private Vector2 endTouchPosition;
    private Rigidbody r;
    private bool grounded = false;

    void Start()
    {
        r = GetComponent<Rigidbody>();
    }

    void Update()
    {
        //Checking if game is still active
        if(!PlatformGenerator.instance.gameOver)
        {
            //Geting position of the first touch
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                startTouchPosition = Input.GetTouch(0).position;
            }

            //Geting position of the final touch
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                endTouchPosition = Input.GetTouch(0).position;

                //Jump
                if ((endTouchPosition.y > startTouchPosition.y) && grounded)
                {
                    r.velocity = new Vector3(r.velocity.x, Mathf.Sqrt(2 * jumpHeight * gravity), r.velocity.z);
                    jumpSound.Play();
                    return;
                }

                //Move left
                if ((endTouchPosition.x < startTouchPosition.x) && transform.position.x > -2f)
                {
                    transform.position = new Vector3(transform.position.x - 2f, transform.position.y, transform.position.z);
                }

                //Move right
                if ((endTouchPosition.x > startTouchPosition.x) && transform.position.x < 2f)
                {
                    transform.position = new Vector3(transform.position.x + 2f, transform.position.y, transform.position.z);
                }
            }
        }      
    }

    void FixedUpdate()
    {
        //Force used to stop the jump and ground again
        r.AddForce(new Vector3(0, -gravity * r.mass, 0));

        grounded = false;
    }

    void OnCollisionStay() //Active when player doesn't jump
    {
        grounded = true;
    }

    void OnCollisionEnter(Collision collision) 
    {
        if (collision.gameObject.tag == "Finish") //Player collided with obstacle
        {
            Time.timeScale = 0;
            PlatformGenerator.instance.gameOverUI.enabled = true;
            PlatformGenerator.instance.gameOver = true;
        }
    }
}