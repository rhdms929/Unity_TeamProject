using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
public class ObjectPoolManager : MonoBehaviour //풀링 할 옵젝 인스펙터 창에서 개수 정해서 이름(정확하게 써야함)이랑 prefab 넣으면 됩니당
{
    [System.Serializable]
    private class ObjectInfo
    {
        public string objectName; //옵젝 이름
        public GameObject prefab; //풀에서 관리할 옵젝 넣기
        public int count = 10; //몇개를 미리 생성 할지
        public int maxPoolSize = 30; //풀 최대 보관 개수
    }
    public static ObjectPoolManager instance;
    public bool IsReady { get; private set; } //오브젝트풀 매니저 준비 완료 표시

    [SerializeField]
    private ObjectInfo[] objectInfos = null;

    private Dictionary<string, IObjectPool<GameObject>> objectPoolDic = new Dictionary<string, IObjectPool<GameObject>>();
    private Dictionary<string, GameObject> prefabDic = new Dictionary<string, GameObject>();
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        Init();
    }
    private void Init()
    {
        IsReady = false;
        objectPoolDic.Clear();
        prefabDic.Clear();

        foreach (var info in objectInfos)
        {
            if (string.IsNullOrWhiteSpace(info.objectName))
            {
                Debug.LogError("objectName이 비어있음");
                continue;
            }

            if (info.prefab == null)
            {
                Debug.LogError(info.objectName + " prefab이 비어있음");
                continue;
            }

            if (prefabDic.ContainsKey(info.objectName))
            {
                Debug.LogError(info.objectName + " 은(는) 이미 등록됨");
                continue;
            }
            prefabDic.Add(info.objectName, info.prefab);

            string key = info.objectName;

            IObjectPool<GameObject> pool = new ObjectPool<GameObject>(
                createFunc: () => CreatePooledItem(key),
                actionOnGet: OnTakeFromPool,
                actionOnRelease: OnReturnedToPool,
                actionOnDestroy: OnDestroyPoolObject,
                collectionCheck: true,
                defaultCapacity: info.count,
                maxSize: info.maxPoolSize
            );

            objectPoolDic.Add(key, pool);

            for (int i = 0; i < info.count; i++)
            {
                GameObject go = pool.Get();
                pool.Release(go);
            }
        }
        Debug.Log("오브젝트풀링 준비 완료");
        IsReady = true;
    }
    private GameObject CreatePooledItem(string key)
    {
        GameObject poolGo = Instantiate(prefabDic[key]);
        PoolAble poolAble = poolGo.GetComponent<PoolAble>();

        if (poolAble == null)
        {
            Debug.LogError(key + " 프리팹에 PoolAble이 없음");
        }
        else
        {
            poolAble.Pool = objectPoolDic[key];
        }

        return poolGo;
    }
    private void OnTakeFromPool(GameObject poolGo)
    {
        poolGo.SetActive(true);
    }
    private void OnReturnedToPool(GameObject poolGo)
    {
        poolGo.SetActive(false);
    }
    private void OnDestroyPoolObject(GameObject poolGo)
    {
        Destroy(poolGo);
    }
    public GameObject GetGo(string goName)
    {
        if (objectPoolDic.ContainsKey(goName) == false)
        {
            Debug.LogError(goName + " 오브젝트풀에 등록되지 않음");
            return null;
        }

        return objectPoolDic[goName].Get();
    }
}
