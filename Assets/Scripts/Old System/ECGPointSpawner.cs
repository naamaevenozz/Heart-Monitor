using UnityEngine;

public class ECGPointManager : MonoBehaviour
{
    [Header("References")]
    public GameObject pointPrefab;
    public ECGPointTester.Params parameters;

    [Header("Settings")]
    public float beatDuration = 1f; 

    private GameObject currentPoint;
    private float timer;

    void Update()
    {
        if (currentPoint == null)
        {
            SpawnPoint();
        }
        else
        {
            timer += Time.deltaTime;
            if (timer >= beatDuration)
            {
                Destroy(currentPoint);
                currentPoint = null;
            }
        }
    }

    void SpawnPoint()
    {
        Vector3 pos = new Vector3(0, 0, 0); 
        currentPoint = Instantiate(pointPrefab, pos, Quaternion.identity);
        currentPoint.GetComponent<ECGPoint>().parameters = parameters;
        timer = 0f;
    }
}