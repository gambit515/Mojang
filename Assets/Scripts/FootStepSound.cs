using UnityEngine;
using System.Collections.Generic;

public class FootstepSound : MonoBehaviour
{
    public Dictionary<string, AudioClip> footstepSounds;    // ������� ��� �������� ������ ������ �� ����� ��������
    public string currentSurface = "default";               // ������� ��������
    [SerializeField] private float footstepInterval = 0.5f; // �������� ����� ������� ������
    [SerializeField] private float raycastDistance = 1.0f;
    private float lastFootstepTime;
    private PlayerController playerController;

    void Start()
    {
        playerController= GetComponent<PlayerController>();
        // ������������� ������� � ������ ���������� ������
        footstepSounds = new Dictionary<string, AudioClip>();
        footstepSounds.Add("default", Resources.Load<AudioClip>("Audio/stone1")); // ���� ������ �� ��������� (����� ���� null)
        footstepSounds.Add("PinkWoolMaterial (Instance)", Resources.Load<AudioClip>("Audio/Cloth_dig1"));
    }

    void Update()
    {
        CheckSurface();
        
        if (Time.time - lastFootstepTime > footstepInterval && (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0) && playerController.isGrounded)
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
            if (footstepSounds.ContainsKey(hit.collider.GetComponent<Renderer>().material.name))
                currentSurface = hit.collider.GetComponent<Renderer>().material.name;
            else currentSurface = "default";

            // ������ �� ������ ������������ currentMaterial ��� ����������� ������� �����������
            //Debug.Log("Current Surface: " + currentSurface);
        }
    }
}
