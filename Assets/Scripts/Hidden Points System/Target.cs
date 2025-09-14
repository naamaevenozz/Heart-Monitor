using Hidden_Points_System;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.Analytics;

public class Target : MonoBehaviour
{
    [SerializeField] public float lifeTime ;
    private float timer;
    private bool isActive;
    [SerializeField] private Color originalColor;

    
    [SerializeField] private SpriteRenderer sr;

    void Start()
    {
        timer = lifeTime;
    }

    public void Activate(float newLifeTime, Vector2 newPosition)
    {
        transform.position = newPosition;
        lifeTime = newLifeTime;
        timer = newLifeTime;
        isActive = true;
        sr.color = originalColor;
        gameObject.SetActive(true);
    }

    void Update()
    {
        if (!isActive) return;
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            StartCoroutine(Deactivate(false));
        }
    }

    void OnMouseDown()
    {
        if (isActive)
        {
            StartCoroutine(Deactivate(true));
        }
    }
    
    private System.Collections.IEnumerator Deactivate(bool clicked)
    {
        isActive = false;

        sr.color = clicked ? Color.red : Color.black;
        yield return new WaitForSeconds(0.3f);

        gameObject.SetActive(false);

        TargetPool.Instance.ReturnToPool(this);
    }
}


