using System.Collections.Generic;
using System;
using UnityEngine;
using Unity.Collections.LowLevel.Unsafe;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private Vector3 StartVector;
    [SerializeField] private Vector3 FinishVector;
    private bool isFinish = false;
    private Vector3 speed = new Vector3();
    [SerializeField] private float duration;
    private List<GameObject> onPlatform = new List<GameObject> { };


    private bool IsFirstXBigger;
    private bool IsFirstYBigger;
    private bool IsFirstZBigger;

    private float elapsedTime = 0;

    // Update is called once per frame
    private void Start()
    {
        IsFirstXBigger = StartVector.x > FinishVector.x ? true : false;
        IsFirstYBigger = StartVector.y > FinishVector.y ? true : false;
        IsFirstZBigger = StartVector.z > FinishVector.z ? true : false;


        /*speed.x = (Math.Abs(StartVector.x) - Math.Abs(FinishVector.x)) / duration;
        speed.y = (Math.Abs(StartVector.y) - Math.Abs(FinishVector.y)) / duration;
        speed.z = (Math.Abs(StartVector.z) - Math.Abs(FinishVector.z)) / duration;*/

        speed.x = (StartVector.x - FinishVector.x) / duration;
        speed.y = (StartVector.y - FinishVector.y) / duration;
        speed.z = (StartVector.z - FinishVector.z) / duration;
    }
    void Update()
    {
        /*float t = Mathf.Clamp01(elapsedTime / duration);
        if (t >= 1f)
        {
            isFinish = !isFinish;
            elapsedTime = 0f;
        }*/


        if (!isFinish)
        {
            TransformVector(-speed);
        }
        else
        {
            TransformVector(speed);
        }

        if (PathIsComplete())
            isFinish = !isFinish;
        //lapsedTime += Time.deltaTime;

    }
    private bool Difference(float a, float b)
    {
        if (a - b > 0.1)
            return true;
        else if(a-b < -0.1)
            return true;
        return false;
    }
    private bool PathIsComplete()
    {
        if(!isFinish)
        {
            if (IsFirstXBigger)
            {
                if (transform.position.x < FinishVector.x && Difference(transform.position.x,FinishVector.x))
                    return true;
            }
            else
            {
                if(transform.position.x > FinishVector.x && Difference(transform.position.x, FinishVector.x))
                {
                    return true;
                }
            }
            if (IsFirstYBigger)
            {
                if (transform.position.y < FinishVector.y && Difference(transform.position.y, FinishVector.y))
                    return true;
            }
            else
            {
                if (transform.position.y > FinishVector.y && Difference(transform.position.y, FinishVector.y))
                {
                    return true;
                }
            }
            if (IsFirstZBigger)
            {
                if (transform.position.z < FinishVector.z && Difference(transform.position.z, FinishVector.z))
                    return true;
            }
            else
            {
                if (transform.position.z > FinishVector.z && Difference(transform.position.z, FinishVector.z))
                {
                    return true;
                }
            }
        }
        else
        {
            if (IsFirstXBigger)
            {
                if (transform.position.x > StartVector.x && Difference(transform.position.x, StartVector.x))
                    return true;
            }
            else
            {
                if (transform.position.x < StartVector.x && Difference(transform.position.x, StartVector.x))
                {
                    return true;
                }
            }
            if (IsFirstYBigger)
            {
                if (transform.position.y > StartVector.y && Difference(transform.position.y, StartVector.y))
                    return true;
            }
            else
            {
                if (transform.position.y < StartVector.y && Difference(transform.position.y, StartVector.y))
                {
                    return true;
                }
            }
            if (IsFirstZBigger)
            {
                if (transform.position.z > StartVector.z && Difference(transform.position.z, StartVector.z))
                    return true;
            }
            else
            {
                if (transform.position.z < StartVector.z && Difference(transform.position.z, StartVector.z))
                {
                     return true;
                }
            }
        }
        return false;
    }
    private void OnCollisionEnter(Collision collision)
    {
        onPlatform.Add(collision.gameObject);
    }
    private void OnCollisionExit(Collision collision)
    {
        onPlatform.Remove(collision.gameObject);
    }
    private void TransformVector(Vector3 speed)
    {
        gameObject.transform.position += speed * Time.deltaTime;
        if (onPlatform != null)
            foreach (GameObject GO in onPlatform)
            {
                GO.transform.position += speed * Time.deltaTime;
            }
    }
}
