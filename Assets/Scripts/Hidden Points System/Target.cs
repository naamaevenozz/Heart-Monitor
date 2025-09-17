using System;
using DefaultNamespace;
using DefaultNamespace.Player;
using ScoreSystem;
using Hidden_Points_System;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.EventSystems;
using Utils;

public class Target : MonoBehaviour, IPoolable
{
    [SerializeField] public float lifeTime ;
    private float timer;
    private bool isActive = true;
    [SerializeField] private Color originalColor;
    [SerializeField] private GameObject halo;
    [SerializeField] private SpriteRenderer sr;
    private void Awake()
    {
        if (halo == null)
        {
            Transform haloTransform = transform.Find("Halo");
            if (haloTransform != null)
                halo = haloTransform.gameObject;
        }

        if (sr == null)
        {
            sr = GetComponent<SpriteRenderer>();
        }
    }

    void Start()
    {
        timer = lifeTime;
        Activate(lifeTime, transform.position);
    }

    public void Activate(float newLifeTime, Vector2 newPosition)
    {
        transform.position = newPosition;
        lifeTime = newLifeTime;
        timer = newLifeTime;
        isActive = true;

        if (sr != null)
            sr.color = originalColor;

        if (halo != null)
            halo.SetActive(false);

        StopAllCoroutines();
        gameObject.SetActive(true);
    }

    private void Update()
    {
        if (!isActive) return;

        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            StartCoroutine(Deactivate(false));
            return;
        }

        HandleInput();
    }
    private void HandleInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began && !IsPointerOverUI(touch.fingerId))
            {
                TryHit(Camera.main.ScreenToWorldPoint(touch.position));
            }
        }

/*#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0) && !IsPointerOverUI())
        {
            TryHit(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
#endif*/

    }

    private void TryHit(Vector3 worldPosition)
    {
        Vector2 pos2D = new Vector2(worldPosition.x, worldPosition.y);
        RaycastHit2D hit = Physics2D.Raycast(pos2D, Vector2.zero);

        if (hit.collider != null && hit.collider.gameObject == gameObject)
        {
            StartCoroutine(Deactivate(true));
        }
    }

    private bool IsPointerOverUI(int fingerId = -1)
    {
        if (fingerId >= 0)
            return EventSystem.current.IsPointerOverGameObject(fingerId);
        else
            return EventSystem.current.IsPointerOverGameObject();
    }

    /*void OnMouseDown()
    {
        if (isActive)
        {
            StartCoroutine(Deactivate(true));
        }
    }*/
    
    private System.Collections.IEnumerator Deactivate(bool clicked)
    {
        Debug.Log("Target Deactivated. Clicked: " + clicked);
        isActive = false;
        
        if (clicked)
        {
            ScoreManager score = ScoreManager.Instance;
            if (score != null)
                score.AddScore(10);
            if (halo != null)
                halo.SetActive(true);
            yield return new WaitForSeconds(0.3f);
            if (halo != null)
                halo.SetActive(false);
        }
        else
        {
            sr.color = Color.red;
            yield return new WaitForSeconds(0.3f);
            HealthSystem.Instance.TakeDamage(1);
        }

        gameObject.SetActive(false);
        TargetPool.Instance.Return(this);
        GameEvents.OnTargetCountChanged?.Invoke(-1);
    }

    public void Reset()
    {
        timer = 0f;
        isActive = false;
        
        if (halo != null)
            halo.SetActive(false);

        if (sr != null)
            sr.color = originalColor;
        
        StopAllCoroutines();
    }
}


