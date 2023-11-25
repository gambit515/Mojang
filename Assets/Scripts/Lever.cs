using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEditor.PackageManager;
using UnityEngine;

public class Lever : MonoBehaviour
{
    [SerializeField] private Vector3 turnOnRotation;
    [SerializeField] private GameObject[] movingObjects;
    [SerializeField] private Vector3[] endPositions;
    private List<Vector3> startPositions = new List<Vector3>();


    private bool isTurnOn;
    private Quaternion turnOffPosition;
    private void Start()
    {
        turnOffPosition = transform.rotation;
        foreach (GameObject obj in movingObjects)
        {
            startPositions.Add(obj.transform.position);
        }
    }

    public void Switch()
    {
        isTurnOn= !isTurnOn;
        if(isTurnOn)
        {
            transform.rotation = Quaternion.Euler(turnOnRotation);
            for(int i = 0; i < movingObjects.Length; i++)
            {
                movingObjects[i].transform.position = endPositions[i];
            }
        }
        else
        {
            transform.rotation = turnOffPosition;
            for(int i = 0; i < movingObjects.Length; i++)
            {
                movingObjects[i].transform.position = startPositions[i];
            }
        }
    }
}
