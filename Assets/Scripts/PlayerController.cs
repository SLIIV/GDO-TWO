using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    /// <summary>
    /// Дистанция перемещения
    /// </summary>
    public float distance = 3;

    /// <summary>
    /// Текущая дистанция перемещения
    /// </summary>
    private float curDistance;
    
    /// <summary>
    /// Компонент аниматора игрока
    /// </summary>
    private Animator playerAnimator;

    /// <summary>
    /// Проверка, двигаемся ли мы сейчас
    /// </summary>
    public bool isInMoving = false;

    /// <summary>
    /// Получаем контроллер игрока
    /// </summary>
    private CharacterController controller;

    /// <summary>
    /// Время анимации перемещения
    /// </summary>
    public float time;

    /// <summary>
    /// Текущее направление перемещения
    /// </summary>
    private float curDir;

    /// <summary>
    /// Сила гравитации
    /// </summary>
    public float gravityForce;

    /// <summary>
    /// Максимальное кол-во дорожек
    /// </summary>
    public int maxWays = 3;

    /// <summary>
    /// Текущая дорожка
    /// </summary>
    private int curWays;

    /// <summary>
    /// Направление гравитации
    /// </summary>
    private Vector3 gravDir;

    /// <summary>
    /// Скорость прыжка
    /// </summary>
    public float jumpSpeed;

    /// <summary>
    /// Скорость перемещения
    /// </summary>
    public float speed;

    /// <summary>
    /// Высота прыжка
    /// </summary>
    public float jumpHight = 3f;

    /// <summary>
    /// Текущая высота прыжка
    /// </summary>
    private float curJumpHight;

    /// <summary>
    /// Вертикальное направление
    /// </summary>
    private float curYDir;

    /// <summary>
    /// Проверка, прыгаем ли мы сейчас
    /// </summary>
    private bool isJump = false;

    /// <summary>
    /// Время прыжка
    /// </summary>
    private float jumpTime;

    /// <summary>
    /// Набор анимаций
    /// </summary>
    public AnimationClip[] anim;

    /// <summary>
    /// Проверка, в кувырке ли мы сейчас
    /// </summary>
    private bool isRoll;

    /// <summary>
    /// Мертвы ли мы сейчас
    /// </summary>
    public bool isDead;

    /// <summary>
    /// Проверка на быстрый прыжок
    /// </summary>
    private bool isFastJump = false;

    /// <summary>
    /// Высота игрока при кувырке
    /// </summary>
    private float rollCharecterHight;

    /// <summary>
    /// Время кувырка
    /// </summary>
    private float rollTime;

    /// <summary>
    /// Текущая время кувырка
    /// </summary>
    private float curRollTime;

    /// <summary>
    /// Центр коллизии при кувырке
    /// </summary>
    private float rollCenter;

    /// <summary>
    /// Проверка, забираемся ли мы сейчас по стене
    /// </summary>
    private bool isWallClimbing = false;

    /// <summary>
    /// Время анимацию залезания на стену
    /// </summary>
    private float climbingTime;

    /// <summary>
    /// Высота стены
    /// </summary>
    private float climbingHight;


    /// <summary>
    /// Экземпляр класса с монетками
    /// </summary>
    public Coins coins = Coins.SetInstance();

    /// <summary>
    /// Все трейлы
    /// </summary>
    public TrailRenderer[] trails;

    #region Particles
    public ParticleSystem landing;
    public ParticleSystem coinTake;
    public ParticleSystem deathParts;
    public ParticleSystem energyBonus;
    public ParticleSystem jumpBonus;
    #endregion

    /// <summary>
    /// Проверка на бонус для прыжка
    /// </summary>
    public bool isJumpBonus = false;

    /// <summary>
    /// Проверяет, начали ли мы приземлятся
    /// </summary>
    bool isLandingStarted = false;

    /// <summary>
    /// Скрипт управляющий миром
    /// </summary>
    public WorldController worldController;

    /// <summary>
    /// Окно смерти
    /// </summary>
    public GameObject deathWindow;

    /// <summary>
    /// Скрипт аудио
    /// </summary>
    public AudioManager audioManager;



    void Start()
    {
        coins.getSetCoins = PlayerPrefs.GetInt("Coins", 0);
        speed = 5.5f;
        //Получаем необходимые компоненты
        playerAnimator = gameObject.GetComponent<Animator>();
        controller = gameObject.GetComponent<CharacterController>();
        
        //Устанавливаем текущую дорожку
        curWays =  maxWays / 2;
        //Устанавливаем время для анимаций
        jumpTime = anim[0].length / jumpSpeed;
        time = anim[1].length / speed;
        rollTime = anim[2].length / 1.5f;
        climbingTime = anim[3].length / 1.6f;

        //Устанавливаем высоту и центр коллизии при кувырке
        rollCharecterHight = controller.height / 2.4f;
        rollCenter = controller.center.y / 1.8f;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        coins.setPoints = 0;

       

    }

    private void OnDestroy()
    {
        //Сохраняем монетки и рекорд игрока в реестр
        if (coins.setPoints > PlayerPrefs.GetFloat("Points", 0))
        {
            PlayerPrefs.SetFloat("Points", coins.setPoints);
        }
        PlayerPrefs.SetInt("Coins", coins.getSetCoins);
        
        PlayerPrefs.Save();
    }

    private void Update()
    {
       //Устанавливаем очки игрока и играем звуки
        if(!isDead)
            coins.setPoints += speed * Time.deltaTime;

            PlaySounds();
    }

    void FixedUpdate()
    {
        //Включаем гравитацию
        if (!isJump && !isWallClimbing)
        {
            gravDir = Vector3.zero;
            gravDir.y -= gravityForce * Time.deltaTime;
            controller.Move(gravDir);
        }

        if (isDead)
            return;
        float yDir = Input.GetAxisRaw("Vertical"); //Вертикальное направление
        float xDir = Input.GetAxisRaw("Horizontal"); //Горизонтальное направление


        //Проверяем, находимся ли мы в процессе приземления
        bool isLandingProcess = playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Jump_down") ^ playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Jump_complete_roll");
        if(controller.isGrounded && isLandingProcess && !isLandingStarted)
        {
            landing.Play();
            isLandingStarted = true;
        }


        if (!isJump && yDir > 0 && controller.isGrounded && !isLandingProcess && !isRoll && !isWallClimbing)
        {
            //Если мы подбигаем слишком близко к стене и она низкая, то включаем карабканье по стене
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 1.25f) && (hit.transform.localScale.y <= 2) && hit.transform.localScale.y >= 1.25f)
            {
               
                    climbingHight = hit.transform.localScale.y * 1.2f;
                    playerAnimator.SetBool("Climbing", true);
                    isWallClimbing = true;
                    curJumpHight = climbingHight;
                    curYDir = yDir;
            }
            //Иначе включаем прыжок
            else
            {
                if(isJumpBonus)
                {
                    jumpBonus.Play();
                }
                playerAnimator.SetTrigger("isJump");
                isJump = true;
                curJumpHight = jumpHight;
                curYDir = yDir;
                if (yDir > 0)
                {
                    playerAnimator.SetFloat("vSpeed", 0.1f);
                }
            }
        }
        if (isJump)
        {
            StartCoroutine(Jump(curYDir));
            isFastJump = false;
        }
        if(isWallClimbing)
        {
            StartCoroutine(Jump(curYDir));
        }
        if (!isJump && !controller.isGrounded && !isRoll && !isWallClimbing)
        {
            playerAnimator.SetFloat("vSpeed", -0.1f);

            trails[0].gameObject.SetActive(true);
            trails[1].gameObject.SetActive(true);
            isLandingStarted = false;

            //Если во время прыжка мы нажимаем "Вниз", то падение ускоряется
            if (yDir < 0)
            {
                isFastJump = true;
                isRoll = true;
                curRollTime = rollTime;
                gravityForce = 15f;
            }
        }
        //Если мы не лезим по стене, но прыгаем
        if (controller.isGrounded && !isWallClimbing)
        {
            playerAnimator.SetBool("isGround", true);
            playerAnimator.SetBool("isFastJump", isFastJump);
            trails[0].gameObject.SetActive(false);
            trails[1].gameObject.SetActive(false);
            if (isFastJump)
            {
                gravityForce = 6f;
                Roll();
            }

        }
        else
        {
            playerAnimator.SetBool("isGround", false);
        }
        

        if (!isInMoving && xDir != 0)
        {
         //Включаем анимацию в зависимости от направления движения   
            curDir = xDir;
            curDistance = distance;
            isInMoving = true;
            StopCoroutine(Move(curDir));
            if (xDir < 0 && curWays > 0 && controller.isGrounded && !playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Left") && !isRoll)
            {
                playerAnimator.Play("Running");
                playerAnimator.SetTrigger("Left");
                
            }
            else if (xDir > 0 && curWays < maxWays - 1 && controller.isGrounded && !playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Right") && !isRoll)
            {
                playerAnimator.Play("Running");
                playerAnimator.SetTrigger("Right");
            }
            
        }
        //Передвигаем персонажа в определённом направлении
        if(isInMoving)
        {
            StartCoroutine(Move(curDir));
        }

        //Обрабатываем кувырок
        if(!isRoll && yDir != 0 && controller.isGrounded && !isLandingProcess)
        {
            isRoll = true;
            curRollTime = rollTime;
            if (yDir < 0)
            {   
                playerAnimator.SetTrigger("Roll");
                playerAnimator.SetBool("isRoll", true);
            }
        }
        if(isRoll)
        {
            Roll();
        }

        if (!isWallClimbing)
        {
            worldController.WorldMoving(speed);
        }
        else worldController.WorldMoving(speed/5);
    }

    /// <summary>
    /// Перемещение влево и вправо
    /// </summary>
    /// <param name="curDir">Направление перемещения</param>
    private IEnumerator Move(float curDir)
    {

            if (curDistance <= 0)
            {
                if (curDir < 0)
                    curWays--;
                else if (curDir > 0)
                    curWays++;
                isInMoving = false;
                yield break;
            }
            if (curWays <= 0 && curDir < 0)
            {
                isInMoving = false;
                yield break;
            }
            else if (curWays >= maxWays - 1 && curDir > 0)
            {
                isInMoving = false;
                yield break;
            }
            float speed = distance / time;
            float tmpDist = speed * Time.deltaTime;
            controller.Move(Vector3.right * curDir * tmpDist);
            curDistance -= tmpDist;
    }

    /// <summary>
    /// Прыжок
    /// </summary>
    /// <param name="yDir">Направление</param>
    private IEnumerator Jump(float yDir)
    {
        if (curJumpHight <= 0)
        {
            isWallClimbing = false;
            isJump = false;
            if(!isWallClimbing)
                playerAnimator.SetFloat("vSpeed", 0f);
            playerAnimator.SetBool("Climbing", false);
            gravDir = Vector3.zero;
            yield break;
        }
        float speed = jumpHight / jumpTime;
        float tmpDist = speed * Time.deltaTime;

        if(isWallClimbing)
        {
            speed = climbingHight / climbingTime;
            tmpDist = speed * Time.deltaTime;
        }

        controller.Move(Vector3.up * yDir * tmpDist);
        curJumpHight -= tmpDist;

        if (controller.isGrounded)
        {
            isJump = false;
            if (!isWallClimbing)
                playerAnimator.SetFloat("vSpeed", 0f);
            yield break;
        }

    }

    /// <summary>
    /// Кувырок
    /// </summary>
    private void Roll()
    {
        if(curRollTime <=0)
        {
            isRoll = false;
            isFastJump = false;
            controller.height = rollCharecterHight * 2.4f;
            controller.center = new Vector3(controller.center.x, rollCenter * 1.8f);
            playerAnimator.SetBool("isRoll", false);
            return;
        }
        controller.height = rollCharecterHight;
        controller.center = new Vector3(controller.center.x, rollCenter);
        curRollTime -= (rollTime * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        //Обработка столкновений
        if(other.CompareTag("Danger") && !isInMoving && !isWallClimbing)
        {
            isDead = true;
            gravityForce = 0;
            StopAllCoroutines();
            playerAnimator.enabled = false;
            controller.enabled = false;
            deathParts.Play();
            StartCoroutine(ActivateDeathWindow(2f));


        }
        if(other.CompareTag("Danger") && isInMoving)
        {
            isDead = true;
            gravityForce = 0;
            StopAllCoroutines();
            StartCoroutine(ActivateDeathWindow(2f));
            playerAnimator.enabled = false;
            controller.enabled = false;

            if (curDir < 0)
            {
                deathParts.transform.rotation = Quaternion.Euler(deathParts.transform.rotation.x, 90, deathParts.transform.rotation.z);
            }
            else if(curDir > 0)
            {
                deathParts.transform.rotation = Quaternion.Euler(deathParts.transform.rotation.x, -90, deathParts.transform.rotation.z);
            }
            deathParts.Play();
        }
        if(other.CompareTag("Coin"))
        {
            coins.getSetCoins += 1;
            other.gameObject.SetActive(false);
            coinTake.Play();
            speed += 0.01f;
            audioManager.soundsSource.clip = audioManager.sounds[3];
            audioManager.soundsSource.loop = false;
            audioManager.soundsSource.Play();

        }
        if (other.CompareTag("EnergyBottle"))
        {
            StartCoroutine(SpeedBuf().GetEnumerator());
            other.gameObject.SetActive(false);
            energyBonus.Play();
        }
        if(other.CompareTag("JumpBottle"))
        {
            StartCoroutine(JumpBuf().GetEnumerator());
            other.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Включает окно смерти
    /// </summary>
    /// <param name="delay">Задержка в секундах</param>
    /// <returns></returns>
    private IEnumerator ActivateDeathWindow(float delay)
    {
        yield return new WaitForSeconds(delay);
        deathWindow.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    /// <summary>
    /// Увелечение скорости
    /// </summary>
    /// <returns></returns>
    private IEnumerable SpeedBuf()
    {
        speed = speed * 1.4f;
        yield return new WaitForSeconds(10);
        speed = speed / 1.4f;
    }    

    /// <summary>
    /// Баф к прыжку
    /// </summary>
    /// <returns></returns>
    private IEnumerable JumpBuf()
    {
        isJumpBonus = true;
        jumpHight = jumpHight * 2;
        gravityForce *= 1.5f;
        yield return new WaitForSeconds(10);
        jumpHight = jumpHight / 2;
        gravityForce /= 1.5f;
        isJumpBonus = false;
    }


    /// <summary>
    /// Проигрывает звуки игры
    /// </summary>
    private void PlaySounds()
    {
        
        if (isJump)
        {
            SetNewSounds(1, false);
            
        }
        if (isDead)
        {
            SetNewSounds(2, false);
        }
        if(controller.isGrounded && !isDead && !coinTake.isPlaying)
        {
            SetNewSounds(0, true);
        }
        
    }

    /// <summary>
    /// Устанавливает новый звук на источник
    /// </summary>
    /// <param name="soundID">Айди звука в массиве</param>
    /// <param name="looping">Зацикливание звука</param>
    private void SetNewSounds(int soundID, bool looping)
    {   
        if (audioManager.soundsSource.clip != audioManager.sounds[soundID])
        {
            audioManager.soundsSource.Stop();
            audioManager.soundsSource.clip = audioManager.sounds[soundID];
            audioManager.soundsSource.loop = looping;
            audioManager.soundsSource.Play();
        }
    }

}
