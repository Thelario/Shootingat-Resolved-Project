using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemySpawnner : MonoBehaviour
{
    [SerializeField] private List<GameObject> enemies;
    [SerializeField] private Camera mainCam;
    [SerializeField] private float secondsBetweenEnemy;
    //private float secondsBetweenEnemyCounter = 0f;

    [Header("Waves")]
    [SerializeField] private float timeBetweenWaves;
    private float timeBetweenWavesCounter;
    [SerializeField] private int maxWaves;
    private int currentWave;
    [SerializeField] private TMP_Text wavesText;

    private void Start()
    {
        timeBetweenWavesCounter = timeBetweenWaves;
        currentWave = 1;
        wavesText.text = currentWave + " / " + maxWaves;

        StartCoroutine(StartWave());
    }

    /*
    private void Update()
    {
        secondsBetweenEnemyCounter += Time.deltaTime;
        if (secondsBetweenEnemyCounter - secondsBetweenEnemy > Mathf.Epsilon)
        {
            SpawnEnemy();
            secondsBetweenEnemyCounter = 0f;
        }
    }
    */

    private IEnumerator StartWave()
    {
        while (currentWave <= maxWaves)
        {
            yield return SpawnEnemies(currentWave, currentWave * (currentWave/2f) * 0.1f);
            yield return new WaitForSeconds(timeBetweenWaves * currentWave/10f);
            UpdateWave();
        }
    }

    private IEnumerator SpawnEnemies(int numOfEnemies, float timeBetweenEachEnemy)
    {
        for (int i = 0; i < numOfEnemies*2; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(timeBetweenEachEnemy);
        }
    }

    private void UpdateWave()
    {
        currentWave++;
        wavesText.text = currentWave + " / " + maxWaves;
    }

    /// <summary>
    /// Spawns a random enemy in a random position
    /// </summary>
    private void SpawnEnemy()
    {
        Vector3 pos = GetRandomPosition();
        GameObject go = GetRandomEnemy();

        Instantiate(go, pos, Quaternion.identity);
    }


    /// <summary>
    /// Calculates a random position outside the camera bounds
    /// </summary>
    /// <returns> The random position calculated outside the camera bounds </returns>
    private Vector3 GetRandomPosition()
    {
        int place = Random.Range(0, 4);

        float xPos;
        float yPos;

        switch(place)
        {
            case 0: // Left
                xPos = mainCam.transform.position.x - mainCam.orthographicSize * mainCam.aspect - 2f;
                yPos = Random.Range(mainCam.transform.position.y - mainCam.orthographicSize - 2f, mainCam.transform.position.y + mainCam.orthographicSize + 2f);
                break;
            case 1: // Right
                xPos = mainCam.transform.position.x + mainCam.orthographicSize * mainCam.aspect + 2f;
                yPos = Random.Range(mainCam.transform.position.y - mainCam.orthographicSize - 2f, mainCam.transform.position.y + mainCam.orthographicSize + 2f);
                break;
            case 2: // Up
                xPos = Random.Range(mainCam.transform.position.x - mainCam.orthographicSize * mainCam.aspect - 2f, mainCam.transform.position.x + mainCam.orthographicSize * mainCam.aspect + 2f);
                yPos = mainCam.transform.position.y + mainCam.orthographicSize + 2f;
                break;
            case 3: // Down
                xPos = Random.Range(mainCam.transform.position.x - mainCam.orthographicSize * mainCam.aspect - 2f, mainCam.transform.position.x + mainCam.orthographicSize * mainCam.aspect + 2f);
                yPos = mainCam.transform.position.y - mainCam.orthographicSize - 2f;
                break;
            default:
                xPos = 5f;
                yPos = 5f;
                break;
        }

        return new Vector3(xPos, yPos);
    }


    /// <summary>
    /// Gets a random enemy from the enemies arrray
    /// </summary>
    /// <returns> Random enemy prefab to be instantiated </returns>
    private GameObject GetRandomEnemy()
    {
        int r = Random.Range(0, enemies.Count);
        return enemies[r];
    }
}
