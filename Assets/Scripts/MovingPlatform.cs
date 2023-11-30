using System.Collections.Generic;
using System;
using UnityEngine;
using Unity.Collections.LowLevel.Unsafe;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private Vector3 StartVector;
    [SerializeField] private Vector3 FinishVector;
    private bool isFinish = false;
    private Vector3 speed = new();
    [SerializeField] private float duration;
    private List<GameObject> onPlatform = new() { };
    [SerializeField] private bool automaticallyPlatfrom;
    private AudioSource audioSource;


    private bool goToFinish;
    private bool goToStart;

    private bool IsFirstXBigger;
    private bool IsFirstYBigger;
    private bool IsFirstZBigger;
    private void Awake()
    {
        transform.position = StartVector;
    }

    // Update is called once per frame
    private void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = Resources.Load<AudioClip>("Audio/Machine");

        goToFinish = false;
        goToStart= false;

        IsFirstXBigger = StartVector.x > FinishVector.x;
        IsFirstYBigger = StartVector.y > FinishVector.y;
        IsFirstZBigger = StartVector.z > FinishVector.z;

        speed.x = (StartVector.x - FinishVector.x) / duration;
        speed.y = (StartVector.y - FinishVector.y) / duration;
        speed.z = (StartVector.z - FinishVector.z) / duration;
    }
    public void GoToFinish()
    {
        goToFinish= true;
        goToStart= false;
        audioSource.Play();
    }
    public void GoToStart()
    {
        goToStart= true;
        goToFinish = false;
        audioSource.Play();
    }
    void Update()
    {
        if(automaticallyPlatfrom)
        {
            if (!isFinish)
            {
                TransformVector(-speed);
            }
            else
            {
                TransformVector(speed);
            }

            if (PathIsComplete(isFinish,FinishVector,StartVector))
                isFinish = !isFinish;
        }
        else
        {
            if (goToStart)
            {
                TransformVector(speed);
                if (PathIsComplete(goToStart, FinishVector, StartVector))
                {
                    goToStart = false;
                    transform.position = StartVector;
                    audioSource.Stop();
                }
                    
            }
            if(goToFinish)
            {
                TransformVector(-speed);
                if (PathIsComplete(!goToFinish, FinishVector,StartVector ))
                {
                    goToFinish = false;
                    transform.position = FinishVector;
                    audioSource.Stop();
                }
            }
        }
        

        
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
    private bool PathIsComplete(bool isFinish, Vector3 FinishVector, Vector3 StartVector)
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
