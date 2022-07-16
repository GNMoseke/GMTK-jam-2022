using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceSpawner : MonoBehaviour
{
    public GameObject dicePrefab;
    public Material[] diceMaterials;

    // How often a die will spawn is dictated by this interval
    [SerializeField]
    private float minSpawnInterval;
    [SerializeField]
    private float maxSpawnInterval;

    private float spawnTimer;

    private void Awake()
    {
        spawnTimer = Random.Range(0, 2f);
    }

    private void Update()
    {
        spawnTimer -= Time.deltaTime;

        if (spawnTimer <= 0)
        {
            // Spawn the die at a random position with a random rotation and material
            GameObject die = GameObject.Instantiate(dicePrefab);
            die.transform.position = transform.position;

            float randomX = Random.Range(0, 360f);
            float randomY = Random.Range(0, 360f);
            float randomZ = Random.Range(0, 360f);

            die.transform.localRotation = Quaternion.Euler(randomX, randomY, randomZ);
            die.GetComponent<Renderer>().material = diceMaterials[Random.Range(0, diceMaterials.Length)];

            spawnTimer = Random.Range(minSpawnInterval, maxSpawnInterval);
        }
    }
}
