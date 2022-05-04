using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveSpawner : MonoBehaviour
{
    // Custom Wave class
    [System.Serializable]
    public class Wave
    {
        public string name;
        public ObjectPool enemyPool;
        public int count;
        public float rate;
    }

    // Define Spawn State
    public enum SpawnState
    {
        SPAWNING,
        WAITING,
        COUNTING
    }

    // Waves container
    public Wave[] waves;
    private int _nextWave = 0;

    public float timeBetweenWaves = 5f;
    public float waveCountdown;

    public float searchCoundown = 1f;

    // First state
    private SpawnState _state = SpawnState.COUNTING;

    private Vector3 _spawnPosition;

    public Text waveName;
    public Text nextWaveCountdown;

    [SerializeField] private Transform _playerPosition;

    

    void Start()
    {
        // Set countdown for the next Wave
        waveCountdown = timeBetweenWaves;
    }

    void Update()
    {
         _spawnPosition = GetSpawnPoint(_playerPosition.position, 15f); // Define Spawn Position

        // Check if Enemy still alive
        if (_state == SpawnState.WAITING)
            if (!EnemyIsAlive())
            {
                NextWave(); // Start next Wave
            }
            else return;

        if (waveCountdown <= 0) 
        {
            if (_state != SpawnState.SPAWNING)
                StartCoroutine(SpawnWave(waves[_nextWave]));
        }
        
        else
        {
            waveCountdown -= Time.deltaTime;
            nextWaveCountdown.text = waveCountdown.ToString("#");
            waveName.text = waves[_nextWave].name;
        }
    }

    // Start next Wave
    private void NextWave()
    {
        _state = SpawnState.COUNTING;
        waveCountdown = timeBetweenWaves;

        if (_nextWave + 1 > waves.Length - 1)
            _nextWave = 0;
        else _nextWave++;
    }

    // Check Enemies on the field
    private bool EnemyIsAlive()
    {
        searchCoundown -= Time.deltaTime;

        if (searchCoundown <= 0f)
        {
            searchCoundown = 1f;
            if (GameObject.FindGameObjectWithTag("Enemy") == null)
                return false;
        }
        return true;
    }

    // Wave Spawner
    IEnumerator SpawnWave(Wave wave)
    {
        _state = SpawnState.SPAWNING;

        for(int i = 0; i < wave.count; i++)
        {
            SpawnEnemy(wave.enemyPool);
            yield return new WaitForSeconds(1f / wave.rate);
        }
        _state = SpawnState.WAITING;
        yield return new WaitForSeconds(0.5f);
    }

    // Enemy Spawner
    private void SpawnEnemy(ObjectPool enemy)
    {
        var _newEnemy = enemy.Get(); // Get Enemy from Pool
        _newEnemy.transform.rotation = transform.rotation; // Set Enemy position and rotation
        _newEnemy.transform.position = new Vector3(_spawnPosition.x, 1f, _spawnPosition.z);
        _newEnemy.SetActive(true);
    }

    // Calculate spawn point in radius around Plaayer
    private Vector3 GetSpawnPoint(Vector3 center, float radius)
    {
        float _angle = Random.Range(0, 2f * Mathf.PI);
        return center + new Vector3(Mathf.Cos(_angle), 0, Mathf.Sin(_angle)) * radius;
    }
}
