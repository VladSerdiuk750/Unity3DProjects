using System;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof (Rigidbody))]
public class CharacterController : Character
{
    private Rigidbody _rigidbody;//кэшированный компонент Rigidbody (чтобы не создавать каждый раз, когда обращаемся)

    private Animator animator;

    private float currentCameraY;

    [SerializeField]
    private Transform RightHandPlace;

    [SerializeField] private Transform LeftHandPlace;
    
    [SerializeField]
    private float cameraTurnSpeed;

    [SerializeField]
    private float maxSpeed; 

	[SerializeField]
    private float movingForce = 20.0f;  //сила для передвижения

    [SerializeField]
    private float jumpForce = 80f;  //сила прыжка

    [SerializeField]
    private float maxSlope = 30f;   //Максимальный уклон, по которому может идти персонаж

    [SerializeField]
    private bool onGround = false;  //Стоит ли персонаж на подходящей поверхности (или летит/падает)

	[SerializeField]
	private float damping = 0.3f;

    //Инициализация объекта
    void Awake ()
    {
        _rigidbody = gameObject.GetComponent<Rigidbody>();  //Находим и запоминаем (кэшируем) компонент Rigidbody
        animator = gameObject.GetComponent<Animator>();
    }
	
	void Start ()
    {
        //Здесь будет взаимодействие с другими объектами в начале игры, после того, как они уже проинициализировались в их методе Awake () 
        //Например, можно найти инвентарь, и вычесть его вес из скорости перемещения. 
    }
    
    //Коллайдер персонажа прекращает взаимодействие с каким-то другим коллайдером
    private void OnCollisionExit(Collision collision)
    {
        onGround = false;
    }
  
    //Коллайдер персонажа начинает взаимодействие с каким-то другим коллайдером
    private void OnCollisionStay(Collision collision)
    {
        onGround = CheckIsOnGround(collision);
    }

    //Вызывается каждый кадр. Частота может меняться в зависимости от сложности рендеринга и мощности компьтера.
    void Update ()
    {
        MouseLook(); //Поворачиваем персонажа к курсору 
		Shoot();
        UpdateTimer();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        if (Input.GetMouseButtonDown(0))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

	private void Shoot()
    {
        Vector3 shootDirection = transform.forward;
        shootDirection = GetShootDirection(shootDirection, Gun.position);
        if (Input.GetButtonDown("Fire1"))
        {
            ShootBullet(shootDirection);
        }
        if (Input.GetButtonDown("Fire2"))
        {
            ShootRocket(shootDirection);
        }
    }


    //Вызывается каждый шаг просчета физики, вне зависимости от FPS (вне зависимости от скорости рендеринга)
    void FixedUpdate()
    {
        animator.SetFloat("vSpeed", Input.GetAxis("Vertical"));
        animator.SetFloat("hSpeed", Input.GetAxis("Horizontal"));


        if (onGround) //если стоим на земле
        {
            animator.applyRootMotion = true;

            if (Input.GetKeyDown(KeyCode.Space)) //Если игрок нажал "пробел"
            {
                animator.SetTrigger("jump");
            }

            animator.SetBool("inAir", false);
        }
        else
        {
            transform.Translate(new Vector3(Input.GetAxis("Horizontal") * 0.1f, 0, 
                Input.GetAxis("Vertical") * 0.1f));

            animator.applyRootMotion = false;
            animator.SetBool("inAir", true);
        }
    }

    void ApplyJumpForce()
    {
        _rigidbody.AddForce(Vector3.up *
                            jumpForce); //прикладываем к Rigidbody силу, направленную вверх, и имеющую величину, равную jumpForce.
    }

    void MouseLook()
    {
        transform.RotateAround(transform.position, Vector3.up, Input.GetAxis("Mouse X") * 
                                                               cameraTurnSpeed);

        Vector3 cameraPosition = transform.position - (transform.forward * 3);
        cameraPosition.y += 2.5f;
        
        Vector3 cameraTargetPosition = transform.position = transform.position;
        cameraTargetPosition.y += 2f;

        currentCameraY -= Input.GetAxis("Mouse Y") * 0.3f;
        currentCameraY = Mathf.Clamp(currentCameraY, -3, 3);
        cameraPosition.y += currentCameraY;
        
        Quaternion cameraRotation = Quaternion.LookRotation(cameraTargetPosition - cameraPosition);

        float smoothing = 0.5f;

        Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, cameraPosition, smoothing);
        Camera.main.transform.rotation = Quaternion.Lerp(Camera.main.transform.rotation, cameraRotation, smoothing);
        Gun.parent.parent.rotation = cameraRotation;
    }

