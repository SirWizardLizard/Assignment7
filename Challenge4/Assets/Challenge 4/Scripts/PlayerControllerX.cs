using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerControllerX : MonoBehaviour
{
    private Rigidbody playerRb;
    public float baseSpeed = 500f;
    private float currentSpeed;
    private float boostSpeed = 1000f;
    private GameObject focalPoint;

    public bool hasPowerup;
    public GameObject powerupIndicator;
    public int powerUpDuration = 5;

    private float normalStrength = 10; // how hard to hit enemy without powerup
    private float powerupStrength = 25; // how hard to hit enemy with powerup

    public GameObject speedBoost;

    public bool gameOver = false;
    public bool tutorial = true;

    public GameObject gameOverText;
    public GameObject winText;
    public GameObject tutorialText;

    Scene currentScene;

    SpawnManagerX spawnManager;

    void Start()
    {
        currentSpeed = baseSpeed;
        playerRb = GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("Focal Point");

        currentScene = SceneManager.GetActiveScene();
        spawnManager = GameObject.FindGameObjectWithTag("SpawnManager").GetComponent<SpawnManagerX>();
    }

    void Update()
    {
        //Forces the player to press space before playing
        if (tutorial) {
            Time.timeScale = 0f;
            tutorialText.SetActive(true);
            if (Input.GetKeyDown(KeyCode.Space)) {
                tutorial = false;
                Time.timeScale = 1f;
                tutorialText.SetActive(false);
            }
        }

        // Add force to player in direction of the focal point (and camera)
        float verticalInput = Input.GetAxis("Vertical");
        playerRb.AddForce(focalPoint.transform.forward * verticalInput * currentSpeed * Time.deltaTime); 

        // Set powerup indicator position to beneath player
        powerupIndicator.transform.position = transform.position + new Vector3(0, -0.6f, 0);

        if (Input.GetKeyDown(KeyCode.Space)) {
            speedBoost.SetActive(true);
            currentSpeed = boostSpeed;
        }

        if (Input.GetKeyUp(KeyCode.Space)) {
            speedBoost.SetActive(false);
            currentSpeed = baseSpeed;
        }

        //Press R to restart the game
        if (Input.GetKeyDown(KeyCode.R) && gameOver) {
            gameOverText.SetActive(false);
            SceneManager.LoadScene(currentScene.name);
            Time.timeScale = 1f;
        }

        //If the player passes 10 waves they win and the game is over.
        if (spawnManager.waveCount > 10) {
            gameOver = true;
            winText.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    // If Player collides with powerup, activate powerup
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Powerup"))
        {
            Destroy(other.gameObject);
            hasPowerup = true;
            powerupIndicator.SetActive(true);
            StartCoroutine(PowerupCooldown());
        }
    }

    // Coroutine to count down powerup duration
    IEnumerator PowerupCooldown()
    {
        yield return new WaitForSeconds(powerUpDuration);
        hasPowerup = false;
        powerupIndicator.SetActive(false);
    }

    // If Player collides with enemy
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Rigidbody enemyRigidbody = other.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = other.gameObject.transform.position - transform.position; 
           
            if (hasPowerup) // if have powerup hit enemy with powerup force
            {
                enemyRigidbody.AddForce(awayFromPlayer * powerupStrength, ForceMode.Impulse);
            }
            else // if no powerup, hit enemy with normal strength 
            {
                enemyRigidbody.AddForce(awayFromPlayer * normalStrength, ForceMode.Impulse);
            }


        }
    }



}
