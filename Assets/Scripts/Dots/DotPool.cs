using System;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

[DisallowMultipleComponent]
public class DotPool : MonoBehaviour
{
    [Header("Pool Settings")]
    [SerializeField] Transform poolRoot;
    [SerializeField] int prewarmPerPrefab = 4;   
    [SerializeField] int maxPoolPerPrefab = 64;  

    readonly Dictionary<DotMove, Stack<DotMove>> pool = new();
    readonly Dictionary<DotMove, DotMove> instanceToPrefab = new();

    void Awake()
    {
        if (!poolRoot)
        {
            var root = new GameObject("DotPool_Root");
            poolRoot = root.transform;
            poolRoot.SetParent(transform, false);
        }
    }

    public void Initialize(IEnumerable<DotMove> prefabs)
    {
        foreach (var prefab in prefabs)
        {
            if (prefab == null) continue;

            if (!pool.ContainsKey(prefab))
                pool[prefab] = new Stack<DotMove>(Mathf.Max(4, prewarmPerPrefab));

            for (int i = 0; i < prewarmPerPrefab; i++)
            {
                var inst = Instantiate(prefab, poolRoot);
                Register(inst, prefab);
                Release(inst); 
            }
        }
    }

    void Register(DotMove inst, DotMove prefab)
    {
        instanceToPrefab[inst] = prefab;
        inst.gameObject.SetActive(false);
    }

    public DotMove Get(DotMove prefab, Vector3 position, Transform parent = null)
    {
        if (!pool.TryGetValue(prefab, out var stack))
        {
            stack = new Stack<DotMove>();
            pool[prefab] = stack;
        }

        DotMove inst = (stack.Count > 0) ? stack.Pop() : Instantiate(prefab);
        Register(inst, prefab);
        inst.ResetForReuse();

        inst.transform.SetParent(parent ? parent : null, false);
        inst.transform.position = position;
        inst.gameObject.SetActive(true);

        return inst;
    }

    public void Release(DotMove inst)
    {
        if (!inst) return;

        if (!instanceToPrefab.TryGetValue(inst, out var prefab))
        {
            Destroy(inst.gameObject);
            return;
        }

        inst.StopHold();
        inst.SetSelected(false);

        // אפס מהירות של Rigidbody2D בלבד
        if (inst.TryGetComponent<Rigidbody2D>(out var rb2D))
        {
            rb2D.linearVelocity = Vector2.zero;
            rb2D.angularVelocity = 0f;
        }

        if (maxPoolPerPrefab > 0 && pool[prefab].Count >= maxPoolPerPrefab)
        {
            Destroy(inst.gameObject);
            instanceToPrefab.Remove(inst);
            return;
        }

        inst.transform.SetParent(poolRoot, false);
        inst.gameObject.SetActive(false);
        pool[prefab].Push(inst);
    }
}
