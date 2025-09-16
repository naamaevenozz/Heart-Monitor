using System;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class DotGameController : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] List<DotMove> dotPrefabs = new();

    [Header("Settings")]
    [SerializeField] int   initialCount   = 4;
    [SerializeField] float autoSpeed      = 0.5f;
    [SerializeField] float switchPeriod   = 2f;
    [SerializeField] float lateralMargin  = 0.5f;

    [Header("Hierarchy & Bounds")]
    [SerializeField] Transform spawnParent;
    [SerializeField] Transform bottomBound;
    [SerializeField] Transform topBound;

    [Header("Pooling")]
    [SerializeField] DotPool pool;

    [Header("Scoring")]
    [SerializeField] PlayerStats player;
    [SerializeField] int pointsOnGraphHit = 100;
    [SerializeField] int lifePenaltyOnMiss = 1;

    readonly List<DotMove> active = new();
    DotSpawner spawner;
    DotSelectionService selection;
    DotStreamService stream;

    Camera cam;
    float switchTimer;

    [Obsolete("Obsolete")]
    void Start()
    {
        cam = Camera.main;

        if (!pool) pool = FindObjectOfType<DotPool>();
        if (!pool || dotPrefabs.Count == 0)
        {
            Debug.LogError("DotGameController: Missing pool or prefabs.");
            enabled = false;
            return;
        }

        pool.Initialize(dotPrefabs);

        spawner   = new DotSpawner(dotPrefabs, pool, spawnParent, bottomBound, topBound, cam);
        selection = new DotSelectionService(active);
        stream    = new DotStreamService(active, spawner, selection, cam, autoSpeed, lateralMargin);

        selection.BeforeSwitchHook = HandleBeforeSwitch;

        GetEdges(out float leftEdge, out float rightEdge);
        spawner.SpawnInitial(initialCount, active, leftEdge, rightEdge);
        selection.SelectIndex(0);
    }

    void Update()
    {
        stream.Tick(Time.deltaTime);

        switchTimer += Time.deltaTime;
        if (switchTimer >= switchPeriod)
        {
            switchTimer = 0f;
            selection.NextSelection();
        }
    }

    void HandleBeforeSwitch(DotMove prevDot)
    {
        if (!player || !prevDot) return;

        if (prevDot.IsTouchingGraph)
            player.AddScore(pointsOnGraphHit);
        else
            player.LoseLife(lifePenaltyOnMiss);
    }

    void GetEdges(out float left, out float right)
    {
        float vert  = cam.orthographicSize;
        float horiz = vert * cam.aspect;
        left  = cam.transform.position.x - horiz;
        right = cam.transform.position.x + horiz;
    }

    // ===== UI Hooks =====
    public void UI_MoveUp()         => selection.MoveUp();
    public void UI_MoveDown()       => selection.MoveDown();
    public void UI_StartHoldUp()    => selection.StartHoldUp();
    public void UI_StartHoldDown()  => selection.StartHoldDown();
    public void UI_StopHold()       => selection.StopHold();

    public void UI_SpawnExtraOne()
    {
        float right = GetRightEdge();
        active.Add(spawner.SpawnAtX(right + lateralMargin));
    }

    public void UI_SpawnExtraMany(int n)
    {
        float right = GetRightEdge();
        for (int i = 0; i < n; i++)
            active.Add(spawner.SpawnAtX(right + lateralMargin + 0.2f * i));
    }

    float GetRightEdge()
    {
        float vert = cam.orthographicSize;
        float horiz = vert * cam.aspect;
        return cam.transform.position.x + horiz;
    }
}
