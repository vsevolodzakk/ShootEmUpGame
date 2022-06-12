using UnityEngine;

public class FlamerController : MonoBehaviour
{
    private Transform _player;
    private EnemyControllerPooled _enemy;
    [SerializeField] private ParticleSystem _firePs;
    [SerializeField] private SceneController _sceneController;
    [SerializeField] private AudioSource _flamerSound;

    private Ray _ray;

    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _enemy = GetComponent<EnemyControllerPooled>(); 
    }

    void Update()
    {
        _ray = new Ray(transform.position, _player.position);
        RaycastHit _hit;

        // Flame player
        if (_enemy.isDead == false)
        {
            if (Physics.Raycast(_ray, out _hit, Mathf.Infinity, 9))
            {
                _firePs.Play();
                if(!_flamerSound.isPlaying)
                    _flamerSound.Play();
            }
            else
            {
                _firePs.Stop();
            }
        }
        else
        {
            _firePs.Stop();
            _flamerSound.Stop();
        }
    }
}
