namespace DefaultNamespace
{
    using UnityEngine;
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

        Rigidbody rb3D;
        Rigidbody2D rb2D;
        int holdDir = 0; // 1=up, -1=down, 0=none

        void Awake()
        {
            rb3D = GetComponent<Rigidbody>();
            rb2D = GetComponent<Rigidbody2D>();
        }

        //=== UI buttons ===
        public void MoveUp()         => Move(step);
        public void MoveDown()       => Move(-step);
        public void StartHoldUp()    { holdDir = 1; }
        public void StartHoldDown()  { holdDir = -1; }
        public void StopHold()       { holdDir = 0; }

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
            Vector3 axis = (space == SpaceMode.World) ? Vector3.up : transform.up;
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
    }
}