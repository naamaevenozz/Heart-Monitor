using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class DotGameController : MonoBehaviour
{
    [Header("Prefabs to spawn")]
    [SerializeField] List<DotMove> dotPrefabs = new();

    [Header("Spawn")]
    [SerializeField] Transform spawnParent;                 
    [SerializeField] Vector2 spawnXRange = new(0f, 0f);      
    [SerializeField] Vector2 spawnYRange = new(-3f, 3f);

    [Header("Bounds to assign to spawned dot (optional)")]
    [SerializeField] Transform bottomBound;
    [SerializeField] Transform topBound;

    IDotMover current;       
    GameObject currentGO;    
    
    //public void UI_ClickDebug() { Debug.Log("CLICK!"); }

    void Start()
    {
        if (dotPrefabs.Count == 0)
        {
            Debug.LogError("DotGameController: weren't defined Prefabs in the list.");
            return;
        }

        SpawnNewDot();
    }

    public void SpawnNewDot()
    {
        if (dotPrefabs.Count == 0) return;

        if (currentGO) Destroy(currentGO);

        var prefab = dotPrefabs[Random.Range(0, dotPrefabs.Count)];
        Vector3 pos = new(
            Random.Range(spawnXRange.x, spawnXRange.y),
            Random.Range(spawnYRange.x, spawnYRange.y),
            0f);

        currentGO = Instantiate(prefab.gameObject, pos, Quaternion.identity, spawnParent);
        var mover = currentGO.GetComponent<DotMove>();
        if (mover)
        {
            mover.bottomBound = bottomBound;
            mover.topBound = topBound;
            current = mover; 
        }
        else
        {
            current = currentGO.GetComponent<IDotMover>();
        }

        if (current == null)
            Debug.LogError("DotGameController: the prefub dosen't implenemt IDotMover.");
    }

    public void UI_MoveUp()        => current?.MoveUp();
    public void UI_MoveDown()      => current?.MoveDown();
    public void UI_StartHoldUp()   => current?.StartHoldUp();
    public void UI_StartHoldDown() => current?.StartHoldDown();
    public void UI_StopHold()      => current?.StopHold();
    public void UI_NewDot()        => SpawnNewDot();
}
