using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float movementSpeed = 5.0f;
    public float rotationSpeed = 2.0f;
    [SerializeField] private float RaycastRange;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] LayerMask groundLayer;

    private Camera playerCamera;
    private Rigidbody playerRigidbody;
    private float cameraRotationX = 0f;
    private bool isGrounded;

    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        playerCamera = GetComponentInChildren<Camera>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        MovePlayer();
        RotatePlayer();
        RotateCamera();
        Jump();
    }
    void Jump()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, RaycastRange, groundLayer);
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
            playerRigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    void MovePlayer()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 moveDirection = new Vector3(horizontal, 0, vertical);
        //moveDirection.Normalize(); // Нормализация, чтобы движение по диагонали не было быстрее


        // Используем transform.Translate вместо playerRigidbody.MovePosition
        transform.Translate(moveDirection * movementSpeed * Time.deltaTime);
        playerCamera.transform.position = transform.position;
    }

    void RotatePlayer()
    {
        float mouseX = Input.GetAxis("Mouse X");
        Vector3 playerRotation = new Vector3(0, mouseX * rotationSpeed *Time.deltaTime, 0);

        // Поворот игрока вокруг оси Y
        playerRigidbody.MoveRotation(playerRigidbody.rotation * Quaternion.Euler(playerRotation));
    }

    void RotateCamera()
    {
        float mouseY = Input.GetAxis("Mouse Y");
        cameraRotationX -= mouseY * rotationSpeed * Time.deltaTime;
        cameraRotationX = Mathf.Clamp(cameraRotationX, -80f, 80f); // Ограничение вертикального угла обзора

        // Поворот камеры вокруг оси X
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
}
