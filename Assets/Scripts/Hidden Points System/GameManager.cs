using UnityEngine;

namespace Hidden_Points_System
{

    public class GameManager : MonoBehaviour
    {
        // [SerializeField] public Rect spawnArea;
        [SerializeField] float xMin ;
        [SerializeField] float xMax ;
        [SerializeField] float yMin ;
        [SerializeField] float yMax ;
         public float baseLifeTime ;
         public int wave = 1;

        void Start()
        {
            StartWave();
        }

        void StartWave()
        {
            int targetsThisWave = Mathf.Min(1 + wave / 2, 5); 
            // float lifeTime = Mathf.Max(baseLifeTime - (wave * 0.2f), 0.5f);
            float lifeTime = 10f;
            for (int i = 0; i < targetsThisWave; i++)
            {
                SpawnTarget(lifeTime);
            }

            NextWave();
        }

        void SpawnTarget(float lifeTime)
        {
            Target t = TargetPool.Instance.GetTarget();
            if (t != null)
            {
                // Vector2 pos = new Vector2(
                //     Random.Range(spawnArea.xMin, spawnArea.xMax),
                //     Random.Range(spawnArea.yMin, spawnArea.yMax)
                // );
                // Vector2 pos = new Vector2(Random.Range(xBound, yBound), Random.Range(yBound, xBound));
                float x = Random.Range(xMin, xMax);
                float y = Random.Range(yMin, yMax);
                Vector2 pos = new Vector2(x, y);
                t.Activate(lifeTime, pos);
            }
        }

        public void NextWave()
        {
            wave++;
            StartWave();
        }
    }

}