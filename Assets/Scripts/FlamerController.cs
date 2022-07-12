using UnityEngine;

public class FlamerController : MonoBehaviour
{
    private Transform _player;
    private HealthComponent _enemyHealth;
    private Ray _ray;

    [SerializeField] private ParticleSystem _firePs;
    [SerializeField] private SceneController _sceneController;
    [SerializeField] private AudioSource _flamerSound;

    void Start()
    {
        _player = GetComponentInParent<EnemyControllerPooled>().playerPosition;
        _enemyHealth = GetComponent<HealthComponent>(); 
    }

    void Update()
    {
        _ray = new Ray(transform.position, _player.position);
        RaycastHit _hit;

        // Flame player
        if (_enemyHealth.isAlive)
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
