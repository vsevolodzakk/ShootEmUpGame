using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class NewWaveSpawner : MonoBehaviour
{
    [System.Serializable]
    public class NewWave
    {
        public string name;
        //public Dictionary<ObjectPool, int> enemyWave; // Реализация состава волн через словарь - не появляется в инспекторе
        public ObjectPool[] enemyPools; // Реализация состава волны черех массивы типа врагов и количество
        public int[] count;
        public float rate;
    }

    private enum SpawnState
    {
        SPAWNING,
        WAITING,
        COUNTING
    }

    public NewWave[] newWaves;
    private int _nextWave = 0;

    private float _timeBetweenNewWaves = 6f;
    private float _newWaveCountdown;

    private float _newSearchCountdown = 2f;


    private SpawnState _state = SpawnState.COUNTING;

    private Vector3 _spawnPosition;

    [SerializeField] private Transform _playerPosition;

    [SerializeField] private Text _waveName;
    [SerializeField] private Text _waveCountdown;

    void Start()
    {
        _newWaveCountdown = _timeBetweenNewWaves;
    }

    void Update()
    {
        _spawnPosition = GetSpawnPoint(_playerPosition.position, 15f);

        if (_state == SpawnState.WAITING)
            if (!EnemyIsAlive())
            {
                NextWave();
            }
            else return;

        if (_newWaveCountdown <= 0)
        {
            if (_state != SpawnState.SPAWNING)
            {
                StartCoroutine(SpawnNewWave(newWaves[_nextWave]));
            }
        }
        else
        {
            _newWaveCountdown -= Time.deltaTime;

            _waveCountdown.text = _newWaveCountdown.ToString("#");
            _waveName.text = newWaves[_nextWave].name;
        }

    }

    private Vector3 GetSpawnPoint(Vector3 center, float radius)
    {
        float _angle = Random.Range(0, 2f * Mathf.PI);
        return center + new Vector3(Mathf.Cos(_angle), 0, Mathf.Sin(_angle)) * radius;
    }

    private bool EnemyIsAlive()
    {
        _newSearchCountdown -= Time.deltaTime;

        if (_newSearchCountdown <= 0f)
        {
            _newSearchCountdown = 2f;
            if (GameObject.FindGameObjectWithTag("Enemy") == null)
                return false;
        }
        return true;
    }

    private void NextWave()
    {
        _state = SpawnState.COUNTING;
        _newWaveCountdown = _timeBetweenNewWaves;

        if (_nextWave + 1 > newWaves.Length - 1)
            _nextWave = 0;
        else _nextWave++;
    }

    IEnumerator SpawnNewWave(NewWave wave)
    {
        _state = SpawnState.SPAWNING;
        for (int i = 0; i < wave.enemyPools.Length; i++)
        {
            StartCoroutine(SpawnEnemy(wave.enemyPools[i], wave.count[i], wave.rate));
            yield return new WaitForSeconds(1f); // Добавить частоту rate
        }
        _state = SpawnState.WAITING;


        // Не оставляет мысль реализовать хранение волны противника в словаре
        #region
        //foreach (KeyValuePair<ObjectPool, int> obj in wave.enemyWave)
        //{
        //    StartCoroutine(SpawnEnemy(obj.Key, obj.Value));
        //    yield return new WaitForSeconds(1f);
        //}
        #endregion

        yield return new WaitForSeconds(0.5f);
    }

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
