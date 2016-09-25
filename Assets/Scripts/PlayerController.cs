using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour {

    public GameObject playerPrefab;
    public Slider playerHealthBar;
    public GameObject[] playerLives;
    public GameObject playerSpawnPoint;
    public float maxRespawnTime;

    public Text gameQuitText;
    public Text gameWonText;
    public Text gameOverText;
    public Button restartButton;
    public Button quitButton;

    private GameObject player;
    private CharacterHealth playerHealth;
    private CharacterMove playerMove;
    private EnemyController enemyController;
    private float currentRespawnTime;
    private bool quitting;

	// Use this for initialization
	void Start () {
        quitting = false;
        enableUiElements(false);
        currentRespawnTime = 0;
        CharacterHealth charHealthController = playerPrefab.GetComponent<CharacterHealth>();
        if(charHealthController)
        {
            charHealthController.healthBar = playerHealthBar;
            charHealthController.lives = playerLives;
        }
        player = Instantiate(playerPrefab, playerSpawnPoint.transform.position, playerSpawnPoint.transform.rotation) as GameObject;
        playerHealth = player.GetComponent<CharacterHealth>();
        playerMove = player.GetComponent<CharacterMove>();
        enemyController = GetComponent<EnemyController>();
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape) && quitting == false)
        {
            quitting = true;
            DoQuit();
            return;
        }
        if (Input.GetKeyDown(KeyCode.Escape) && quitting == true)
        {
            quitting = false;
            DoResume();
            return;
        }
        if (playerMove.goalReached)
        {
            if(enemyController.currentEnemies == 0)
                DoGameWon();
            else
            {
                playerMove.goalReached = false;
            }
            return;
        }
        if (playerHealth.realDead)
        {
            DoGameOver();
            return;
        }
	    if (playerHealth.dead)
        {
            if(currentRespawnTime <= 0)
            {
                currentRespawnTime = maxRespawnTime;
            }
            currentRespawnTime -= Time.deltaTime;
            if (currentRespawnTime <= 0)
            {
                RespawnPlayer();
                currentRespawnTime = 0;
            }
        }
	}

    void RespawnPlayer()
    {
        enemyController.RecreateAllEnemies();
        player.transform.position = playerSpawnPoint.transform.position;
        player.transform.rotation = playerSpawnPoint.transform.rotation;
        playerHealth.dead = false;
        player.SetActive(true);
    }

    void DoGameOver()
    {
        gameOverText.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true);
        quitButton.gameObject.SetActive(true);
    }

    void DoGameWon()
    {
        gameWonText.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true);
        quitButton.gameObject.SetActive(true);
        player.SetActive(false);
    }

    void enableUiElements(bool enable)
    {
        gameQuitText.gameObject.SetActive(enable);
        gameWonText.gameObject.SetActive(enable);
        gameOverText.gameObject.SetActive(enable);
        restartButton.gameObject.SetActive(enable);
        quitButton.gameObject.SetActive(enable);
    }

    public void OnRestartClick()
    {
        playerHealthBar.value = playerHealthBar.maxValue;
        foreach (Object life in playerLives)
        {
            (life as GameObject).SetActive(true);
        }
        playerHealth.lives = playerLives;
        playerHealth.RefillLives();
        RespawnPlayer();
        playerHealth.realDead = false;
        enemyController.RecreateAllEnemies();
        enableUiElements(false);
    }

    public void OnQuitClick()
    {
        Application.Quit();
    }

    void DoQuit()
    {
        player.SetActive(false);
        gameQuitText.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true);
        quitButton.gameObject.SetActive(true);
    }

    public void DoResume()
    {
        player.SetActive(true);
        enableUiElements(false);
    }
}
