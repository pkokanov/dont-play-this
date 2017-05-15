using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour {

    public GameObject playerPrefab;
    public Slider playerHealthBar;
    public GameObject[] playerLives;
    public GameObject playerSpawnPoint;
    public float maxRespawnTime;
    public AudioClip gameOverClip;
    public AudioClip deathClip;
    public AudioSource gameOverSource;
    public AudioSource deathSource;

    public Text gameQuitText;
    public Text gameWonText;
    public Text gameOverText;
    public Button restartButton;
    public Button quitButton;

    private GameObject player;
    private CharacterHealth playerHealth;
    private CharacterMove playerMove;
    private EnemyController enemyController;
    private Timer timerController;
    private bool quitting;
    private bool winning;
    private bool deathStarted;
    private bool handlingRealDeath;

	// Use this for initialization
	void Start () {
        quitting = false;
        winning = false;
        deathStarted = false;
        handlingRealDeath = false;
        enableUiElements(false);
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
        timerController = GetComponent<Timer>();
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape) && quitting == false && winning == false)
        {
            quitting = true;
            DoQuit();
            return;
        }
        if (Input.GetKeyDown(KeyCode.Escape) && quitting == true && winning == false)
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
        if (playerHealth.realDead && !handlingRealDeath)
        {
            handlingRealDeath = true;
            gameOverSource.PlayOneShot(gameOverClip);
            player.SetActive(false);
            DoGameOver();
            return;
        }
	    if (playerHealth.dead && !deathStarted && !handlingRealDeath)
        {
            deathStarted = true;
            StartCoroutine("HandleDeathCoroutine");
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

    void DoGameOver  ()
    {
        gameOverText.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true);
        quitButton.gameObject.SetActive(true);
        timerController.StopTimer();
    }

    void DoGameWon()
    {
        winning = true;
        timerController.StopTimer();
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
        playerMove.enabled = true;
        playerHealth.realDead = false;
        handlingRealDeath = false;
        deathStarted = false;
        quitting = false;
        winning = false;
        enemyController.RecreateAllEnemies();
        enableUiElements(false);
        timerController.ResetTimer();
    }

    public void OnQuitClick()
    {
        Application.Quit();
    }

    void DoQuit()
    {
        timerController.StopTimer();
        player.SetActive(false);
        gameQuitText.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true);
        quitButton.gameObject.SetActive(true);
    }

    public void DoResume()
    {
        player.SetActive(true);
        enableUiElements(false);
        timerController.ResumeTimer();
    }

    IEnumerator HandleDeathCoroutine()
    {
        playerMove.enabled = false;
        timerController.StopTimer();
        player.GetComponent<Renderer>().enabled = false;
        player.GetComponent<Shoot>().enabled = false;
        playerHealth.audioSource.PlayOneShot(deathClip);
        while (playerHealth.audioSource.isPlaying)
        {
            yield return null;
        }
        player.SetActive(false);
        yield return new WaitForSeconds(1f);
        RespawnPlayer();
        playerMove.enabled = true;
        player.GetComponent<Renderer>().enabled = true;
        player.GetComponent<Shoot>().enabled = true;
        timerController.ResumeTimer();
        deathStarted = false;
    }
}
