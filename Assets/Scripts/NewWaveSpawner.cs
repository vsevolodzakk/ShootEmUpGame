using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class NewWaveSpawner : MonoBehaviour
{
    [System.Serializable]
    /// Enemy wave class
    public class NewWave
    {
        public string name; // Wave name
        public ObjectPool[] enemyPools; // Wave by enemy type
        public int[] count; // Numner of enemy types to spawn
        public float rate; // Rate of spawn
    }

    // Spawn states
    private enum SpawnState
    {
        SPAWNING,
        WAITING,
        COUNTING
    }

    // Event of new wave countdown
    public delegate void NewWaveCountdownStarted();
    public static event NewWaveCountdownStarted onCountdownStart;

    // Wave container
    public NewWave[] newWaves;
    private int _nextWave = 0;
    private float _timeBetweenNewWaves = 4f;
    private float _newWaveCountdown;

    // Countdown for new search of active enemies
    private float _newSearchCountdown = 2f;

    private SpawnState _state = SpawnState.COUNTING;

    private Vector3 _spawnPosition;

    [SerializeField] private Transform _playerPosition;

    [SerializeField] private Text _waveCountdown;

    [SerializeField] private GameObject _waveClearMessage;

    void Start()
    {
        _newWaveCountdown = _timeBetweenNewWaves;
    }

    void Update()
    {
        // Set enemy spawn position
        _spawnPosition = GetSpawnPoint(_playerPosition.position, 15f);
        

        if (_state == SpawnState.WAITING)
            if (!EnemyIsAlive())
            {
                // If there is no active enemies left start new wave
                NextWave();
            }
            else return;

        if (_newWaveCountdown <= 0)
        {
            if (_state != SpawnState.SPAWNING)
            {
                // Start wave spawning process
                StartCoroutine(SpawnNewWave(newWaves[_nextWave]));
            }

            // Some UI info - need to be moved in separate logic
            _waveCountdown.text = " ";
            _waveClearMessage.SetActive(false);
        }
        else
        {
            // Countdown for a new wave
            _newWaveCountdown -= Time.deltaTime;

            _waveCountdown.text = "New wave in " + _newWaveCountdown.ToString("#");
        }
    }

    /// <summary>
    /// Calculate spawn point of the enemy in some area away from Player
    /// </summary>
    /// <param name="center">Center of restricted area</param>
    /// <param name="radius">Radius of restricted area</param>
    /// <returns></returns>
    private Vector3 GetSpawnPoint(Vector3 center, float radius)
    {
        float _angle = Random.Range(0, 2f * Mathf.PI);
        return center + new Vector3(Mathf.Cos(_angle), 0, Mathf.Sin(_angle)) * radius;
    }

    // Check is active enemy left
    private bool EnemyIsAlive()
    {
        _newSearchCountdown -= Time.deltaTime;

        if (_newSearchCountdown <= 0f)
        {
            _newSearchCountdown = 2f;
            if (GameObject.FindGameObjectWithTag("Enemy") == null)
            {
                _waveClearMessage.SetActive(true); // UI info
                return false;
            }   
        }

        return true;
    }

    /// <summary>
    /// Activator of the next wave
    /// </summary>
    private void NextWave()
    {
        _state = SpawnState.COUNTING;
        _newWaveCountdown = _timeBetweenNewWaves;

        onCountdownStart?.Invoke();

        if (_nextWave + 1 > newWaves.Length - 1)
            _nextWave = newWaves.Length - 1;
        else _nextWave++;
    }

    /// <summary>
    /// Wave spawner
    /// </summary>
    /// <param name="wave">Wave to spawn</param>
    /// <returns></returns>
    IEnumerator SpawnNewWave(NewWave wave)
    {
        _state = SpawnState.SPAWNING;
        for (int i = 0; i < wave.enemyPools.Length; i++)
        {
            StartCoroutine(SpawnEnemy(wave.enemyPools[i], wave.count[i], wave.rate));
            yield return new WaitForSeconds(wave.rate);
        }
        _state = SpawnState.WAITING;

        yield return new WaitForSeconds(0.5f);
    }

    /// <summary>
    /// Enemy spawner
    /// </summary>
    /// <param name="enemy">Pool of enemies</param>
    /// <param name="count">Count of enemy type</param>
    /// <param name="rate">Spawn rate of the enemy</param>
    /// <returns></returns>
    IEnumerator SpawnEnemy(ObjectPool enemy, int count, float rate)
    {
        for(int i = 0; i < count; i++)
        {
            var _newEnemy = enemy.Get(); // Get Enemy from Pool
            _newEnemy.transform.rotation = transform.rotation; // Set Enemy position and rotation
            _newEnemy.transform.position = new Vector3(_spawnPosition.x, 1f, _spawnPosition.z);
            _newEnemy.SetActive(true);
            yield return new WaitForSeconds(rate);
        }
    }
}
