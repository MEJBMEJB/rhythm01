using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectInfo
{
    public GameObject _goPrefab;
    public int _count;
    public Transform _tfPoolParent;
}


public class ObjectPool : MonoBehaviour
{
    private static ObjectPool _instanceObjectPool;
    [SerializeField]
    private ObjectInfo[] _objectInfo = null;

    private Queue<GameObject> _noteQueue;
    
    public static ObjectPool Instance
    {
        get => _instanceObjectPool;
    }

    public Queue<GameObject> NoteQueue
    { 
        get => _noteQueue; 
    }

    // Start is called before the first frame update
    void Start()
    {
        _instanceObjectPool = this;
        _noteQueue = new Queue<GameObject>();
        _noteQueue = InsertQueue(_objectInfo[0]);
    }

    Queue<GameObject> InsertQueue(ObjectInfo p_objectInfo)
    {
        Queue<GameObject> t_queue = new Queue<GameObject>();
        for(int i = 0; i < p_objectInfo._count; i++) 
        {
            GameObject t_clone = Instantiate(p_objectInfo._goPrefab, transform.position, Quaternion.identity);
            t_clone.SetActive(false);

            if (p_objectInfo._tfPoolParent != null)
                t_clone.transform.SetParent(p_objectInfo._tfPoolParent);
            else
                t_clone.transform.SetParent(this.transform);
            t_queue.Enqueue(t_clone);
        }
        return t_queue;
    }

}
