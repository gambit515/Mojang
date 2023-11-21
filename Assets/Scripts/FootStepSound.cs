using UnityEngine;
using System.Collections.Generic;

public class FootstepSound : MonoBehaviour
{
    public Dictionary<string, AudioClip> footstepSounds;    // Словарь для хранения звуков ходьбы по типам покрытия
    public string currentSurface = "default";               // Текущее покрытие
    [SerializeField] private float footstepInterval = 0.5f; // Интервал между звуками ходьбы
    [SerializeField] private float raycastDistance = 1.0f;
    private float lastFootstepTime;
    private PlayerController playerController;

    void Start()
    {
        playerController= GetComponent<PlayerController>();
        // Инициализация словаря и пример добавления звуков
        footstepSounds = new Dictionary<string, AudioClip>();
        footstepSounds.Add("default", Resources.Load<AudioClip>("Audio/stone1")); // Звук ходьбы по умолчанию (может быть null)
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
        // Получаем звук ходьбы текущего покрытия из словаря
        AudioClip currentFootstepSound = footstepSounds[currentSurface];

        // Проверка наличия звука
        if (currentFootstepSound != null)
        {
            // Создание источника звука и воспроизведение звука
            AudioSource.PlayClipAtPoint(currentFootstepSound, transform.position);
        }
    }
    void CheckSurface()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        RaycastHit hit;

        // Проверяем столкновение луча
        if (Physics.Raycast(ray, out hit, raycastDistance))
        {
            // Получаем материал объекта, с которым столкнулся луч
            if (footstepSounds.ContainsKey(hit.collider.GetComponent<Renderer>().material.name))
                currentSurface = hit.collider.GetComponent<Renderer>().material.name;
            else currentSurface = "default";

            // Теперь вы можете использовать currentMaterial для определения текущей поверхности
            //Debug.Log("Current Surface: " + currentSurface);
        }
    }
}
