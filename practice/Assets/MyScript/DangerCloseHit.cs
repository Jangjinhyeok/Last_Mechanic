using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerCloseHit : MonoBehaviour
{
    float timer;
    float hittime;
    void Start()
    {
        hittime = 0.3f;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        timer += Time.deltaTime;

        RaycastHit[] rayHits = Physics.SphereCastAll(transform.position, 30f, Vector3.up, 30f, LayerMask.GetMask("Enemy"));
        foreach (RaycastHit hitObj in rayHits)
        {
            if(timer >= hittime)
            {
                timer = 0;
                if (hitObj.collider.CompareTag("BigEnemy"))
                {
                    hitObj.transform.GetComponent<BigEnemy>().isHitted = true;
                    hitObj.transform.GetComponent<BigEnemy>().HP -= 5.0f;
                    GameObject.Find("PlayerMech").GetComponent<MechShooting>().UltGauge += 3.5f;
                }

                else if (hitObj.collider.CompareTag("Boss"))
                {
                    hitObj.transform.GetComponent<Panda.BossBT.BossAI>().isHitted = true;
                    hitObj.transform.GetComponent<Panda.BossBT.BossAI>().BossHP -= 2.0f;
                    GameObject.Find("PlayerMech").GetComponent<PlayerMechShooting>().UltGauge += 3.5f;
                }

            }
        }
    }

}
