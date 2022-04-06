using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {
    private Rigidbody playerRb;
    public float speed = 5.0f;
    private float forwardInput;

    private GameObject focalPoint;

    public bool hasPowerup;
    private float powerupStrength;

    public GameObject powerupIndicator;

    public bool gameOver = false;
    public bool tutorial = true;

    public GameObject gameOverText;
    public GameObject winText;
    public GameObject tutorialText;

    Scene currentScene;

    SpawnManager spawnManager;

    // Start is called before the first frame update
    void Start() {
        playerRb = GetComponent<Rigidbody>();
        focalPoint = GameObject.FindGameObjectWithTag("FocalPoint");

        currentScene = SceneManager.GetActiveScene();
        spawnManager = GameObject.FindGameObjectWithTag("SpawnManager").GetComponent<SpawnManager>();
    }

    // Update is called once per frame
    void Update() {

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

        forwardInput = Input.GetAxis("Vertical");

        //Move our powerup indicator to the ground below our player
        powerupIndicator.transform.position = transform.position + new Vector3(0, -0.58f, 0);

        //If the player falls off of the platform the game is over
        if (transform.position.y < -5 && !gameOver) {
            gameOver = true;
            gameOverText.SetActive(true);
            Time.timeScale = 0f;
        }

        //Press R to restart the game
        if (Input.GetKeyDown(KeyCode.R) && gameOver) {
            gameOverText.SetActive(false);
            SceneManager.LoadScene(currentScene.name);
            Time.timeScale = 1f;
        }

        //If the player passes 10 waves they win and the game is over.
        if (spawnManager.waveNumber > 10) {
            gameOver = true;
            winText.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    private void FixedUpdate() {
        playerRb.AddForce(focalPoint.transform.forward * speed * forwardInput);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Powerup")) {
            hasPowerup = true;
            Destroy(other.gameObject);
            StartCoroutine(PowerupCountdownRoutine());
            powerupIndicator.gameObject.SetActive(true);
        }
    }

    IEnumerator PowerupCountdownRoutine() {
        yield return new WaitForSeconds(7);
        hasPowerup = false;
        powerupIndicator.gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision) {
        if(collision.gameObject.CompareTag("Enemy") && hasPowerup) {
            Debug.Log("Player collided with " + collision.gameObject.name + 
                " with powerup set to " + hasPowerup);

            //Enemy RB reference
            Rigidbody enemyRb = collision.gameObject.GetComponent<Rigidbody>();

            //Direction away from the player
            Vector3 awayFromPlayer = (collision.gameObject.transform.position - transform.position).normalized;

            //Add force away from player
            enemyRb.AddForce(awayFromPlayer * powerupStrength, ForceMode.Impulse);
        }
    }
}

