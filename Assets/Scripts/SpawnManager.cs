using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab = null;
    [SerializeField]
    private GameObject _powerUpPrefab = null;

    private IEnumerator coroutine;
    [SerializeField]
    private float spawnDelay = 5.0f;
    [SerializeField]
    private GameObject _enemyContainer = null ;
    private bool _stopSpawning = false;
    [SerializeField]
    private GameObject[] _powerUps;

    public void StartSpawning()
    {
        coroutine = SpawnEnemyRoutine(spawnDelay);
        StartCoroutine(coroutine);
        StartCoroutine(SpawnPowerUpRoutine(3f, 7f));
    }

    private IEnumerator SpawnEnemyRoutine(float waitTime)
    {
        yield return new WaitForSeconds(3.0f);

        while(!_stopSpawning)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7f, 0);
            GameObject newEnemy = Instantiate(_enemyPrefab, posToSpawn, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;

            yield return new WaitForSeconds(waitTime);
        }
    }

    private IEnumerator SpawnPowerUpRoutine(float waitTimeMin, float waitTimeMax)
    {
        yield return new WaitForSeconds(3.0f);

        while (!_stopSpawning)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7f, 0);
            int randomPowerup = Random.Range(0, 3);
            GameObject newPowerUp = Instantiate(_powerUps[randomPowerup], posToSpawn, Quaternion.identity);

            yield return new WaitForSeconds(Random.Range(waitTimeMin, waitTimeMax));
        }
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }
}