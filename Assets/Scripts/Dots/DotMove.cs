namespace DefaultNamespace
{
    using UnityEngine;

    [DisallowMultipleComponent]
    public class DotMove : MonoBehaviour, IDotMover
    {
        public enum SpaceMode { World, Local }

        [Header("Step / Speed")]
        [SerializeField] float step = 0.1f;          
        [SerializeField] float holdSpeed = 2f;       
        [SerializeField] SpaceMode space = SpaceMode.World;

        [Header("Bounds")]
        public Transform bottomBound;
        public Transform topBound;

        [Header("Highlight")]
        [SerializeField] Color selectedColor = Color.blue;
        [SerializeField] float selectedScale = 1.15f;

        Rigidbody rb3D;
        Rigidbody2D rb2D;
        SpriteRenderer sr;

        int holdDir = 0;            // 1=Up, -1=Down, 0=None
        [SerializeField] Color baseColor = Color.cyan;
        Vector3 baseScale;

        void Awake()
        {
            rb3D = GetComponent<Rigidbody>();
            rb2D = GetComponent<Rigidbody2D>();
            sr   = GetComponent<SpriteRenderer>();

            baseScale = transform.localScale;
            if (sr) baseColor = sr.color;
        }

        void OnDisable()
        {
            holdDir = 0;
        }

        // ===== UI Interface =====
        public void MoveUp()        => Move(step);
        public void MoveDown()      => Move(-step);
        public void StartHoldUp()   { holdDir = 1; }
        public void StartHoldDown() { holdDir = -1; }
        public void StopHold()      { holdDir = 0; }

        void Update()
        {
            if (holdDir != 0 && rb3D == null && rb2D == null)
                Move(holdDir * holdSpeed * Time.deltaTime);
        }

        void FixedUpdate()
        {
            if (holdDir != 0 && (rb3D != null || rb2D != null))
                Move(holdDir * holdSpeed * Time.fixedDeltaTime);
        }

        void Move(float delta)
        {
            Vector3 axis   = (space == SpaceMode.World) ? Vector3.up : transform.up;
            Vector3 target = transform.position + axis * delta;

            if (topBound || bottomBound)
            {
                float minY = bottomBound ? bottomBound.position.y : float.NegativeInfinity;
                float maxY = topBound    ? topBound.position.y    : float.PositiveInfinity;
                target.y = Mathf.Clamp(target.y, minY, maxY);
            }

            if (rb2D) target.z = transform.position.z;

            if (rb2D)      rb2D.MovePosition(new Vector2(target.x, target.y));
            else if (rb3D) rb3D.MovePosition(target);
            else           transform.position = target;
        }

        public void SetSelected(bool on)
        {
            if (sr) sr.color = on ? selectedColor : baseColor;
            transform.localScale = on ? baseScale * selectedScale : baseScale;
        }

        // ===== שימושי לפולינג =====
        public void ResetForReuse()
        {
            holdDir = 0;
            if (rb2D)
            {
                rb2D.linearVelocity = Vector2.zero;
                rb2D.angularVelocity = 0f;
            }
            if (rb3D)
            {
                rb3D.linearVelocity = Vector3.zero;
                rb3D.angularVelocity = Vector3.zero;
            }

            SetSelected(false);
        }
        
        public void SetBaseAppearance(Color color, float scale = 1f)
        {
            baseColor = color;
            baseScale = Vector3.one * scale;
            if (sr) sr.color = baseColor;
            if (transform.localScale != baseScale && scale > 0f)
                transform.localScale = baseScale;
        }
    }
}
