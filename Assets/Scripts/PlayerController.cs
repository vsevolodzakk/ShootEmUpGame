using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // Player move Speed
    [SerializeField] private float _speed;

    // Camera reference
    [SerializeField] private Camera _mainCamera;

    // Player Aminator reference
    [SerializeField] private Animator _animator;

    // LayerMask for AimTowardsMousePosition
    [SerializeField] private LayerMask _aimLayerMask;

    // VisualFX for hit reaction
    [SerializeField] private ParticleSystem _hitMarker;

    // Player SoundFX
    [SerializeField] private AudioSource _footSteps;
    [SerializeField] private AudioSource _playerHitSound;
    [SerializeField] private AudioSource _playerDeadSound;
    [SerializeField] private AudioSource _playerHealSound;
    
    // Bool flag of Player running state for Animator
    private bool _isRunning;

    // CharacterController reference
    private CharacterController _player;

    // Scene controller reference
    [SerializeField] private SceneController _sceneController;

    // Movement vector
    private float _h;
    private float _v;
    private Vector3 _movement;

    // Y coordinate for "Jump" bug fix
    private float _startingPositionY;

    // Player Health component reference
    private HealthComponent _playerHealth;

    // Player crossair
    [SerializeField] private GameObject _pointer;

    // Player Death Event
    public delegate void PlayerDeath();
    public static event PlayerDeath OnPlayerDeath;

    // Player Hit event
    public delegate void PlayerHit();
    public static event PlayerHit OnPlayerHit;

    private void OnEnable()
    {
        HealthComponent.OnCharacterDeath += MakePlayerDead;

        HealthPickupObject.OnHealthPickupObjectTaken += PlayHealSound;
    }

    private void Start()
    {
        _player = GetComponent<CharacterController>();
        _playerHealth = GetComponent<HealthComponent>();

        _startingPositionY = transform.position.y;
        _isRunning = false;
    }

    private void OnMove(InputValue input)
    {
        // Input
        _h = input.Get<Vector2>().x;
        _v = input.Get<Vector2>().y;
    }

    private void Update()
    {
        // Movement Vector
        _movement = new Vector3(_h, 0f, _v);

        if (_playerHealth.IsAlive && _sceneController.GameOnPause == false)
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

            // Footstep Audio
            if (_isRunning) 
            {
                if(!_footSteps.isPlaying)
                    _footSteps.Play();
            }    
            else _footSteps.Stop();
        }
    }

    /// <summary>
    /// Death Event
    /// </summary>
    private void MakePlayerDead()
    {
        if (!_playerHealth.IsAlive)
        {
            _animator.SetTrigger("gotDead");
            _playerDeadSound.Play();
            OnPlayerDeath?.Invoke();
        }
    }

    /// <summary>
    /// Rotate character towards crossair direction
    /// </summary>
    private void AimTowardMouse()
    {
        // Look in mouse cursor direction
        Ray ray = _mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

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

    /// <summary>
    /// Hit by enemy check
    /// </summary>
    /// <param name="other">Hitting Collider</param>
    private void OnTriggerEnter(Collider other)
    {
        if (_playerHealth.IsAlive)
        {
            // Hit by Enemy event
            if (other.CompareTag("EnemyBullet"))
            {
                _hitMarker.Play();
                _playerHitSound.Play();
                OnPlayerHit?.Invoke();
            }
        }
    }

    /// <summary>
    /// Plays Heal sound when Health has been restored
    /// </summary>
    private void PlayHealSound()
    {
        _playerHealSound.Play();
    }

    private void OnDisable()
    {
        HealthComponent.OnCharacterDeath -= MakePlayerDead;

        HealthPickupObject.OnHealthPickupObjectTaken -= PlayHealSound;
    }
}
