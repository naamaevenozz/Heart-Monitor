using System.Collections.Generic;
using UnityEngine;
using DefaultNamespace; 

public class DotGameController : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] List<DotMove> dotPrefabs = new();

    [Header("Initial / Speed / Switch")]
    [SerializeField] int   initialCount = 4;
    [SerializeField] float autoSpeed    = 0.5f;
    [SerializeField] float switchPeriod = 2f;

    [Header("Spawn Parents / Bounds")]
    [SerializeField] Transform spawnParent;
    [SerializeField] Transform bottomBound;
    [SerializeField] Transform topBound;
    [SerializeField] float    lateralMargin = 0.5f;

    [Header("Dependencies")]
    [SerializeField] DotPool pool;

    [Header("Scoring")]
    [SerializeField] PlayerStats player;       // ← drag your PlayerStats here
    [SerializeField] int pointsOnGraphHit = 100;
    [SerializeField] int lifePenaltyOnMiss = 1;

    // runtime
    readonly List<DotMove> active = new();
    DotSpawner spawner;
    DotSelectionService selection;
    DotStreamService stream;

    Camera cam;
    float switchTimer;

    void Start()
    {
        cam = Camera.main;

        if (!pool) pool = FindObjectOfType<DotPool>();
        if (!pool) { Debug.LogError("DotGameController: Missing DotPool."); enabled = false; return; }
        if (dotPrefabs.Count == 0) { Debug.LogError("DotGameController: No prefabs."); enabled = false; return; }

        pool.Initialize(dotPrefabs);

        spawner   = new DotSpawner(dotPrefabs, pool, spawnParent, bottomBound, topBound, cam);
        selection = new DotSelectionService(active);
        stream    = new DotStreamService(active, spawner, selection, cam, autoSpeed, lateralMargin);

        // Register scoring hook before each selection switch (timer only)
        selection.BeforeSwitchHook = HandleBeforeSwitch;

        // Spawn initial & select first
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

    // Called ONLY on timed switch, not on recycle
    void HandleBeforeSwitch(DotMove prevDot)
    {
        if (!player || !prevDot) return;
        
        Debug.Log("SWITCH | IsTouchingGraph: " + prevDot.IsTouchingGraph);
        
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

    // ===== UI hooks =====
    public void UI_MoveUp()        => selection.MoveUp();
    public void UI_MoveDown()      => selection.MoveDown();
    public void UI_StartHoldUp()   => selection.StartHoldUp();
    public void UI_StartHoldDown() => selection.StartHoldDown();
    public void UI_StopHold()      => selection.StopHold();

    public void UI_SpawnExtraOne()
    {
        float vert  = cam.orthographicSize;
        float horiz = vert * cam.aspect;
        float right = cam.transform.position.x + horiz;
        active.Add(spawner.SpawnAtX(right + lateralMargin));
    }

    public void UI_SpawnExtraMany(int n)
    {
        float vert  = cam.orthographicSize;
        float horiz = vert * cam.aspect;
        float right = cam.transform.position.x + horiz;
        for (int i = 0; i < n; i++)
            active.Add(spawner.SpawnAtX(right + lateralMargin + 0.2f * i));
    }
}
