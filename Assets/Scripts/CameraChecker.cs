using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraChecker : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float cameraDistance;
    [SerializeField] private GameObject MiningButton;
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (IsGold(transform.position, transform.forward, cameraDistance))
        {
            MiningButton.SetActive(true);
        }
        else
            MiningButton.SetActive(false);
    }
    bool IsGold(Vector3 orgin, Vector3 moveDirection,float maxDistance)
    {
        // ������������ ���, ������������ ������ �� ��������� ���������
        Ray ray = new Ray(orgin, moveDirection);
      
        //����������� ������� ��� �������
        Debug.DrawRay(ray.origin, ray.direction * maxDistance, Color.blue);

        // ���������, ���� �� �������� �� ���� ����
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, maxDistance))
        {
            if (hit.collider.gameObject.tag == "Gold" )
                // ���� ����, ���������� true, ��� ��������� �� �����������
                return true;
        }

        // � ��������� ������, ���������� false
        return false;
    }
}
