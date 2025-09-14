using UnityEngine;

public class ECGPoint : MonoBehaviour
{
    public ECGPointTester.Params parameters;
    private SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        float time = Time.time;
        Vector2 pos = transform.position;

        float waveY = ECGPointTester.WaveValue(new Vector2(pos.x, 0), time, parameters);

        float dist = Mathf.Abs(pos.y - waveY);

        float score = Mathf.Max(0, 1f - dist * 5f);

        sr.color = Color.Lerp(Color.red, Color.green, score);
    }
}