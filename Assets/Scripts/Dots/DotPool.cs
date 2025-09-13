using System;
using System.Collections.Generic;
using UnityEngine;
using DefaultNamespace;

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
            var go = new GameObject("DotPool_Root");
            poolRoot = go.transform;
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

        DotMove inst;
        if (stack.Count > 0) inst = stack.Pop();
        else { inst = Instantiate(prefab); Register(inst, prefab); }

        inst.ResetForReuse();

        if (parent) inst.transform.SetParent(parent, false);
        else        inst.transform.SetParent(null, false);

        inst.transform.position = position;
        inst.gameObject.SetActive(true);
        return inst;
    }

    [Obsolete("Obsolete")]
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

        // DotPool.Release(...)
        if (inst.TryGetComponent<Rigidbody2D>(out var rb2d)) {
            rb2d.velocity = Vector2.zero;
            rb2d.angularVelocity = 0f;
        }
        if (inst.TryGetComponent<Rigidbody>(out var rb3d)) {
            rb3d.velocity = Vector3.zero;
            rb3d.angularVelocity = Vector3.zero;
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
