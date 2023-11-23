using UnityEngine;
using UnityEngine.SceneManagement;
using EasyJoystick;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    //Скорость передвижения игрока
    [SerializeField] private float movementSpeed = 5.0f;
    //Скорость поворота камеры игрока
    [SerializeField] private float rotationSpeed = 2.0f;
    [SerializeField] private float colliderCheckerRay;
    //Дальность луча, который проверяет, что игрок на щемле
    [SerializeField] private float jumpRaycastRange;
    //Сила прыжка игрока
    [SerializeField] private float jumpHeight = 10f;
    [SerializeField] private float timeToJumpApex = 0.5f;
    //Слой, который считается землей
    [SerializeField] LayerMask groundLayer;
    //Джойстик из пакета EasyJoystick, планируется перенос в отдельную надстройку
    [SerializeField] EasyJoystick.Joystick joystick;
    //Чувствительносноть передвижения камеры сенсорным экраном
    [SerializeField] private float touchSensivity;
    //Чувствительность передвижения камеры с помощью мыши
    [SerializeField] private float mouseSensivity;
    //Высота камеры над основным телом объекта
    [SerializeField] private float cameraHeight = 1f;
    [SerializeField] private GameObject[] meshPoints;
    [SerializeField] private RectTransform rightPieceOfScreen;

    [SerializeField] private float rangeBColiderCheckers;

    //private float gravity;
    //private float jumpVelocity;
    //private Vector3 velocity;

    //Сенсорные значения передвижения
    private float sensorXaxis;
    private float sensorYaxis;

    
    private Camera playerCamera;
    private Rigidbody playerRigidbody;

    //Переменная для хранения поворота камеры по x
    private float cameraRotationX = 0f;

    //Флаг, что объект находится на земле
    public bool isGrounded;

    void Start()
    {
        //gravity = -(jumpHeight/6) / Mathf.Pow(timeToJumpApex, 2);
        //jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;

        //Получение компонентов с объекта
        //meshVertices = GetVertices(GetComponent<MeshCollider>());
        playerRigidbody = GetComponent<Rigidbody>();
        playerCamera = GetComponentInChildren<Camera>();

        //Настройки курсора
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
        Vector3 floor = transform.position;
        floor.y = floor.y - cameraHeight;
        isGrounded = Physics.Raycast(floor, Vector3.down, jumpRaycastRange, groundLayer);
        //if (!isGrounded)
        //    ApplyGravity();
        //else
        //    velocity.y= 0f;
       
    }
    //private void ApplyGravity()
    //{
    //    velocity.y += gravity * Time.deltaTime;
    //}
    //Функция которая вызывает прыжок
    public void WannaJump()
    {
        //Debug.Log("transform"+transform.position);
        Vector3 cameraPosition = transform.position; //Точка указывающая текущее положение камеры
        cameraPosition.y += cameraHeight;
        if (isGrounded && !IsObstacle(cameraPosition, Vector3.up * jumpHeight))
        {
            //playerRigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            //Vector3 jumpVector = new(transform.position.x,transform.position.y+jumpHeight, transform.position.z);
            Vector3 jumpVector = new(0, jumpHeight,0);
            playerRigidbody.velocity = jumpVector;
            Invoke(nameof(StopVelocity), 0.2f);
            //transform.position = jumpVector;
            //playerRigidbody.AddForce(jumpVector,ForceMode.Impulse);
        }
            
    }
    private void StopVelocity()
    {
        playerRigidbody.velocity = Vector3.zero;
    }
    bool IsSideClear(Vector3 direction, float range)
    {
        Vector3 highestPosition = new(transform.position.x,transform.position.y+range, transform.position.z);
        Vector3 mediumPosition = transform.position;
        Vector3 shortestPosition = new(transform.position.x, transform.position.y - range, transform.position.z);
        return !IsObstacle(new Vector3[] { highestPosition, mediumPosition, shortestPosition }, direction);
    }
    bool IsObstacle(Vector3[] origins, Vector3 moveDirection)
    {
        foreach (Vector3 origin in origins)
        {
            Ray ray = new Ray(origin, moveDirection);
            float maxDistance = colliderCheckerRay;
            Debug.DrawRay(ray.origin, ray.direction * maxDistance, Color.blue);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, maxDistance))
            {
                Debug.Log("Мне мишает " + hit.collider.name);
                return true;
            }
        }
        return false;
    }
    void MovePlayer()
    {
        
        float horizontal = !SDKLANG.IsMobileDevice ? Input.GetAxis("Horizontal") : joystick.Horizontal();
        float vertical = !SDKLANG.IsMobileDevice ? Input.GetAxis("Vertical") : joystick.Vertical();
            if (vertical > 0)
            {
                if (IsSideClear(transform.forward, rangeBColiderCheckers))
                {
                    Vector3 moveSide = new(0, 0, vertical);
                    transform.Translate(moveSide * movementSpeed * Time.deltaTime);
                }
            }
            if (vertical < 0)
            {
                if (IsSideClear(-transform.forward, rangeBColiderCheckers))
                {
                    Vector3 moveSide = new(0, 0, vertical);
                    transform.Translate(moveSide * movementSpeed * Time.deltaTime);
                }
            }
            if (horizontal > 0)
            {
                if (IsSideClear(transform.right, rangeBColiderCheckers))
                {
                    Vector3 moveSide = new(horizontal, 0, 0);
                    transform.Translate(moveSide * movementSpeed * Time.deltaTime);
                }
            }
            if (horizontal < 0)
            {
                if (IsSideClear(-transform.right, rangeBColiderCheckers))
                {
                    Vector3 moveSide = new(horizontal, 0, 0);
                    transform.Translate(moveSide * movementSpeed * Time.deltaTime);
                }
            }

        //transform.position += velocity * Time.deltaTime;



    }
    //Проверка на отсутствии объекта в сторону перемещения
    bool IsObstacle(Vector3 orgin,Vector3 moveDirection)
    {
        // Рассчитываем луч, направленный вперед от положения персонажа
         Ray ray = new Ray(orgin, moveDirection);
        float maxDistance = colliderCheckerRay; // Максимальная дистанция луча

        //Отображение вектора для отладки
        Debug.DrawRay(ray.origin, ray.direction * maxDistance, Color.blue);

        // Проверяем, есть ли коллизии на пути луча
        RaycastHit hit;
        if (Physics.Raycast(ray,out hit, maxDistance))
        {
            if (hit.collider.gameObject.tag != "Player")
                // Если есть, возвращаем true, что указывает на препятствие
                return true;
        }

        // В противном случае, возвращаем false
        return false;
    }
    void RotatePlayer()
    {
        float mouseX = !SDKLANG.IsMobileDevice ? Input.GetAxis("Mouse X")*mouseSensivity : -sensorXaxis;
        if (!mouseX.Equals(new Vector3(0, 0, 0)))
        {
            Vector3 playerRotation = new(0, mouseX * rotationSpeed *Time.deltaTime, 0);

            // Поворот игрока вокруг оси Y
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
            joystick.TouchUp();
        }
            
    }
    
    //Обработка каждого отдельного касания
    void TouchSwapping(Touch touch)
    {
        //if (touch.position.x > Screen.width / 2)
        if (RectTransformUtility.RectangleContainsScreenPoint(rightPieceOfScreen, touch.position))
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
            joystick.TouchDown(touch);
        }
    }

}
