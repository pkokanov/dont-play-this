using UnityEngine;
using System.Collections;

public class Shoot : MonoBehaviour
{

    public Rigidbody2D bullet;
    public float bulletSpeed;
    public GameObject marker;
    public float rotationAngle;
    public float shootCooldown;

    private Vector3 shootDirection;
    private float markerDistance;
    private bool shot;
    private float cooldownTimer;
    private bool rotated;
    // Use this for initialization
    void Start()
    {
        Vector3 markerVector = marker.transform.position - transform.position;
        markerDistance = markerVector.magnitude;
        shot = false;
        rotated = false;
        cooldownTimer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePosGameWorld = getMousePosGameWorld();
        shootDirection = Vector3.ClampMagnitude(mousePosGameWorld - transform.position, markerDistance);
        marker.transform.position = transform.position + shootDirection;

        if (marker.transform.position.x < transform.position.x && !rotated)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
            rotated = true;
        } else if (marker.transform.position.x >= transform.position.x && rotated)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            rotated = false;
        }


        if (cooldownTimer >=0 )
        {
            cooldownTimer -= Time.deltaTime;
        }

        if (Input.GetButton("Fire1") && cooldownTimer <= 0)
        {
            shot = true;
            cooldownTimer = shootCooldown;
        }
    }

    void FixedUpdate()
    {
        if (shot)
        {
            Vector3 muzzle = transform.position + Vector3.ClampMagnitude(shootDirection, shootDirection.magnitude/4);
            Rigidbody2D body = Instantiate(bullet, muzzle, transform.rotation) as Rigidbody2D;
            body.AddForce(shootDirection * bulletSpeed, ForceMode2D.Force);
            shot = false;
        }
    }


    Vector3 getMousePosGameWorld()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return new Vector3(mousePos.x, mousePos.y, 0);
    }

}