    // Проверяем, подходит ли поверхность коллайдера для того, чтобы персонаж на ней стоял.
    //Объект Collision для проверки.
    //return true, если поверхность подходящая, false - если нет.
    private bool CheckIsOnGround(Collision collision)
    {
        for (int i = 0; i < collision.contacts.Length; i++) //Проверяем все точки соприкосновения
        {
            if (collision.contacts[i].point.y < transform.position.y)   //если точка соприкосновения находится ниже центра нашего персонажа
            {
                if (Vector3.Angle(collision.contacts[i].normal, Vector3.up) < maxSlope)   //Если уклон поверхности не превышает допустимое значение
                {
                    return true;    //найдена точка соприкосновения с подходящей поверхностью - выходим из функции, возвращаем значение true.
                }
            }
        }
        return false;   //Подходящая поверхность не найдена, возвращаем значение false.
    }

    // Рассчитываем и прикладываем силу перемещения персонажа в зависимости от значений осей инпута
    private void ApplyMovingForce()
    {
        //При рассчете силы по той или иной оси используются локальные оси персонажа. Transform автоматически предоставляет вектора, соответствующие его текущей оси Z и оси X (и оси Y тоже):
        Vector3 xAxisForce = transform.right * Input.GetAxis("Horizontal"); //определяем силу по оси Х
        Vector3 zAxisForce = transform.forward * Input.GetAxis("Vertical"); //определяем силу по оси Z

        Vector3 resultXZForce = xAxisForce + zAxisForce;    //Складываем вектора

		//Если сложить два перпендикулярных вектора, каждый длиной 1, 
		//получится вектор длиной примерно 1,41... (квадратный корень из двух).
		//То есть персонаж будет быстрее бегать по диагонали, чем строго по одной из осей.
		//Чтобы этого не было, нормализуем результирующий вектор (установим его длину равной 1):
		resultXZForce.Normalize();

		resultXZForce *= movingForce; //умножаем результирующий вектор на силу движения персонажа (задаем скорость)


		if (resultXZForce.magnitude > 0)
		{
			_rigidbody.AddForce(resultXZForce); //Прикладываем силу к Rigidbody

		}
		else
		{
			Vector3 dampedVelocity = _rigidbody.velocity * damping;
			dampedVelocity.y = _rigidbody.velocity.y;
			_rigidbody.velocity = dampedVelocity;
		}
    }

    /// Разворачиваем персонажа лицом к курсору
    private void LookAtTarget()
    {
        Plane plane = new Plane(Vector3.up, transform.position);

        float distance;

        //Находим главную камеру, и с ее помощью получаем луч, идущий из камеры в ту точку пространства, которая находится под курсором мыши.
        // Input.mousePosition - текущее положение курсора в пространстве экрана (нижний левый угол - 0, 0; верхний правый угол - ширина окна, высота окна)
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);    

        //С помощью физического движка запускаем полученный луч
        if (plane.Raycast(ray, out distance))  //если луч попал в какой-то коллайдер, метод возвращает true, и выводит параметры столкновения в переменную hit (ключевое слово out)
        {
            Vector3 position = ray.GetPoint(distance);  //Находим на луче точку, находящуюся на заданном расстоянии от начала луча. Это расстояние берем из параметров столкновения - переменной hit. 
            position.y = transform.position.y;  //Поскольку точка столкновения может находиться выше или ниже персонажа, приравниваем ее координату Y к координате Y персонажа.
            transform.LookAt(position); //Поворачиваем трансформ персонажа локальной осью Z (forward) в сторону точки столкновения луча с коллайдером.
        }
    }

    void Destroyed()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    Vector3 GetShootDirection(Vector3 shootDirection, Vector3 gunPosition)
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            Vector3 targetVector = hit.point - gunPosition;
            if (Vector3.Angle(shootDirection, targetVector) < 45)
            {
                shootDirection = targetVector;
            }
        }
        return shootDirection;
    }


    private void OnAnimatorIK(int layerIndex)
    {
        ///Left Hand:
        
        animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
        animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);
        
        
        animator.SetIKPosition(AvatarIKGoal.LeftHand, LeftHandPlace.position);
        animator.SetIKRotation(AvatarIKGoal.LeftHand, LeftHandPlace.rotation);
        
        ///Right Hand:
        
        animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
        animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);
        
        animator.SetIKPosition(AvatarIKGoal.RightHand, RightHandPlace.position);
        animator.SetIKRotation(AvatarIKGoal.RightHand, RightHandPlace.rotation);
        
        ///Head:
        
        animator.SetLookAtWeight(1);
        
        animator.SetLookAtPosition(Gun.position + (Gun.forward * 10));
    }
}
