using UnityEngine;
using System.Collections;

public class EnemyMove : MonoBehaviour {

    public AudioClip hitSound;
    public AudioClip moveSound;
    public AudioClip deathSound;
    public float moveSpeed;
    public float health;
    public float damage;
    public bool dead;
    public string myName;

    private int move;
    private AudioSource audioSource;
	// Use this for initialization
	void Start () {
        audioSource = GetComponent<AudioSource>();
        dead = false;
        move = -1;
	}
	
	// Update is called once per frame
	void Update () {
        float timeFromLastFrame = Time.deltaTime * moveSpeed;
        Vector3 newPosition = new Vector3(move*moveSpeed + transform.position.x, transform.position.y);
        transform.position = Vector3.Lerp(transform.position, newPosition, timeFromLastFrame);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (move == -1 && other.tag == "Boundary")
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
            move = 1;
        } else if (move == 1 && other.tag == "Boundary")
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            move = -1;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Collider2D collider = collision.collider;
        if(collider.tag == "Player")
        {
            CharacterHealth character = collider.GetComponent<CharacterHealth>();
            if(character)
            {
                DoDamage(character);
                Rigidbody2D body = GetComponent<Rigidbody2D>();
                body.AddForce(new Vector2(1, 1));
            }
        }
    }

    public void TakeDamage(float damage)
    {
        audioSource.PlayOneShot(hitSound, 0.5f);
        health -= damage;
        if (health <=0)
        {
            DoDeath();
            dead = true;
            //gameObject.SetActive(false);
        }
    }

    public void DoDeath()
    {
        audioSource.PlayOneShot(deathSound, 0.5f);
    }

    void DoDamage(CharacterHealth character)
    {
        character.TakeDamage(damage);
    }

    public bool IsSoundPlaying()
    {
        return audioSource.isPlaying;
    }

}
