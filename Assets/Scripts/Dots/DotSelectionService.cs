using System;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class DotSelectionService
{
    readonly List<DotMove> active;
    int currentIndex = -1;
    int heldDir = 0; // -1=Down, 0=None, +1=Up

    public DotSelectionService(List<DotMove> active) => this.active = active;

    public int CurrentIndex => currentIndex;
    public int HeldDir => heldDir;

    public Action<DotMove> BeforeSwitchHook;

    public void SelectIndex(int index)
    {
        if (active.Count == 0) { currentIndex = -1; return; }

        if (IsValid(currentIndex))
        {
            BeforeSwitchHook?.Invoke(active[currentIndex]);
            active[currentIndex].StopHold();
            active[currentIndex].SetSelected(false);
        }

        currentIndex = Mathf.Clamp(index, 0, active.Count - 1);
        active[currentIndex].SetSelected(true);

        ApplyHold();
    }

    public void NextSelection()
    {
        if (active.Count == 0 || !IsValid(currentIndex)) return;

        active.Sort((a, b) => a.transform.position.x.CompareTo(b.transform.position.x));

        int indexInSorted = active.IndexOf(active[currentIndex]);
        int nextIndex = (indexInSorted + 1) % active.Count;
        SelectIndex(nextIndex);
    }

    public void HandleRemoval(int removedIndex)
    {
        if (active.Count == 0) { currentIndex = -1; return; }

        if (removedIndex < currentIndex) currentIndex--;
        else if (removedIndex == currentIndex)
            currentIndex = Mathf.Clamp(currentIndex, 0, active.Count - 1);
    }

    public void ReapplySelectionWithHold()
    {
        if (!IsValid(currentIndex)) return;

        active[currentIndex].SetSelected(true);
        ApplyHold();
    }

    void ApplyHold()
    {
        if (!IsValid(currentIndex)) return;

        if (heldDir == 1)      active[currentIndex].StartHoldUp();
        else if (heldDir == -1) active[currentIndex].StartHoldDown();
    }

    bool IsValid(int index) => index >= 0 && index < active.Count;

    // === UI Hooks ===
    public void MoveUp()        { if (IsValid(currentIndex)) active[currentIndex].MoveUp(); }
    public void MoveDown()      { if (IsValid(currentIndex)) active[currentIndex].MoveDown(); }
    public void StartHoldUp()   { heldDir = 1;  if (IsValid(currentIndex)) active[currentIndex].StartHoldUp(); }
    public void StartHoldDown() { heldDir = -1; if (IsValid(currentIndex)) active[currentIndex].StartHoldDown(); }
    public void StopHold()      { if (IsValid(currentIndex)) active[currentIndex].StopHold(); heldDir = 0; }
}
