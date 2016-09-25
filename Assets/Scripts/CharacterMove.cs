using UnityEngine;
using System.Collections;

public class CharacterMove : MonoBehaviour {

    public float moveSpeed;
    public float jumpForce;
    public bool goalReached;

    private Rigidbody2D body;
    private bool jumping;
    private bool jumped;

	void Start () {
        body = GetComponent<Rigidbody2D>();
        goalReached = false;
        jumping = false;
        jumped = false;
    }

	void Update () {
        int move = 0;

	    if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            move++;
        }

        if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            move--;
        }

        float timeFromLastFrame = Time.deltaTime * moveSpeed;
        Vector3 newPosition = new Vector3(move* moveSpeed + transform.position.x, transform.position.y);
        transform.position = Vector3.Lerp(transform.position, newPosition, timeFromLastFrame);
        Vector3 newCameraPosition = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z);
        newCameraPosition.Set(transform.position.x, transform.position.y, Camera.main.transform.position.z);
        if (newCameraPosition.x < 0.6f)
        {
            newCameraPosition.Set(0.6f, newCameraPosition.y, newCameraPosition.z);
        }
        if(newCameraPosition.x > 3.5f)
        {
            newCameraPosition.Set(3.5f, newCameraPosition.y, newCameraPosition.z);
        }
        if (newCameraPosition.y < -0.1f)
        {
            newCameraPosition.Set(newCameraPosition.x, -0.1f, newCameraPosition.z);
        }
        if (newCameraPosition.y > 1.5f)
        {
            newCameraPosition.Set(newCameraPosition.x, 1.5f, newCameraPosition.z);
        }
        if ((newCameraPosition - Camera.main.transform.position).magnitude != 0)
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, newCameraPosition, timeFromLastFrame*2);
        transform.position = Vector3.Lerp(transform.position, newPosition, timeFromLastFrame);
        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space)) && !jumping)
        {
            jumped = true;
        }


    }

    void FixedUpdate()
    {
        if(jumped)
        {
            body.AddForce(transform.up*jumpForce, ForceMode2D.Impulse);
            jumping = true;
            jumped = false;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        jumping = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Goal")
        {
            goalReached = true;
        }
    }

}
