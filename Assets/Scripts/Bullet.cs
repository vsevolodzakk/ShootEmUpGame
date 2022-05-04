using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour, IBulletPooled
{
    [SerializeField] private float _speed = 50f;
    [SerializeField] private float _maxLifetime = 3f;
    
    private float _lifetime;
    private BulletPool _bulletPool;

    public BulletPool Pool
    {
        get { return _bulletPool; }
        set
        {
            if (_bulletPool == null)
                _bulletPool = value;
            else
                throw new System.Exception("Bad pool use!");
        }
    }

    private void OnEnable()
    {
        _lifetime = 0f;
    }
    private void Update()
    {
        transform.Translate(Vector3.forward * _speed * Time.deltaTime);
        _lifetime += Time.deltaTime;
        if (_lifetime > _maxLifetime)
            Pool.ReturnToPool(this.gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("Wall") || other.CompareTag("Player"))
        {
            Pool.ReturnToPool(this.gameObject);
        }      
    }
}
