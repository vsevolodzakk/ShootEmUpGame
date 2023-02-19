﻿using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyControllerPooled : MonoBehaviour, IGameObjectPooled
{
    // Enemy NAvMeshAgent
    [SerializeField] private NavMeshAgent _enemy;

    // Trigger to detect objects in melee atack range
    [SerializeField] private Collider _attackTrigger;

    // Reference to Player location to track
    public Transform playerPosition;

    // VisualFX for hit reaction
    [SerializeField] private ParticleSystem _ps;
    
    // Enemy SoundFX
    [SerializeField] private AudioSource _takeHitSound;
    [SerializeField] private AudioSource _footstepsSound;
    [SerializeField] private AudioSource _diesSound;
    [SerializeField] private AudioSource _knifeAttackSound;

    // Enemy run speed
    private float _spawningSpeed;

    // Enemy Animator
    private Animator _animator;

    // Running state flag
    private bool _isRunning;

    // Enemy Health component
    private HealthComponent _enemyHealth;

    // Amount of score point Enemy costs
    [SerializeField] private int _scorePoints;

    // Enemy Death Event
    public delegate void EnemyDeath(int score);
    public static event EnemyDeath OnEnemyDies;

    //public AnimatorClipInfo[] clipInfo; 
    //private float _waitTime;

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

    private void OnEnable()
    {
        _isRunning = false;

        if(_attackTrigger != null)
            _attackTrigger.enabled = true;
    }

    private void Start()
    {
        _enemy = GetComponent<NavMeshAgent>();

        _animator = GetComponent<Animator>();

        _enemyHealth = GetComponent<HealthComponent>();

        _spawningSpeed = _enemy.speed;
    }

    private void Update()
    {
        if (_enemyHealth.IsAlive)
        {
            // Move Enemy to Player position
            _enemy.SetDestination(playerPosition.position);

            // Run Animation
            if (_enemy.speed != 0)
            {
                _animator.SetBool("isRunning", true);
                _isRunning = true;
            }
            else
            {
                _animator.SetBool("isRunning", false);
            }

            // Stop enemy after Player death
            if (playerPosition.gameObject.GetComponent<HealthComponent>().IsAlive == false)
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
            {
                _footstepsSound.Stop();
            }
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        // If bullet hits Enemy
        if (other.CompareTag("Bullet") & _enemyHealth.IsAlive)
        {
            StopAllCoroutines();

            // Логика получения урона через свойство Health

            if (_enemyHealth.Health <= 1)
            {
                // Enemy Death

                //Debug.Log("DEAD!");
                if(_attackTrigger != null)
                    _attackTrigger.enabled = false;

                StartCoroutine(EnemyDies());
                OnEnemyDies?.Invoke(_scorePoints);
                _diesSound.Play();

            }
            else
            {
                // If there is still HP left, hit Enemy

                StartCoroutine(EnemyTakeHit());
                _enemy.speed = _spawningSpeed * 0.7f;
                _takeHitSound.Play();
            }

            // Hit visual effect
            _ps.Play();
        }
       
        // Melee Attack of the Enemy
        if(other.CompareTag("Player") && _enemyHealth.IsAlive)
        {
            StartCoroutine(EnemyMeleeAttacks());
        }
    }

    //private void MakeEnemyDead()
    //{
    //    Разобраться, почему событие срабатывает 2 раза ?

    //    if (!_enemyHealth.isAlive && _inGame)
    //    {
    //        StopAllCoroutines();

    //        Debug.Log("DEAD!");

    //        _diesSound.Play();
    //        StartCoroutine(EnemyDies());

    //        if (OnEnemyDies != null)
    //            OnEnemyDies(_scorePoints);
    //    }
    //}

    private IEnumerator EnemyTakeHit()
    {
        _enemy.isStopped = true;
        _animator.SetBool("isRunning", false);
        _animator.SetTrigger("gotHit");
        
        //Debug.Log("HIT!");
        
        yield return new WaitForSeconds(1f); // Magic number?

        _enemy.isStopped = false;
        //Debug.Log("Recover!");
    }

    private IEnumerator EnemyDies()
    {
        _enemy.isStopped = true;
        _animator.SetTrigger("gotDead");

        yield return new WaitForSeconds(1.6f); // Magic number?

        _enemy.speed = _spawningSpeed;
        
        Pool.ReturnToPool(this.gameObject);
    }

    private IEnumerator EnemyMeleeAttacks()
    {
        _enemy.isStopped = true;
        _animator.SetTrigger("onAttack");
        if(_knifeAttackSound.clip != null)
            _knifeAttackSound.Play();
        yield return new WaitForSeconds(1f); // Magic number?
        _enemy.isStopped = false;
    }

    // Метод для рассчета времени проигрывания анимации. 
    // есть проблема в неправильном вычислении состояния
    //private float GetClipDuration()
    //{
    //    AnimatorClipInfo[] clipInfo = _animator.GetCurrentAnimatorClipInfo(0);
    //    //Debug.Log(clipInfo[0].clip.length.ToString());
    //    //Debug.Log(clipInfo[0].clip.name);
    //    return clipInfo[0].clip.length;
    //}
}
