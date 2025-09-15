using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class DotStreamService
{
    readonly List<DotMove> active;
    readonly DotSpawner spawner;
    readonly DotSelectionService selection;
    readonly Camera cam;
    readonly float autoSpeed;
    readonly float lateralMargin;

    public DotStreamService(List<DotMove> active, DotSpawner spawner, DotSelectionService selection,
                            Camera cam, float autoSpeed, float lateralMargin)
    {
        this.active = active;
        this.spawner = spawner;
        this.selection = selection;
        this.cam = cam;
        this.autoSpeed = autoSpeed;
        this.lateralMargin = lateralMargin;
    }

    public void Tick(float dt)
    {
        GetEdges(out float leftEdge, out float rightEdge);

        for (int i = active.Count - 1; i >= 0; i--)
        {
            var dot = active[i];
            if (!dot) { active.RemoveAt(i); continue; }

            MoveLeft(dot, autoSpeed * dt);

            if (dot.transform.position.x < leftEdge - lateralMargin)
            {
                bool wasCurrent = (i == selection.CurrentIndex);

                if (wasCurrent)
                    selection.BeforeSwitchHook?.Invoke(dot);

                dot.StopHold();
                dot.SetSelected(false);
                spawner.Despawn(dot);
                active.RemoveAt(i);
                selection.HandleRemoval(i);

                var newDot = spawner.SpawnAtX(rightEdge + lateralMargin);
                active.Add(newDot);

                if (wasCurrent && active.Count > 0)
                    selection.ReapplySelectionWithHold();
            }
        }
    }

    void MoveLeft(DotMove dot, float dx)
    {
        dx = Mathf.Abs(dx);
        var pos = dot.transform.position;

        if (dot.TryGetComponent(out Rigidbody2D rb))
            rb.MovePosition(new Vector2(pos.x - dx, pos.y));
        else
            dot.transform.Translate(Vector2.left * dx, Space.World);
    }

    void GetEdges(out float left, out float right)
    {
        float vert = cam.orthographicSize;
        float horiz = vert * cam.aspect;
        left = cam.transform.position.x - horiz;
        right = cam.transform.position.x + horiz;
    }
}
