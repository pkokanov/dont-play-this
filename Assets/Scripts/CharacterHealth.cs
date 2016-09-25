using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CharacterHealth : MonoBehaviour {

    public float health;
    public float respawnTime;
    public Slider healthBar;
    public GameObject[] lives;
    public bool dead;
    public bool realDead;
    private Stack livesStack;
    // Use this for initialization
    void Awake ()
    {
        livesStack = new Stack();
    }

    void Start () {
        dead = false;
        realDead = false;       
        foreach (GameObject life in lives) {
            livesStack.Push(life);
        }
	}
	
	// Update is called once per frame
	void Update () {
	}

    public void TakeDamage(float damage)
    {
        healthBar.value -= damage;
        if (healthBar.value <= 0)
        {
            GameObject  lifeToDestroy = livesStack.Pop() as GameObject;
            if (lifeToDestroy)
            {
                lifeToDestroy.SetActive(false);
                dead = true;
                gameObject.SetActive(false);
            }

            if (livesStack.Count == 0)
            {
                realDead = true;
                gameObject.SetActive(false);   
                return;
            }
            healthBar.value = health;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Death")
        {
            TakeDamage(healthBar.value);
        }
    }

    public void RefillLives()
    {
        foreach (GameObject life in lives)
        {
            livesStack.Push(life);
        }
    }

}
