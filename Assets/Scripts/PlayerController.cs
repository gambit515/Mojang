using UnityEngine;
using UnityEngine.SceneManagement;
using EasyJoystick;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 5.0f;
    [SerializeField] private float rotationSpeed = 2.0f;
    [SerializeField] private float jumpRaycastRange;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] EasyJoystick.Joystick joystick;
    [SerializeField] private float touchSensivity;
    [SerializeField] private float mouseSensivity;
    [SerializeField] private float cameraHeight = 1f;

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
    //Откат прыжка и при нажатии на пробел вызов прыжка
    void IsOnGround()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, jumpRaycastRange, groundLayer);
       
    }
    //Функция которая вызывает прыжок
    public void WannaJump()
    {
        
        Vector3 cameraPosition = transform.position; //Точка указывающая текущее положение камеры
        cameraPosition.y += cameraHeight;
        if (isGrounded && !IsObstacle(cameraPosition, Vector3.up * jumpForce))
            playerRigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    void MovePlayer()
    {
        float horizontal = Input.GetAxis("Horizontal") != 0 ? Input.GetAxis("Horizontal"): joystick.Horizontal();
        float vertical = Input.GetAxis("Vertical") != 0 ? Input.GetAxis("Vertical"): joystick.Vertical();

        Vector3 moveDirection = new(horizontal, 0, vertical);
        

        if (!moveDirection.Equals(new Vector3(0, 0, 0)))
        {
            Vector3 moveLocalDirection = transform.TransformDirection(moveDirection);
            Vector3 cameraPosition = transform.position; //Точка указывающая текущее положение камеры
            cameraPosition.y += cameraHeight;
            // Проверка отсутсвия блоков перед двумя точками, перед камерой и перед серединой тела
            if (!IsObstacle(transform.position,moveLocalDirection) && !IsObstacle(cameraPosition, moveLocalDirection))
            {
                transform.Translate(moveDirection * movementSpeed * Time.deltaTime);
            }
        }
            
    }
    //Проверка на отсутствии объекта в сторону перемещения
    bool IsObstacle(Vector3 orgin,Vector3 moveDirection)
    {
        // Рассчитываем луч, направленный вперед от положения персонажа
         Ray ray = new Ray(orgin, moveDirection);
        float maxDistance = 0.75f; // Максимальная дистанция луча

        //Отображение вектора для отладки
        Debug.DrawRay(ray.origin, ray.direction * maxDistance, Color.blue);

        // Проверяем, есть ли коллизии на пути луча
        if (Physics.Raycast(ray, maxDistance))
        {
            // Если есть, возвращаем true, что указывает на препятствие
            return true;
        }

        // В противном случае, возвращаем false
        return false;
    }
    void RotatePlayer()
    {
        float mouseX = Input.GetAxis("Mouse X") != 0 ? Input.GetAxis("Mouse X")*mouseSensivity : sensorXaxis;
        if (!mouseX.Equals(new Vector3(0, 0, 0)))
        {
            Vector3 playerRotation = new(0, mouseX * rotationSpeed *Time.deltaTime, 0);

            // Поворот игрока вокруг оси Y
            playerRigidbody.MoveRotation(playerRigidbody.rotation * Quaternion.Euler(playerRotation));
        }
    }

    void RotateCamera()
    {
        float mouseY = Input.GetAxis("Mouse Y") != 0 ? Input.GetAxis("Mouse Y")*mouseSensivity : sensorYaxis;
        //float mouseY = Input.GetAxis("Mouse Y");
        if(!mouseY.Equals(new Vector3(0, 0, 0)))
        {
            cameraRotationX -= mouseY * rotationSpeed * Time.deltaTime;
            cameraRotationX = Mathf.Clamp(cameraRotationX, -80f, 80f); // Ограничение вертикального угла обзора

            // Поворот камеры вокруг оси X
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
    //Обработка касаний для вразения камеры/вращения объекта
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
            joystick.OnPointerUp(new PointerEventData(EventSystem.current));
            joystick.gameObject.SetActive(false);
        }
            
    }
    //Обработка каждого отдельного касания
    void TouchSwapping(Touch touch)
    {
        if (touch.position.x > Screen.width / 2)
            switch (touch.phase)
            {
                case TouchPhase.Moved:
                    // Получаем относительное смещение сенсорного ввода
                    float horizontalDelta = touch.deltaPosition.x / Screen.width * touchSensivity;
                    float verticalDelta = touch.deltaPosition.y / Screen.height * touchSensivity;

                    // Привязываем смещение к осям ввода
                    sensorXaxis = Input.GetAxis("Horizontal") + horizontalDelta;
                    sensorYaxis = Input.GetAxis("Vertical") + verticalDelta;
                    break;
            }
        else
        {
            if (!joystick.gameObject.activeSelf)
            {
                joystick.gameObject.SetActive(true);
                joystick.transform.position = new Vector3(touch.position.x, touch.position.y);
                
            }
            PointerEventData eventData = new PointerEventData(EventSystem.current);
            eventData.position = touch.position;
            joystick.OnPointerDown(eventData);
        }
    }

}
