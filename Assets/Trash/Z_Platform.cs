using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Z_Platform : MonoBehaviour
{
    [SerializeField] private float zBiggetPoint;
    [SerializeField] private float zLessPoint;
    private bool isFinish = false;
    [SerializeField] private float speed;
    private List<GameObject> onPlatform = new List<GameObject> { };

    // Update is called once per frame
    void Update()
    {
        if (!isFinish)
        {
            TransformZ(speed);
        }
        else
        {
            TransformZ(-speed);
        }
        if (zBiggetPoint < gameObject.transform.position.z || zLessPoint > gameObject.transform.position.z)
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
    private void TransformZ(float speed)
    {
        gameObject.transform.position += new Vector3(0, 0, speed) * Time.deltaTime;
        if (onPlatform != null)
            foreach (GameObject GO in onPlatform)
            {
                GO.transform.position += new Vector3(0, 0, speed) * Time.deltaTime;
            }
    }
}
