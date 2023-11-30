using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PicaxeScript : MonoBehaviour
{
    public Transform player; // ������ �� ������
    public float moveSpeed = 5.0f; // �������� ����������� ������
    public float returnSpeed = 2.0f; // �������� ����������� � ������

    void Start()
    {
        // ������ ������ ����������� ������
        //MoveCameraToPoint(new Vector3(10.0f, 0.0f, 0.0f));
        transform.position = player.position;
    }

    // ����� ��� ����������� ������ � �����
    public void MoveCameraToPoint(Vector3 targetPosition)
    {
        StartCoroutine(MoveToPosition(targetPosition));
    }

    // �������� ��� �������� ����������� ������ � �����
    IEnumerator MoveToPosition(Vector3 targetPosition)
    {
        float timer = 0f;

        while (Vector3.Distance(transform.position, targetPosition) > 0.01f && timer < 1f)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * moveSpeed);
            timer += Time.deltaTime;
            yield return null;
        }

        // ����� ���������� ����� ��� ��������� �������, ��������� �������� ����������� � ������
        transform.position = player.position;
    }

    // �������� ��� ����������� ������ � ������
}
