using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PicaxeScript : MonoBehaviour
{
    public Transform player; // Ссылка на игрока
    public float moveSpeed = 5.0f; // Скорость перемещения камеры
    public float returnSpeed = 2.0f; // Скорость возвращения к игроку

    void Start()
    {
        // Пример вызова перемещения камеры
        //MoveCameraToPoint(new Vector3(10.0f, 0.0f, 0.0f));
        transform.position = player.position;
    }

    // Метод для перемещения камеры к точке
    public void MoveCameraToPoint(Vector3 targetPosition)
    {
        StartCoroutine(MoveToPosition(targetPosition));
    }

    // Корутина для плавного перемещения камеры к точке
    IEnumerator MoveToPosition(Vector3 targetPosition)
    {
        float timer = 0f;

        while (Vector3.Distance(transform.position, targetPosition) > 0.01f && timer < 1f)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * moveSpeed);
            timer += Time.deltaTime;
            yield return null;
        }

        // После достижения точки или истечения времени, запускаем корутину возвращения к игроку
        transform.position = player.position;
    }

    // Корутина для возвращения камеры к игроку
}
