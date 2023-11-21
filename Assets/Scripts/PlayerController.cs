using UnityEngine;
using UnityEngine.SceneManagement;
using EasyJoystick;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    //�������� ������������ ������
    [SerializeField] private float movementSpeed = 5.0f;
    //�������� �������� ������ ������
    [SerializeField] private float rotationSpeed = 2.0f;
    [SerializeField] private float colliderCheckerRay;
    //��������� ����, ������� ���������, ��� ����� �� �����
    [SerializeField] private float jumpRaycastRange;
    //���� ������ ������
    [SerializeField] private float jumpForce = 10f;
    //����, ������� ��������� ������
    [SerializeField] LayerMask groundLayer;
    //�������� �� ������ EasyJoystick, ����������� ������� � ��������� ����������
    [SerializeField] EasyJoystick.Joystick joystick;
    //������������������ ������������ ������ ��������� �������
    [SerializeField] private float touchSensivity;
    //���������������� ������������ ������ � ������� ����
    [SerializeField] private float mouseSensivity;
    //������ ������ ��� �������� ����� �������
    [SerializeField] private float cameraHeight = 1f;
    [SerializeField] private GameObject[] meshPoints;
    [SerializeField] private RectTransform rightPieceOfScreen;



    //��������� �������� ������������
    private float sensorXaxis;
    private float sensorYaxis;

    
    private Camera playerCamera;
    private Rigidbody playerRigidbody;

    //���������� ��� �������� �������� ������ �� x
    private float cameraRotationX = 0f;

    //����, ��� ������ ��������� �� �����
    public bool isGrounded;

    //private Vector3[] meshVertices;

    void Start()
    {

        //��������� ����������� � �������
        //meshVertices = GetVertices(GetComponent<MeshCollider>());
        playerRigidbody = GetComponent<Rigidbody>();
        playerCamera = GetComponentInChildren<Camera>();

        //��������� �������
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        IsOnGround();
        HandleTouches();
        MovePlayer();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            WannaJump();
        }
    }
    private void FixedUpdate()
    {
        RotatePlayer();
        RotateCamera();
        CameraUpdate();
    }
    private void CameraUpdate()
    {
        Vector3 cameraVector = transform.position;
        cameraVector.y = transform.position.y + cameraHeight;
        playerCamera.transform.position = cameraVector;
    }
    //����� ������ � ��� ������� �� ������ ����� ������
    void IsOnGround()
    {
        Vector3 floor = transform.position;
        floor.y = floor.y - cameraHeight;
        isGrounded = Physics.Raycast(floor, Vector3.down, jumpRaycastRange, groundLayer);
       
    }
    //������� ������� �������� ������
    public void WannaJump()
    {
        //Debug.Log("transform"+transform.position);
        Vector3 cameraPosition = transform.position; //����� ����������� ������� ��������� ������
        cameraPosition.y += cameraHeight;
        if (isGrounded && !IsObstacle(cameraPosition, Vector3.up * jumpForce))
            playerRigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    void MovePlayer()
    {
        
        float horizontal = !SDKLANG.IsMobileDevice ? Input.GetAxis("Horizontal"): joystick.Horizontal();
        float vertical = !SDKLANG.IsMobileDevice ? Input.GetAxis("Vertical"): joystick.Vertical();

        Vector3 moveDirection = new(horizontal, 0, vertical);
        

        if (!moveDirection.Equals(new Vector3(0, 0, 0)))
        {
            Vector3 moveLocalDirection = transform.TransformDirection(moveDirection);
            Vector3 cameraPosition = transform.position; //����� ����������� ������� ��������� ������
            cameraPosition.y += cameraHeight;
            // �������� ��������� ������ ����� ����� �������, ����� ������� � ����� ��������� ����
            if (!IsObstacle(meshPoints, moveLocalDirection) && !IsObstacle(cameraPosition, moveLocalDirection))
            {
                transform.Translate(moveDirection * movementSpeed * Time.deltaTime);
            }
        }
            
    }
    //�������� �� ���������� ������� � ������� �����������
    bool IsObstacle(Vector3 orgin,Vector3 moveDirection)
    {
        // ������������ ���, ������������ ������ �� ��������� ���������
         Ray ray = new Ray(orgin, moveDirection);
        float maxDistance = colliderCheckerRay; // ������������ ��������� ����

        //����������� ������� ��� �������
        Debug.DrawRay(ray.origin, ray.direction * maxDistance, Color.blue);

        // ���������, ���� �� �������� �� ���� ����
        RaycastHit hit;
        if (Physics.Raycast(ray,out hit, maxDistance))
        {
            if (hit.collider.gameObject.tag != "Player")
                // ���� ����, ���������� true, ��� ��������� �� �����������
                return true;
        }

        // � ��������� ������, ���������� false
        return false;
    }
    bool IsObstacle(GameObject[] origins, Vector3 moveDirection)
    {
        foreach(GameObject origin in origins)
        {
            Vector3 origin2 = origin.transform.position;
            Ray ray = new Ray(origin2, moveDirection);
            float maxDistance = colliderCheckerRay;
            Debug.DrawRay(ray.origin, ray.direction * maxDistance, Color.blue);
            RaycastHit hit;
            if (Physics.Raycast(ray,out hit,maxDistance))
            {
                if(hit.collider.gameObject.tag != "Player")
                    return true;
            }
        }
        return false;
    }
    void RotatePlayer()
    {
        float mouseX = !SDKLANG.IsMobileDevice ? Input.GetAxis("Mouse X")*mouseSensivity : -sensorXaxis;
        if (!mouseX.Equals(new Vector3(0, 0, 0)))
        {
            Vector3 playerRotation = new(0, mouseX * rotationSpeed *Time.deltaTime, 0);

            // ������� ������ ������ ��� Y
            playerRigidbody.MoveRotation(playerRigidbody.rotation * Quaternion.Euler(playerRotation));
        }
    }

    void RotateCamera()
    {
        float mouseY = !SDKLANG.IsMobileDevice ? Input.GetAxis("Mouse Y")*mouseSensivity : -sensorYaxis;
        //float mouseY = Input.GetAxis("Mouse Y");
        if(!mouseY.Equals(new Vector3(0, 0, 0)))
        {
            cameraRotationX -= mouseY * rotationSpeed * Time.deltaTime;
            cameraRotationX = Mathf.Clamp(cameraRotationX, -80f, 80f); // ����������� ������������� ���� ������

            // ������� ������ ������ ��� X
            playerCamera.transform.localRotation = Quaternion.Euler(cameraRotationX, 0, 0);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Respawn":
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                break;
            case "Finish":
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                break;
        }
    }
    //��������� ������� ��� �������� ������/�������� �������
    void HandleTouches()
    {
        sensorXaxis = 0;
        sensorYaxis = 0;
        if (Input.touchCount > 0)
        {
            foreach(Touch touch in Input.touches)
            {
                TouchSwapping(touch);
            }
        }
        else
        {
            joystick.TouchUp();
        }
            
    }
    
    //��������� ������� ���������� �������
    void TouchSwapping(Touch touch)
    {
        //if (touch.position.x > Screen.width / 2)
        if (RectTransformUtility.RectangleContainsScreenPoint(rightPieceOfScreen, touch.position))
            switch (touch.phase)
            {
                case TouchPhase.Moved:
                    // �������� ������������� �������� ���������� �����
                    float horizontalDelta = touch.deltaPosition.x / Screen.width * touchSensivity;
                    float verticalDelta = touch.deltaPosition.y / Screen.height * touchSensivity;

                    // ����������� �������� � ���� �����
                    sensorXaxis = Input.GetAxis("Horizontal") + horizontalDelta;
                    sensorYaxis = Input.GetAxis("Vertical") + verticalDelta;
                    break;
            }
        else
        {
            joystick.TouchDown(touch);
        }
    }

}
