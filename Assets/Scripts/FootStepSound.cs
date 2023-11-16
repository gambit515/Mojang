using UnityEngine;
using System.Collections.Generic;

public class FootstepSound : MonoBehaviour
{
    public Dictionary<string, AudioClip> footstepSounds; // ������� ��� �������� ������ ������ �� ����� ��������
    public string currentSurface = "default"; // ������� ��������
    [SerializeField] private float footstepInterval = 0.5f; // �������� ����� ������� ������
    [SerializeField] private float raycastDistance = 1.0f;
    private float lastFootstepTime;

    void Start()
    {
        // ������������� ������� � ������ ���������� ������
        footstepSounds = new Dictionary<string, AudioClip>();
        footstepSounds.Add("default", null); // ���� ������ �� ��������� (����� ���� null)
        footstepSounds.Add("PinkWoolMaterial (Instance)", Resources.Load<AudioClip>("Audio/Cloth_dig1"));
        footstepSounds.Add("concrete", Resources.Load<AudioClip>("Footstep_Concrete"));
    }

    void Update()
    {
        CheckSurface();
        
        if (Time.time - lastFootstepTime > footstepInterval && (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0))
        {
            PlayFootstepSound();
            lastFootstepTime = Time.time;
        }
    }

    void PlayFootstepSound()
    {
        // �������� ���� ������ �������� �������� �� �������
        AudioClip currentFootstepSound = footstepSounds[currentSurface];

        // �������� ������� �����
        if (currentFootstepSound != null)
        {
            // �������� ��������� ����� � ��������������� �����
            AudioSource.PlayClipAtPoint(currentFootstepSound, transform.position);
        }
    }
    void CheckSurface()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        RaycastHit hit;

        // ��������� ������������ ����
        if (Physics.Raycast(ray, out hit, raycastDistance))
        {
            // �������� �������� �������, � ������� ���������� ���
            
            if(hit.collider.GetComponent<Renderer>().material.name == "Default-Material (Instance)")
            {
                currentSurface = "default";
            }
            else
            {
                currentSurface = hit.collider.GetComponent<Renderer>().material.name;
            }


            // ������ �� ������ ������������ currentMaterial ��� ����������� ������� �����������
            Debug.Log("Current Surface: " + currentSurface);
        }
    }
}
