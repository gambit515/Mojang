using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 5.0f;
    [SerializeField] private float rotationSpeed = 2.0f;
    [SerializeField] private float RaycastRange;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] Joystick joystick;
    [SerializeField] private float touchSensivity;

    private float sensorXaxis;
    private float sensorYaxis;
    private Camera playerCamera;
    private Rigidbody playerRigidbody;
    private float cameraRotationX = 0f;
    private bool isGrounded;

    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        playerCamera = GetComponentInChildren<Camera>();
        //Cursor.lockState = CursorLockMode.Locked;
        //�Cursor.visible = false;
    }

    void Update()
    {
        HandleTouches();
        MovePlayer();
        RotatePlayer();
        RotateCamera();
        PrepearingToJump();
        //Debug.Log(Input.GetAxis("Horizontal")+"   "+ Input.GetAxis("Vertical"));
    }
    //����� ������ � ��� ������� �� ������ ����� ������
    void PrepearingToJump()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, RaycastRange, groundLayer);
        if (Input.GetKeyDown(KeyCode.Space))
            Jump();
    }
    //������� ������� �������� ������
    public void Jump()
    {
        if (isGrounded)
            playerRigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    void MovePlayer()
    {

        //float vertical = Input.GetAxis("Vertical");
        //float horizontal = Input.GetAxis("Horizontal");
        float horizontal = Input.GetAxis("Horizontal") != 0 ? Input.GetAxis("Horizontal"): joystick.Horizontal;
        float vertical = Input.GetAxis("Vertical") != 0 ? Input.GetAxis("Vertical"): joystick.Vertical;

        Vector3 moveDirection = new(horizontal, 0, vertical);
        Vector3 moveLocalDirection = transform.TransformDirection(moveDirection);
        if (moveDirection != new Vector3(0,0,0))
            
            if (!IsObstacleInFront(moveLocalDirection))
            {
                transform.Translate(moveDirection * movementSpeed * Time.deltaTime);
                playerCamera.transform.position = transform.position;
            }
    }
    //�������� �� ���������� ������� � ������� �����������
    bool IsObstacleInFront(Vector3 moveDirection)
    {
        // ������������ ���, ������������ ������ �� ��������� ���������
         Ray ray = new Ray(transform.position, moveDirection);
        float maxDistance = 0.75f; // ������������ ��������� ����
        Debug.DrawRay(ray.origin, ray.direction * maxDistance, Color.blue);
        // ���������, ���� �� �������� �� ���� ����
        if (Physics.Raycast(ray, maxDistance))
        {
            // ���� ����, ���������� true, ��� ��������� �� �����������
            return true;
        }

        // � ��������� ������, ���������� false
        return false;
    }
    void RotatePlayer()
    {
        float mouseX = Input.GetAxis("Mouse X") != 0 ? Input.GetAxis("Mouse X") : sensorXaxis;
        //float mouseX = Input.GetAxis("Mouse X");
        Vector3 playerRotation = new(0, mouseX * rotationSpeed *Time.deltaTime, 0);

        // ������� ������ ������ ��� Y
        playerRigidbody.MoveRotation(playerRigidbody.rotation * Quaternion.Euler(playerRotation));
    }

    void RotateCamera()
    {
        float mouseY = Input.GetAxis("Mouse Y") != 0 ? Input.GetAxis("Mouse Y") : sensorYaxis;
        //float mouseY = Input.GetAxis("Mouse Y");
        cameraRotationX -= mouseY * rotationSpeed * Time.deltaTime;
        cameraRotationX = Mathf.Clamp(cameraRotationX, -80f, 80f); // ����������� ������������� ���� ������

        // ������� ������ ������ ��� X
        playerCamera.transform.localRotation = Quaternion.Euler(cameraRotationX, 0, 0);
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
    }
    //��������� ������� ���������� �������
    void TouchSwapping(Touch touch)
    {
        if (touch.position.x > Screen.width / 2)
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
    }

}
