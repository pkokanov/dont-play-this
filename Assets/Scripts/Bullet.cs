using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

    public AudioSource bulletSound;
    public float damage;
    public float maxDuration;
	// Use this for initialization
	void Start () {
    }
	
	// Update is called once per frame
	void Update () {
        if(maxDuration <=0)
        {
            Destroy(gameObject);
        } else
        {
            maxDuration -= Time.deltaTime;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if( other.tag != "Player" && other.tag != "Boundary")
        {
            Destroy(gameObject);
            if (other.tag == "Enemy")
            {
                EnemyMove enemy = other.GetComponent<EnemyMove>();
                if (enemy) { 
                    DoDamage(enemy);
                }
            }
        }
    }

    void DoDamage(EnemyMove enemy)
    {
        enemy.TakeDamage(damage);
    }
}
