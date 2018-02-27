using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallaxer : MonoBehaviour {

    class PoolObject
    {

        public Transform transform;
        public bool inUse;
        public PoolObject(Transform t)
        {
            transform = t;
        }
        public void Use()
        {
            inUse = true;
        }
        public void Dispose()
        {
            inUse = false;
        }

    }

    [System.Serializable]
    public struct YSpwnRange
    {
        public float min;
        public float max;
    }

    public GameObject prefab;
    public int poolSize;
    public float shiftSpeed;
    public float spwnRate;

    public YSpwnRange ySpwnRange;
    public Vector3 spwnPos;
    public bool spwnImmediate;
    public Vector3 spwnImmediatePos;
    public Vector2 targetAspectRatio;

    float spwnTimer;
    float targetAspect;

    PoolObject[] poolObjects;
    GameManager GM;

    void Awake ()
    {
        Configure();
    }

    void Start ()
    {
        GM = GameManager.Instance;
    }

    void OnEnable ()
    {
        GameManager.OnGameOverConfirmed += OnGameOverConfirmed;
    }

    void OnDisable ()
    {
        GameManager.OnGameOverConfirmed -= OnGameOverConfirmed;
    }

    void OnGameOverConfirmed ()
    {
        for (int i = 0; i < poolObjects.Length; i++)
        {
            poolObjects[i].Dispose();
            poolObjects[i].transform.position = Vector3.one * 1000;
        }
        if (spwnImmediate)
        {
            SpawnImmediate();
        }
    }

    void Update ()
    {
        if (GM.gameOver) return;
        Shift();
        spwnTimer += Time.deltaTime;
        if (spwnTimer>spwnRate)
        {
            Spawn();
            spwnTimer = 0;
        }
    }

    void Configure ()
    {
        targetAspect = targetAspectRatio.x / targetAspectRatio.y;
        poolObjects = new PoolObject[poolSize];
        for (int i = 0; i < poolObjects.Length; i++)
        {
            GameObject go = Instantiate(prefab) as GameObject;
            Transform t = go.transform;
            t.SetParent(transform);
            t.position = Vector3.one * 1000;
            poolObjects[i] = new PoolObject(t);
        }
        if (spwnImmediate)
        {
            SpawnImmediate();
        }
    }

    void Spawn()
    {
        Transform t = GetPoolObject();
        if (t==null)
        {
            return;
        }
        Vector3 pos = Vector3.zero;
        pos.x = spwnPos.x * Camera.main.aspect / targetAspect;
        pos.y = Random.Range(ySpwnRange.min, ySpwnRange.max);
        t.position = pos;
    }

    void SpawnImmediate()
    {
        Transform t = GetPoolObject();
        if (t == null)
        {
            return;
        }
        Vector3 pos = Vector3.zero;
        pos.x = spwnImmediatePos.x * Camera.main.aspect / targetAspect;
        pos.y = Random.Range(ySpwnRange.min, ySpwnRange.max);
        t.position = pos;
    }

    void Shift()
    {
        if (GM.gameOver) return;/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        for (int i = 0; i < poolObjects.Length; i++)
        {
                poolObjects[i].transform.localPosition += -Vector3.right * shiftSpeed * Time.deltaTime;
                CheckDisposeObject(poolObjects[i]);
        }
    }

    void CheckDisposeObject(PoolObject poolObject)
    {
        if (poolObject.transform.position.x < (-spwnPos.x*Camera.main.aspect)/targetAspect)
        {
            poolObject.Dispose();
            poolObject.transform.position = Vector3.one * 1000;
        }
    }

    Transform GetPoolObject ()
    {
        for (int i = 0; i < poolObjects.Length; i++)
        {
            if (!poolObjects[i].inUse)
            {
                poolObjects[i].Use();
                return poolObjects[i].transform;
            }
        }
        return null;
    }
}
