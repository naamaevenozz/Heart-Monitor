// namespace DefaultNamespace
using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(RectTransform))]
public class SafeAreaFitter : MonoBehaviour
{
    [Tooltip("extra distance inside the Safe Area (pixelas in each direction)")]
    public Vector2Int extraPadding = Vector2Int.zero;

    RectTransform rt;
    Rect lastSafe;           
    Rect lastPixelRect;

    void Awake()  { rt = GetComponent<RectTransform>(); }
    void OnEnable() => Apply();
#if UNITY_EDITOR
    void Update() { if (!Application.isPlaying) Apply(); }
#endif

    void Apply()
    {
        var canvas = rt.GetComponentInParent<Canvas>();
        if (!canvas) return;

        var pixelRect = canvas.pixelRect;

        var sa = Screen.safeArea;

        sa.x -= pixelRect.x;
        sa.y -= pixelRect.y;

        sa.xMin += extraPadding.x;
        sa.xMax -= extraPadding.x;
        sa.yMin += extraPadding.y;
        sa.yMax -= extraPadding.y;

        sa.width  = Mathf.Clamp(sa.width,  0, pixelRect.width);
        sa.height = Mathf.Clamp(sa.height, 0, pixelRect.height);

        if (sa == lastSafe && pixelRect == lastPixelRect) return;
        lastSafe = sa; lastPixelRect = pixelRect;

        Vector2 min = new Vector2(sa.xMin / pixelRect.width,  sa.yMin / pixelRect.height);
        Vector2 max = new Vector2(sa.xMax / pixelRect.width,  sa.yMax / pixelRect.height);

        min = Vector2.Max(Vector2.zero, Vector2.Min(min, Vector2.one));
        max = Vector2.Max(Vector2.zero, Vector2.Min(max, Vector2.one));

        rt.anchorMin = min;
        rt.anchorMax = max;
        rt.offsetMin = Vector2.zero;   
        rt.offsetMax = Vector2.zero;
    }
}
