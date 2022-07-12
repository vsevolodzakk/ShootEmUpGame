using System.Collections;
using UnityEngine;

public class EnemyGunController : MonoBehaviour
{
    [SerializeField] private Transform _gun;
    [SerializeField] private BulletPool _bulletPool;
    [SerializeField] private HealthComponent _enemyHealth;
    [SerializeField] private bool _burstFireMode;
    [SerializeField] private SceneController _sceneController;

    [SerializeField] private AudioSource _shotFiredSound;

    private Transform _player;
    private float _fireRate = .5f;
    private float _fireTimer = 0f;
 
    private Ray _ray;

    void Start()
    {
        _bulletPool = GameObject.Find("EnemyBulletPool").GetComponent<BulletPool>();
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _enemyHealth = GetComponent<HealthComponent>();
    }

    void Update()
    {
        // Check if Player on line of fire
        _ray = new Ray(transform.position, _player.position);
        _fireTimer += Time.deltaTime;

        RaycastHit _hit;

        // Fire
        if(_enemyHealth.isAlive && _sceneController.gameOnPause == false)
        {
            if (Physics.Raycast(_ray, out _hit, Mathf.Infinity, 9)
                && _fireTimer >= (1 / _fireRate) + Random.Range(0f,1f))
            {
                if (_burstFireMode)
                {
                    StartCoroutine(HeavyGunFire());  
                }
                else
                {
                    EnemyFire();
                }

                _fireTimer = 0f;
            }
        }  
    }

    /// <summary>
    /// Single fire mode
    /// </summary>
    private void EnemyFire()
    {
        var _bullet = _bulletPool.Get();
        _bullet.transform.rotation = _gun.transform.rotation;
        _bullet.transform.position = _gun.transform.position;
        _bullet.SetActive(true);

        _shotFiredSound.Play();
    }

    /// Burst gun mode for Boss (and maybe some ordinary enemy) 
    IEnumerator HeavyGunFire()
    {
        for (int i = 0; i < 5; i++)
        { 
            EnemyFire();
            yield return new WaitForSeconds(.1f);
        }  
    }
}
