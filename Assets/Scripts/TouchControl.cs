using UnityEngine;

public class TouchControl : MonoBehaviour
{
    public float moveSpeed = 5f;

    void Update()
    {
        HandleTouches();
    }

    void HandleTouches()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Moved:
                    // �������� ������������� �������� ���������� �����
                    float horizontalDelta = touch.deltaPosition.x / Screen.width;
                    float verticalDelta = touch.deltaPosition.y / Screen.height;

                    // ����������� �������� � ���� �����
                    float horizontalInput = Input.GetAxis("Horizontal") + horizontalDelta;
                    float verticalInput = Input.GetAxis("Vertical") + verticalDelta;

                    // ��������� ���� � �������� �������
                    Vector3 movement = new(horizontalInput, 0, verticalInput);
                    transform.Translate(movement * moveSpeed * Time.deltaTime);
                    break;
            }
        }
    }
}
