using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BEShotEffect1 : MonoBehaviour
{
    [SerializeField] Transform effectPos;
    [SerializeField] float power;
    Panda.BossBT.BossAI bossai;

    void Start()
    {
        bossai = GameObject.Find("Boss").GetComponent<Panda.BossBT.BossAI>();
    }

    void Update()
    {
        transform.Translate(0, 0, power);
        RaycastHit hit;
        if(Physics.Raycast(transform.position,transform.forward,out hit,1))
        {
            //if(hit.collider.CompareTag("PlayerMech"))
            //{
            //    hit.collider.GetComponent<PlayerMechController>().HP -= 15.0f;
            //    hit.collider.GetComponent<PlayerMechController>().isHitMech = true;
            //    Debug.Log("hp : " + hit.collider.GetComponent<PlayerMechController>().HP);
            //    Destroy(gameObject);
            //}
            
            if(hit.collider.CompareTag("PlayerMech"))
            {
                hit.collider.GetComponent<PlayerMechController1>().HP -= 7.0f;
                hit.collider.GetComponent<PlayerMechController1>().isHitMech = true;
                Debug.Log("hp : " + hit.collider.GetComponent<PlayerMechController1>().HP);
                bossai.UltGauge += 10.0f;
                Destroy(gameObject);
            }
        }
    }
}
