using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Y_Platform : MonoBehaviour
{
    [SerializeField] private float yBiggetPoint;
    [SerializeField] private float yLessPoint;
    private bool isFinish = false;
    [SerializeField] private float speed;
    private List<GameObject> onPlatform = new List<GameObject> { };

    // Update is called once per frame
    void Update()
    {
        if (!isFinish)
        {
            TransformY(speed);
        }
        else
        {
            TransformY(-speed);
        }
        if (yBiggetPoint < gameObject.transform.position.y || yLessPoint > gameObject.transform.position.y)
            isFinish = !isFinish;

    }
    private void OnCollisionEnter(Collision collision)
    {
        onPlatform.Add(collision.gameObject);
    }
    private void OnCollisionExit(Collision collision)
    {
        onPlatform.Remove(collision.gameObject);
    }
    private void TransformY(float speed)
    {
        gameObject.transform.position += new Vector3(0, speed, 0) * Time.deltaTime;
        if (onPlatform != null)
            foreach (GameObject GO in onPlatform)
            {
                GO.transform.position += new Vector3(0, speed, 0) * Time.deltaTime;
            }
    }
}
