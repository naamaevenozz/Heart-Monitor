using System;
using System.Collections.Generic;
using UnityEngine;
using DefaultNamespace; // DotMove

public class DotSelectionService
{
    readonly List<DotMove> active;
    int currentIndex = -1;
    int heldDir = 0; // -1=Down, 0=None, +1=Up

    public DotSelectionService(List<DotMove> active) => this.active = active;

    public int CurrentIndex => currentIndex;
    public int HeldDir => heldDir;

    public Action<DotMove> BeforeSwitchHook;
    public void SelectIndex(int idx)
    {
        if (active.Count == 0) { currentIndex = -1; return; }

        if (currentIndex >= 0 && currentIndex < active.Count)
        {
            BeforeSwitchHook?.Invoke(active[currentIndex]);
            active[currentIndex].StopHold();
            active[currentIndex].SetSelected(false);
        }

        currentIndex = Mathf.Clamp(idx, 0, active.Count - 1);
        active[currentIndex].SetSelected(true);

        if (heldDir == 1) active[currentIndex].StartHoldUp();
        else if (heldDir == -1) active[currentIndex].StartHoldDown();
    }

    public void NextSelection()
    {
        if (active.Count == 0) return;
        active.Sort((a, b) => a.transform.position.x.CompareTo(b.transform.position.x));
        int indexInSorted = active.IndexOf(active[currentIndex]);
        int nextIndex = (indexInSorted + 1) % active.Count;
        SelectIndex(nextIndex);
    }

    public void HandleRemoval(int removedIndex)
    {
        if (active.Count == 0) { currentIndex = -1; return; }

        if (removedIndex < currentIndex) currentIndex--;
        else if (removedIndex == currentIndex) currentIndex = Mathf.Clamp(currentIndex, 0, active.Count - 1);
    }

    public void ReapplySelectionWithHold()
    {
        if (currentIndex < 0 || currentIndex >= active.Count) return;
        active[currentIndex].SetSelected(true);
        if (heldDir == 1) active[currentIndex].StartHoldUp();
        else if (heldDir == -1) active[currentIndex].StartHoldDown();
    }

    // UI actions
    public void MoveUp()        { if (currentIndex >= 0 && currentIndex < active.Count) active[currentIndex].MoveUp(); }
    public void MoveDown()      { if (currentIndex >= 0 && currentIndex < active.Count) active[currentIndex].MoveDown(); }
    public void StartHoldUp()   { heldDir = 1;  if (currentIndex >= 0) active[currentIndex].StartHoldUp(); }
    public void StartHoldDown() { heldDir = -1; if (currentIndex >= 0) active[currentIndex].StartHoldDown(); }
    public void StopHold()      { if (currentIndex >= 0) active[currentIndex].StopHold(); heldDir = 0; }
}
