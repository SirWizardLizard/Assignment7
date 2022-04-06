using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DisplayCurrentWave : MonoBehaviour {
    public Text textbox;
    SpawnManager spawnManager;

    // Start is called before the first frame update
    void Start() {
        spawnManager = GameObject.FindGameObjectWithTag("SpawnManager").GetComponent<SpawnManager>();
        textbox.text = "Current wave: " ;
    }

    // Update is called once per frame
    void Update() {
        textbox.text = "Current wave: " + spawnManager.waveNumber;
    }
}
