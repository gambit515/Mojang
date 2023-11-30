using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Lever : MonoBehaviour
{
    [SerializeField] private Vector3 turnOnRotation;
    [SerializeField] private MovingPlatform[] movingObjects;
    [SerializeField] private Vector3[] endPositions;
    //[SerializeField] private AudioClip machineSound;
    //private List<Vector3> startPositions = new List<Vector3>();
    //private List<AudioSource> audioSources = new();
    //[SerializeField] private float duration;
    //private float elapsedTime = 0.0f;


    private bool isTurnOn;
    private Quaternion turnOffPosition;
    private void Start()
    {
        turnOffPosition = transform.rotation;
        //foreach (MovingPlatform obj in movingObjects)
        //{
        //    startPositions.Add(obj.gameObject.transform.position);
        //}
    }

    public void Switch()
    {
        isTurnOn= !isTurnOn;
        if(isTurnOn)
        {
            //StopEverySound(audioSources);
            transform.rotation = Quaternion.Euler(turnOnRotation);
            for(int i = 0; i < movingObjects.Length; i++)
            {
                //audioSources.Add(new AudioSource());
                //AudioSourceInit(audioSources[i], movingObjects[i].transform.position);
                movingObjects[i].GoToFinish();
                //movingObjects[i].transform.position = endPositions[i];
            }
        }
        else
        {
            transform.rotation = turnOffPosition;
            //StopEverySound(audioSources);
            for (int i = 0; i < movingObjects.Length; i++)
            {

                //audioSources.Add(new AudioSource());
                //AudioSourceInit(audioSources[i], movingObjects[i].transform.position);
                movingObjects[i].GoToStart();
                //movingObjects[i].transform.position = startPositions[i];
            }
        }
    }
    //private void StopEverySound(List<AudioSource> audioSources)
    //{
    //    foreach (AudioSource audioSource in audioSources)
    //    {
    //        audioSource.Stop();
    //    }
    //    audioSources.Clear();
    //}
    //private void AudioSourceInit(AudioSource audioSource,Vector3 position)
    //{
    //    audioSource.transform.position = position;
    //    audioSource.clip = machineSound;
    //    audioSource.Play();
    //}
}
