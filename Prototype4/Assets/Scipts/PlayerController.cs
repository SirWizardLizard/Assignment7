using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    private Rigidbody playerRb;
    public float speed = 5.0f;
    private float forwardInput;

    private GameObject focalPoint;

    public bool hasPowerup;
    private float powerupStrength;

    public GameObject powerupIndicator;

    // Start is called before the first frame update
    void Start() {
        playerRb = GetComponent<Rigidbody>();
        focalPoint = GameObject.FindGameObjectWithTag("FocalPoint");
    }

    // Update is called once per frame
    void Update() {
        forwardInput = Input.GetAxis("Vertical");

        //Move our powerup indicator to the ground below our player
        powerupIndicator.transform.position = transform.position + new Vector3(0, -0.58f, 0);
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

