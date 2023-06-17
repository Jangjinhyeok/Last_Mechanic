using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
public class SmallEnemy : MonoBehaviour
{
    [SerializeField] Transform[] waypoints;
    [SerializeField] GameObject player;
    [SerializeField] Transform gunRoot;
    [SerializeField] GameObject shotEffect;
    [SerializeField] Slider HPbar;
    Animator anim;
    NavMeshAgent nav;

    int waycount;
    float distance;
    bool isDetect;
    bool isOnRay;

    float shotTimer;
    int shotDelayTime;

    float maxHP;
    public float HP;
    bool isHitted;

    RaycastHit rayPoint;

    void Awake()
    {
        anim = this.GetComponent<Animator>();
        nav = this.GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        waycount = 0;
        distance = 18.0f;
        nav.SetDestination(waypoints[0].position);
        anim.SetBool("isMove", nav.speed > 0);
        isDetect = false;
        isOnRay = false;

        shotTimer = 2.0f;
        shotDelayTime = 2;

        maxHP = 30.0f;
        HP = maxHP;
        isHitted = false;
    }

    void Update()
    {
        if(!isDetect)
            Patrol();
        Attack();
        DetectPlayer();
        CheckHP();
    }

    void Patrol()
    {
        if (Vector3.Distance(transform.position, waypoints[waycount].position) < 1)
        {
            anim.SetBool("isMove", nav.speed > 0);

            if (waycount == waypoints.Length - 1)
            {
                waycount = 0;
                nav.SetDestination(waypoints[0].position);
            }
            else if(waycount < waypoints.Length)
            {
                waycount++;
                nav.SetDestination(waypoints[waycount].position);
            }
        }
    }

    void DetectPlayer()
    {
        RaycastHit hit;
        Vector3 rayTrans;
        rayTrans = new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z);
        Debug.DrawRay(rayTrans, transform.forward * distance, Color.red);

        if (Physics.Raycast(rayTrans, transform.forward, out hit, distance))
        {
            if (hit.collider.CompareTag("Player"))
            {
                Debug.Log("detect player");
                isDetect = true;
                isOnRay = true;
                rayPoint = hit;
            }
        }
        else
            isOnRay = false;
    }

    void Attack()
    {
        if(isDetect)
        {
            transform.LookAt(player.transform.position);
            if(Vector3.Distance(transform.position,player.transform.position) <= player.GetComponent<PlayerShooting>().Shotdistance - 2)
            {
                shotTimer += Time.deltaTime;
                if (shotTimer >= shotDelayTime)
                {
                    shotTimer = 0;
                    nav.speed = 0;
                    anim.SetBool("isMove", false);

                    anim.SetBool("isShot", true);
                }

                else if (shotTimer < shotDelayTime)
                {
                    nav.speed = 0;
                    anim.SetBool("isShot", false);
                    anim.SetBool("isMove", false);
                }
            }
            else if (Vector3.Distance(transform.position, player.transform.position) > player.GetComponent<PlayerShooting>().Shotdistance - 2)
            {
                transform.LookAt(player.transform.position);
                anim.SetBool("isShot", false);
                nav.SetDestination(player.transform.position);
                nav.speed = 3.5f;
                anim.SetBool("isMove", nav.speed > 0);
            }
        }
    }

    void MakeShotEffect()
    {
        Instantiate(shotEffect, gunRoot.position, gunRoot.rotation);
        if (isOnRay)
        {
            player.GetComponent<TPSCharacterController>().isHitted = true;
            player.GetComponent<TPSCharacterController>().HP -= 5.0f;
        }
    }

    void CheckHP()
    {
        HPbar.maxValue = maxHP;
        HPbar.value = HP;
        if (HP < 30.0f)
            isHitted = true;
        if (isHitted)
            isDetect = true;
        if(HP <= 0.0f)
        {
            Destroy(gameObject);
        }
    }
}
