using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    [SerializeField] private GameObject _bullet; // Bullet prefab reference

    private Queue<GameObject> _bullets = new Queue<GameObject>();
    public static BulletPool Instance { get; private set; }

    // Singleton
    private void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// Get bullet from BulletPool
    /// </summary>
    /// <returns></returns>
    public GameObject Get()
    {
        if (_bullets.Count == 0 && _bullets.Count <= 5)
            AddBullet(1);
        return _bullets.Dequeue();
    }

    /// <summary>
    /// Return Bullet to Pool
    /// </summary>
    /// <param name="bulletToReturn">Refrence to Bullet to return</param>
    public void ReturnToPool(GameObject bulletToReturn)
    {
        bulletToReturn.SetActive(false);
        _bullets.Enqueue(bulletToReturn);
    }

    /// <summary>
    /// Add Bullet to Pool
    /// </summary>
    /// <param name="count">Nubmer of Bullets to Add</param>
    private void AddBullet(int count)
    {
        GameObject newBullet = GameObject.Instantiate(_bullet);
        newBullet.SetActive(false);
        _bullets.Enqueue(newBullet);

        newBullet.GetComponent<IBulletPooled>().Pool = this;
    }
}
