using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    [SerializeField] private GameObject _bullet;

    public static BulletPool Instance { get; private set; }

    private Queue<GameObject> _bullets = new Queue<GameObject>();

    private void Awake()
    {
        Instance = this;
    }

    // Get bullet from BulletPool
    public GameObject Get()
    {
        if (_bullets.Count == 0 && _bullets.Count <= 5)
            AddBullet(1);
        return _bullets.Dequeue();
    }

    // Return Bullet to Pool
    public void ReturnToPool(GameObject bulletToReturn)
    {
        bulletToReturn.SetActive(false);
        _bullets.Enqueue(bulletToReturn);
    }

    // Add Bullet to Pool
    private void AddBullet(int count)
    {
        GameObject newBullet = GameObject.Instantiate(_bullet);
        newBullet.SetActive(false);
        _bullets.Enqueue(newBullet);

        newBullet.GetComponent<IBulletPooled>().Pool = this;
    }
}
