using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private Animator _animator;

    [SerializeField] private LayerMask _aimLayerMask;

    [SerializeField] private ParticleSystem _hitMarker;

    [SerializeField] private AudioSource _footSteps;
    [SerializeField] private AudioSource _playerHitSound;
    [SerializeField] private AudioSource _playerDeadSound;
    [SerializeField] private AudioSource _playerHealSound;
    
    private bool _isRunning = false;
    
    public int health;

    private CharacterController _player;
    [SerializeField] private SceneController _sceneController;

    private float _h;
    private float _v;
    private Vector3 _movement;

    private float _startingPositionY;

    public bool isAlive;
    [SerializeField] private GameObject _pointer;
    [SerializeField] private GunController _gun;

    public delegate void PlayerDeath();
    public static event PlayerDeath OnPlayerDeath;

    public delegate void PlayerHit();
    public static event PlayerHit OnPlayerHit;

    private void OnEnable()
    {
        HealthPickupObject.OnHealthPickupObjectTaken += AddHealth;
    }

    private void Start()
    {
        _player = GetComponent<CharacterController>();
        isAlive = true;
        health = 3;

        _startingPositionY = transform.position.y;
    }

    private void Update()
    {
        // Input
        _h = Input.GetAxis("Horizontal");
        _v = Input.GetAxis("Vertical");

        // Movement Vector
        _movement = new Vector3(_h, 0f, _v);

        if (isAlive && _sceneController.gameOnPause == false)
        {
            AimTowardMouse();
            _isRunning = false;

            // Player Move
            if (_movement.magnitude > 0)
            {
                _movement.Normalize();
                _player.Move(_movement * _speed * Time.deltaTime);
                _isRunning = true;
            }
            
            // Animation parameters
            float velocityZ = Vector3.Dot(_movement.normalized, transform.forward);
            float velocityX = Vector3.Dot(_movement.normalized, transform.right);

            // Player Animation
            _animator.SetFloat("VelocityZ", velocityZ, 0.1f, Time.deltaTime);
            _animator.SetFloat("VelocityX", velocityX, 0.1f, Time.deltaTime);

            // "Jump" bug workaround -- персонаж игрока "подпрыгивал" при продолжительной коллизии с препядствием или противником
            //                          было решено проверять положение игрока по вертикали и поправлять если что-то не так
            if (transform.position.y > _startingPositionY)
                transform.position = new Vector3(transform.position.x, _startingPositionY, transform.position.z) ;

            // Death Event
            if (health == 0)
            {
                isAlive = false;
                _playerDeadSound.Play();
                _animator.SetTrigger("gotDead");
                OnPlayerDeath?.Invoke();
            }

            // Footstep Audio
            if (_isRunning) 
            {
                if(!_footSteps.isPlaying)
                    _footSteps.Play();
            }    
            else _footSteps.Stop();
        }  
    }

    private void AimTowardMouse()
    {
        // Look in mouse cursor direction
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit raycastHit, Mathf.Infinity, _aimLayerMask))
        {
            var _direction = raycastHit.point - transform.position;
            _direction.y = 0f;
            _direction.Normalize();
            transform.forward = _direction;
        }

        if (raycastHit.point != _player.transform.position)
            _pointer.transform.position = new Vector3(raycastHit.point.x, 0.7f, raycastHit.point.z);

    }

    private void OnTriggerEnter(Collider other)
    {
        if(isAlive == true)
        {
            // Hit by Enemy event shooting
            if (other.CompareTag("Enemy") && health > 0
                && !other.gameObject.GetComponent<EnemyControllerPooled>().isDead 
                    || other.CompareTag("EnemyBullet"))
            {
                health--;
                _hitMarker.Play();
                _playerHitSound.Play();
                Debug.Log("Player HIT!");
                OnPlayerHit?.Invoke();
            }
        }       
    }

    private void AddHealth()
    {
        if(health < 3)
        {
            Debug.Log("ADD HEALTH");
            health++;
            _playerHealSound.Play();
        }
    }

    private void OnDisable()
    {
        HealthPickupObject.OnHealthPickupObjectTaken -= AddHealth;
    }
}
