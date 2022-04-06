using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour {
    public GameObject enemyPrefab;
    public GameObject powerupPrefab;
    private float spawnRange = 9;

    public int enemyCount;
    public int waveNumber = 1;

    // Start is called before the first frame update
    void Start() {
        SpawnEnemyWave(waveNumber);
        SpawnPowerup(1);
    }

    private void SpawnEnemyWave(int enemiesToSpawn) {
        for (int i = 0; i < enemiesToSpawn; i++) {
            //Instantiate the enemy in the random position
            Instantiate(enemyPrefab, GenerateRandomSpawnPosition(), enemyPrefab.transform.rotation);
        }
    }

    private void SpawnPowerup(int powerupsToSpawn) {
        for (int i = 0; i < powerupsToSpawn; i++) {
            //Instantiate the powerup in the random position
            Instantiate(powerupPrefab, GenerateRandomSpawnPosition(), powerupPrefab.transform.rotation);
        }
    }

    private Vector3 GenerateRandomSpawnPosition() {
        //Generate a random position on the platform
        float spawnPosX = Random.Range(-spawnRange, spawnRange);
        float spawnPosZ = Random.Range(-spawnRange, spawnRange);
        Vector3 randomPos = new Vector3(spawnPosX, 0, spawnPosZ);
        return randomPos;
    }

    void Update() {
        enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
        if(enemyCount == 0) {
            waveNumber++;
            SpawnEnemyWave(waveNumber);
            SpawnPowerup(1);
        }
    }
}
