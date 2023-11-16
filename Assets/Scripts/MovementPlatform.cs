using UnityEngine;

public class MovementScript : MonoBehaviour
{
    [SerializeField] private Vector3 movementSpeed;

    // ���������� ��� ��������������� � ������ �����������
    private void OnCollisionStay(Collision collision)
    {
        // ���������, ������������� �� ������ � ����� "Player"
        if (collision.gameObject.CompareTag("Player"))
        {
            // �������� ����������� �������� �� �������� ��������� � ������� ������� "������"
            Vector3 direction = transform.forward;

            // ����������� ������ �����������, ����� ��������� �������� ���� ����������
            //direction.Normalize();

            // ���������� ������ � ������� ������
            collision.gameObject.transform.Translate(movementSpeed * Time.deltaTime,Space.World);
        }
    }
}
