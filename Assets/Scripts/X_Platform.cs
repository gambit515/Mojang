using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class X_Platform : MonoBehaviour
{
    [SerializeField] private float xBiggetPoint;
    [SerializeField] private float xLessPoint;
    private bool isFinish = false;
    [SerializeField] private float speed;
    private List<GameObject> onPlatform = new List<GameObject> { };

    // Update is called once per frame
    void Update()
    {
        if (!isFinish)
        {
            TransformX(speed);
        }
        else
        {
            TransformX(-speed);
        }
        if (xBiggetPoint < gameObject.transform.position.x || xLessPoint > gameObject.transform.position.x)
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
    private void TransformX(float speed)
    {
        gameObject.transform.position += new Vector3(speed, 0, 0) * Time.deltaTime;
        if (onPlatform != null)
            foreach (GameObject GO in onPlatform)
            {
                GO.transform.position += new Vector3(speed, 0, 0) * Time.deltaTime;
            }
    }
}
