using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class DotSpawner
{
    readonly List<DotMove> prefabs;
    readonly DotPool pool;
    readonly Transform spawnParent;
    readonly Transform bottomBound, topBound;
    readonly Camera cam;

    public DotSpawner(List<DotMove> prefabs, DotPool pool, Transform spawnParent,
        Transform bottomBound, Transform topBound, Camera cam)
    {
        this.prefabs = prefabs;
        this.pool = pool;
        this.spawnParent = spawnParent;
        this.bottomBound = bottomBound;
        this.topBound = topBound;
        this.cam = cam;
    }

    public void SpawnInitial(int count, List<DotMove> active, float leftEdge, float rightEdge)
    {
        float span = (rightEdge - leftEdge) * 0.9f;
        float spacing = (count > 1) ? span / (count - 1) : span;

        for (int i = 0; i < count; i++)
        {
            float x = rightEdge - (i * spacing);
            active.Add(SpawnAtX(x));
        }
    }

    public DotMove SpawnAtX(float x)
    {
        var prefab = prefabs[Random.Range(0, prefabs.Count)];
        float y = RandomYWithinBounds();

        var instance = pool.Get(prefab, new Vector3(x, y, 0f), spawnParent);

        instance.bottomBound = bottomBound;
        instance.topBound = topBound;
        instance.SetSelected(false);
        instance.StopHold();

        return instance;
    }

    public void Despawn(DotMove dot) => pool.Release(dot);

    float RandomYWithinBounds()
    {
        if (topBound && bottomBound)
            return Random.Range(bottomBound.position.y, topBound.position.y);

        float vert = cam.orthographicSize;
        return Random.Range(cam.transform.position.y - vert * 0.8f,
            cam.transform.position.y + vert * 0.8f);
    }
}