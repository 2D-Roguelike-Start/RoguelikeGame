using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager
{
    #region Pool
    class Pool 
    { 
        public GameObject Original { get; private set; }
        public Transform Root { get; set; }

        Stack<Poolable> _poolStack = new Stack<Poolable>();

        //정해놓은 갯수만큼 Pool_Root산하에 배치
        public void init(GameObject original, int count = 5)
        {
            Original = original;
            Root = new GameObject().transform;
            Root.name = $"{original.name}_Root";

            for (int i = 0; i < count; i++)
                Push(Create());
        }

        Poolable Create()
        {
            GameObject go = Object.Instantiate<GameObject>(Original);
            go.name = Original.name;
            return go.GetAddComponent<Poolable>();
        }

        public void Push(Poolable poolable)
        {
            if (poolable == null) return;

            poolable.transform.parent = Root;
            //꺼내기 전까지 꺼놓기
            poolable.gameObject.SetActive(false);
            poolable.isUse = false;

            _poolStack.Push(poolable);
        }

        public Poolable Pop(Transform parent)
        {
            //필요하면 스택에서 꺼냄
            Poolable poolable;
            if (_poolStack.Count > 0)
                poolable = _poolStack.Pop();
            else
                poolable = Create(); //없으면 다시정해진 갯수만큼 만듬

            //꺼낸걸 root산하에 붙이고 setactive(true)
            poolable.gameObject.SetActive(true);
            poolable.transform.parent = parent;
            poolable.isUse = true;

            return poolable;
        }
    }
    #endregion

    Dictionary<string, Pool> _pool = new Dictionary<string, Pool>();
    Transform _root;
    public void init()
    {
        if(_root == null)
        {
            _root = new GameObject { name = "@Pool_Root" }.transform;
            Object.DontDestroyOnLoad(_root);
        }
    }

    public void CreatePool(GameObject original, int count = 5)
    {
        Pool pool = new Pool();
        pool.init(original, count);
        pool.Root.parent = _root;

        _pool.Add(original.name, pool);
    }

    public void Push(Poolable poolable)
    {
        string name = poolable.gameObject.name;
        if(_pool.ContainsKey(name) == false)
        {
            GameObject.Destroy(poolable.gameObject);
            return;
        }
        _pool[name].Push(poolable);
    }

    public Poolable Pop(GameObject original, Transform parent = null)
    {
        //풀에 오브젝트가 없다면 생성
        if (_pool.ContainsKey(original.name) == false)
            CreatePool(original);

        return _pool[original.name].Pop(parent);
    }

    public GameObject GetOriginal(string name)
    {
        if (_pool.ContainsKey(name) == false)
            return null;
        return _pool[name].Original;
    }

    public void Clear()
    {
        foreach (Transform child in _root)
            GameObject.Destroy(child.gameObject);

        _pool.Clear();
    }
}
