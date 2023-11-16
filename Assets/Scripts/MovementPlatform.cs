using UnityEngine;

public class MovementScript : MonoBehaviour
{
    [SerializeField] private Vector3 movementSpeed;

    // Вызывается при соприкосновении с другим коллайдером
    private void OnCollisionStay(Collision collision)
    {
        // Проверяем, соприкасается ли объект с тегом "Player"
        if (collision.gameObject.CompareTag("Player"))
        {
            // Получаем направление движения от текущего положения к позиции объекта "переда"
            Vector3 direction = transform.forward;

            // Нормализуем вектор направления, чтобы изменение скорости было постоянным
            //direction.Normalize();

            // Перемещаем объект в сторону переда
            collision.gameObject.transform.Translate(movementSpeed * Time.deltaTime,Space.World);
        }
    }
}
