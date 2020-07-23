using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JetsAttack : MonoBehaviour
{
    public float speed;
    private Vector3 player;
    public ParticleSystem[] particleSystems;
    public GameObject explosionObject;
    private bool isAttack = false;
    private int attackCount = 0;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

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
    void JetAttack()
    {
        
        for (int i = 0; i < particleSystems.Length; i++)
        {
            particleSystems[i].Play();
        }
        StartCoroutine(Reload(0.2f));


        attackCount++;
        if (attackCount >= 3)
        {
            isAttack = true;
            attackCount = 0;
            explosionObject.SetActive(false);
            //StopAllCoroutines();
            //StartCoroutine(Reload(1f));
        }
    }
    IEnumerator Reload(float delay)
    {
        yield return new WaitForSeconds(delay / 1.5f);
        explosionObject.SetActive(false);
        yield return new WaitForSeconds(delay / 1.4f);
        explosionObject.SetActive(true);
        isAttack = false;



    }
    IEnumerator AttackDelay()
    {
        yield return new WaitForSeconds(1f);
        isAttack = false;
    }

}
