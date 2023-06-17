using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UltHit : MonoBehaviour
{
    float timer;
    float hittime;
    void Start()
    {
        timer = 0;
        hittime = 0.4f;
    }

    void LateUpdate()
    {
        Debug.DrawRay(transform.position, transform.forward * 26f, Color.white);
        timer += Time.deltaTime;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 26f))
        {
            if(hit.collider.CompareTag("BigEnemy"))
            {
                if(timer > hittime)
                {
                    timer = 0;
                    hit.collider.GetComponent<BigEnemy>().isHitted = true;
                    hit.collider.GetComponent<BigEnemy>().HP -= 5.0f;
                }
            }
            else if(hit.collider.CompareTag("Boss"))
            {
                if(timer > hittime)
                {
                    timer = 0;
                    hit.collider.GetComponent<Panda.BossBT.BossAI>().isHitted = true;
                    hit.collider.GetComponent<Panda.BossBT.BossAI>().BossHP -= 5.0f;
                }
            }
        }
    }
}
