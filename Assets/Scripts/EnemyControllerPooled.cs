using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyControllerPooled : MonoBehaviour, IGameObjectPooled
{
    // EnemyPool reference
    private ObjectPool _pool;
    public ObjectPool Pool
    {
        get { return _pool; }
        set
        {
            if (_pool == null)
                _pool = value;
            else
                throw new System.Exception("Bad pool use!");
        }
    }

    [SerializeField] private NavMeshAgent _enemy;
    [SerializeField] private Transform _player;
    [SerializeField] private ParticleSystem _ps;
    
    [SerializeField] private AudioSource _takeHitSound;
    [SerializeField] private AudioSource _footstepsSound;
    [SerializeField] private AudioSource _diesSound;
    [SerializeField] private AudioSource _knifeAttackSound;
 
    private float _spawningHp;
    private float _spawningSpeed;
    private Animator _animator;
    private bool _isRunning;

    [SerializeField] private float hp;
    public bool isDead;

    [SerializeField] private int scorePoints;

    // Enemy Death Event
    public delegate void EnemyDeath(int score);
    public static event EnemyDeath OnEnemyDies;

    //public AnimatorClipInfo[] clipInfo; 
    private float _waitTime;

    private void OnEnable()
    {
        isDead = false;
        _isRunning = false;
    }

    private void Start()
    {
        _enemy = GetComponent<NavMeshAgent>();
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _animator = GetComponent<Animator>();

        _spawningHp = hp;
        _spawningSpeed = _enemy.speed;   
    }

    private void Update()
    {
        // Move Enemy to Player position
        _enemy.SetDestination(_player.position);

        // Run Animation
        if (_enemy.speed != 0) 
        {
            _animator.SetBool("isRunning", true);
            _isRunning = true;
        }  
        else 
            _animator.SetBool("isRunning", false);

        // Stop enemy after Player death
        if (_player.gameObject.GetComponent<PlayerController>().isAlive == false)
        {
            _enemy.isStopped = true;
            _isRunning = false;

            // Disable animations
            _animator.SetBool("isRunning", false);
        }

        // Enemy steps sound
        if (_isRunning)
        {
            if (!_footstepsSound.isPlaying)
                _footstepsSound.Play();
        }
        else 
            _footstepsSound.Stop();     
    }
    
    private void OnTriggerEnter(Collider other)
    {
        // If bullet hits Enemy
        if (other.CompareTag("Bullet") && !isDead)
        {
            StopAllCoroutines();
            if(hp == 1)
            {
                // Enemy Death
                isDead = true;
                StartCoroutine(EnemyDies());
                if (OnEnemyDies != null)
                    OnEnemyDies(scorePoints);
                _diesSound.Play();
            }
            else
            {
                // If there is still HP left, hit Enemy
                hp--;
                
                StartCoroutine(EnemyTakeHit());
                _enemy.speed = _spawningSpeed * 0.7f;
                _takeHitSound.Play();
            }

            // Hit visual effect
            _ps.Play();
        }
       
        // Melee Attack of the Enemy
        if(other.CompareTag("Player") && !isDead)
        {
            StartCoroutine(EnemyMeleeAttacks());
        }
    }

    private IEnumerator EnemyTakeHit()
    {
        _enemy.isStopped = true;
        _animator.SetBool("isRunning", false);
        _animator.SetTrigger("gotHit");
        
        Debug.Log("HIT!");
        
        yield return new WaitForSeconds(1f);
        
        _enemy.isStopped = false;
        Debug.Log("Recover!");
    }

    private IEnumerator EnemyDies()
    {
        _enemy.isStopped = true;
        _animator.SetTrigger("gotDead");
        yield return new WaitForSeconds(1.6f);

        hp = _spawningHp;
        _enemy.speed = _spawningSpeed;
        
        Pool.ReturnToPool(this.gameObject);  
    }

    private IEnumerator EnemyMeleeAttacks()
    {
        _enemy.isStopped = true;
        _animator.SetTrigger("onAttack");
        if(_knifeAttackSound.clip != null)
            _knifeAttackSound.Play();
        yield return new WaitForSeconds(1f);
        _enemy.isStopped = false;
    }

    // Метод для рассчета времени проигрывания анимации. 
    // есть проблема в неправильном вычислении состояния
    private float GetClipDuration()
    {
        AnimatorClipInfo[] clipInfo = _animator.GetCurrentAnimatorClipInfo(0);
        //Debug.Log(clipInfo[0].clip.length.ToString());
        //Debug.Log(clipInfo[0].clip.name);
        return clipInfo[0].clip.length;
    }
}
