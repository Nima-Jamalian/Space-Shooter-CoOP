using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    [SerializeField] private GameObject _enemyPrefab = default;
    [SerializeField] private GameObject _enemyContainer = default;
    [SerializeField] private GameObject[] _powerups = default;
    [SerializeField] private float _enemySpawnRateTime = 5f;

    private bool _stopSpawning = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemy());
        StartCoroutine(SpawnPowerup());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator SpawnEnemy(){
        yield return new WaitForSeconds(3.0f);
        while(_stopSpawning == false){
        GameObject _newEnemy = Instantiate(_enemyPrefab, new Vector3(Random.Range(-10.32f,10.32f),7.47f,0),Quaternion.identity);
        _newEnemy.transform.parent = _enemyContainer.transform;
        yield return new WaitForSeconds(_enemySpawnRateTime);
        }
    }

    IEnumerator SpawnPowerup()
    {
        yield return new WaitForSeconds(3.0f);
        while(_stopSpawning == false)
        {
            int randomPowerup = Random.Range(0, 3);
            Instantiate(_powerups[randomPowerup], new Vector3(Random.Range(-10.32f, 10.32f), 7.47f, 0), Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(3, 8));
        }
    }

    public void OnPlayerDeath(){
        _stopSpawning = true;
    }
}
