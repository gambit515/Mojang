using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockLogic : MonoBehaviour
{
    [SerializeField] private float time_delay;
    [SerializeField] private AudioClip hitSound;
    private int hits;
    private void Start()
    {

        hits= 0;
    }
    public void RegisterHit()
    {
        Invoke(nameof(GetHit), time_delay);
    }
    private void GetHit()
    {
        hits++;
        AudioSource.PlayClipAtPoint(hitSound, transform.position);
        switch (hits)
        {
            case 1:
                GetComponent<Animator>().SetTrigger("GetHit");
                break;
            case 2:
                gameObject.SetActive(false); 
                break;
        }
    }
}
