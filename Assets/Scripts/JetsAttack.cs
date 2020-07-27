using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JetsAttack : MonoBehaviour
{

    /// <summary>
    /// Скорость
    /// </summary>
    public float speed;

    /// <summary>
    /// Текущее местоположение игрока
    /// </summary>
    private Vector3 player;
    
    /// <summary>
    /// Системы частисц со взрывом
    /// </summary>
    public ParticleSystem[] particleSystems;

    /// <summary>
    /// Объект взрыва
    /// </summary>
    public GameObject explosionObject;

    /// <summary>
    /// Атакует истребитель ли сейчас
    /// </summary>
    private bool isAttack = false;

    /// <summary>
    /// Количество атак
    /// </summary>
    private int attackCount = 0;

    /// <summary>
    /// Источник звука истребителя
    /// </summary>
    private AudioSource audioSource;
    void Start()
    {
        //Получаем необходимые компоненты
        player = GameObject.FindGameObjectWithTag("Player").transform.position;
        audioSource = GetComponentInChildren<AudioSource>();
    }

    void FixedUpdate()
    {
        //Двигаем в сторону игрока
        transform.position += Vector3.back * speed * Time.deltaTime;
        if(transform.position.z - player.z <= 12 && !isAttack)
        {
            JetAttack();
        }
        if(transform.position.z - player.z <= -9)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Атака истребителя
    /// </summary>
    void JetAttack()
    {
        //Проигрываем все системы частиц
        
        for (int i = 0; i < particleSystems.Length; i++)
        {
            particleSystems[i].Play();
            audioSource.enabled = true;
            audioSource.Play();
        }
        StartCoroutine(Reload(0.2f));


        attackCount++;
        if (attackCount >= 3)
        {
            isAttack = true;
            attackCount = 0;
            explosionObject.SetActive(false);
            audioSource.Stop();
            audioSource.enabled = false;
        }
    }

    /// <summary>
    /// Перезарядка
    /// </summary>
    /// <param name="delay"></param>
    /// <returns></returns>
    IEnumerator Reload(float delay)
    {
        yield return new WaitForSeconds(delay / 1.5f);
        explosionObject.SetActive(false);
        yield return new WaitForSeconds(delay / 1.4f);
        explosionObject.SetActive(true);
        isAttack = false;



    }

}
