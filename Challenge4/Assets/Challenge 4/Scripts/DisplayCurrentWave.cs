/*
 * Zechariah Burrus
 * Assignment 7
 * Displays the current wave on the screen by updating text
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DisplayCurrentWave : MonoBehaviour {
    public Text textbox;
    SpawnManagerX spawnManager;

    // Start is called before the first frame update
    void Start() {
        spawnManager = GameObject.FindGameObjectWithTag("SpawnManager").GetComponent<SpawnManagerX>();
        textbox.text = "Current wave: ";
    }

    // Update is called once per frame
    void Update() {
        textbox.text = "Current wave: " + (spawnManager.waveCount - 1);
    }
}
