using UnityEngine;

public class ECGPointController : MonoBehaviour
{
    public float moveStep = 0.2f; 
    public float minY = -4.5f;   
    public float maxY = 4.5f;    

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Move(Vector3.up);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Move(Vector3.down);
        }

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began) 
            {
                if (touch.position.y > Screen.height / 2f)
                {
                    Move(Vector3.up);
                }
                else
                {
                    Move(Vector3.down);
                }
            }
        }
    }

    void Move(Vector3 direction)
    {
        transform.position += direction * moveStep;

        if (transform.position.y > maxY)
        {
            transform.position = new Vector3(transform.position.x, minY, transform.position.z);
        }
        else if (transform.position.y < minY)
        {
            transform.position = new Vector3(transform.position.x, maxY, transform.position.z);
        }
    }
}