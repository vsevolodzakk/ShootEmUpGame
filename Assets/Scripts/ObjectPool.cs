using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    // Object prefab
    [SerializeField] private GameObject _prefab;

    private Queue<GameObject> _objects = new Queue<GameObject>();

    public static ObjectPool Instance { get; private set; }

    // Singleton
    private void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// Get Object from Pool
    /// </summary>
    /// <returns></returns>
    public GameObject Get()
    {
        if (_objects.Count == 0)
            AddObjects(1);
        return _objects.Dequeue();
    }

    /// <summary>
    /// Return Object to Pool
    /// </summary>
    /// <param name="objectToReturn"></param>
    public void ReturnToPool(GameObject objectToReturn)
    {
        objectToReturn.SetActive(false);
        _objects.Enqueue(objectToReturn);
    }

    /// <summary>
    /// Add more Object instances to Pool
    /// </summary>
    /// <param name="count"></param>
    private void AddObjects(int count)
    {
        var _newObject = GameObject.Instantiate(_prefab);
        _newObject.SetActive(false);
        _objects.Enqueue(_newObject);

        _newObject.GetComponent<IGameObjectPooled>().Pool = this;
    }
}
