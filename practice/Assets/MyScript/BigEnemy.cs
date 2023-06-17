using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
public class BigEnemy : MonoBehaviour
{
    Animator anim;
    NavMeshAgent nav;
    [SerializeField] GameObject player;
    [SerializeField] GameObject playerMech;
    [SerializeField] GameObject shotEffect;
    [SerializeField] Slider HPbar;
    [SerializeField] Transform GunRoot;
    float detectDist;
    float shotDist;
    bool isHitEMP;
    public bool detectPlayer;
    public bool inRangeMech;
    bool followStart;

    float maxHP;
    public float HP;
    public bool isHitted;
    public bool isDead;

    void Awake()
    {
        anim = this.GetComponent<Animator>();
        nav = this.GetComponent<NavMeshAgent>();
        
    }

    void Start()
    {
        detectPlayer = false;
        inRangeMech = false;
        followStart = false;
        detectDist = 15.0f;
        shotDist = 25.0f;
        maxHP = 80.0f;
        HP = maxHP;
        isDead = false;
    }

    void Update()
    {
        if(!isHitEMP && !isDead)
        {
            if (!playerMech.GetComponent<PlayerMechController>().isStart)
            {
                FollowStartPlayer();
                DetectAround();
            }
            else
            {
                FollowStartMech();
                DetectPlayerMech();
            }
        }
        HitCheck();
        CheckHP();
    }

    void CheckHP()
    {
        HPbar.maxValue = maxHP;
        HPbar.value = HP;
        if(HP <= 0.0f)
        {
            isDead = true;
            nav.speed = 0.0f;
            StartCoroutine(DeadAnim());
        }
    }

    IEnumerator DeadAnim()
    {
        anim.SetBool("isDead", isDead);
        yield return new WaitForSeconds(4.0f);
        gameObject.SetActive(false);
    }

    void HitCheck()
    {
        if(isHitted)
        {
            StartCoroutine(HitAnim());
        }
    }

    IEnumerator HitAnim()
    {
        anim.SetBool("isHitted", isHitted);
        yield return new WaitForSeconds(0.7f);
        isHitted = false;
        anim.SetBool("isHitted", isHitted);
    }

    void Attack()
    {
        if (detectPlayer)
        {
            transform.LookAt(player.transform.position);
            Debug.Log("원거리공격");
            nav.SetDestination(this.transform.position);
            nav.speed = 0.0f;
            anim.SetFloat("isMove", nav.speed);

            followStart = true;
        }

        else if(inRangeMech)
        {
            transform.LookAt(playerMech.transform.position);
            nav.SetDestination(this.transform.position);
            nav.speed = 0.0f;
            anim.SetFloat("isMove", nav.speed);
        }
    }
    
    void FollowStartPlayer()
    {
        if(followStart)
        {
            if (Vector3.Distance(transform.position, player.transform.position) > detectDist)
            {
                detectPlayer = false;
                anim.SetBool("isDetect", detectPlayer);
                nav.SetDestination(player.transform.position);
                nav.speed = 2.5f;
                anim.SetFloat("isMove", nav.speed);
            }
        }
    }

    void DetectAround()
    {
        var colliders = Physics.OverlapSphere(this.transform.position, detectDist);

        foreach (var collider in colliders)
        {
            if (!collider.CompareTag("Player"))
            {
                continue;
            }
            else
            {
                detectPlayer = true;
                followStart = true;
                anim.SetBool("isDetect", detectPlayer);
                Attack();
            }
        }
    }

    void FollowStartMech()
    {
        if(Vector3.Distance(transform.position,playerMech.transform.position) > shotDist)
        {
            inRangeMech = false;
            anim.SetBool("isDetect", inRangeMech);
            nav.SetDestination(playerMech.transform.position);
            nav.speed = 2.5f;
            anim.SetFloat("isMove", nav.speed);
        }
    }

    void DetectPlayerMech()
    {
        var colliders = Physics.OverlapSphere(this.transform.position, shotDist);
        foreach (var collider in colliders)
        {
            if (!collider.CompareTag("PlayerMech"))
            {
                continue;
            }
            else
            {
                inRangeMech = true;
                anim.SetBool("isDetect", inRangeMech);
                Attack();
            }
        }
    }

    void MakeShotEffect()
    {
        if(playerMech.activeSelf == true)
        {
            if(!isHitted && inRangeMech)
            {
                GameObject instantShotEffect = Instantiate(shotEffect, GunRoot.position, GunRoot.rotation);
                Destroy(instantShotEffect, 2.5f);
            }
        }
    }

    public void HitEMP()
    {
        isHitEMP = true;
        detectPlayer = false;
        anim.SetBool("isDetect", detectPlayer);
        followStart = false;
        nav.SetDestination(this.transform.position);
        nav.speed = 0.0f;
        anim.SetFloat("isMove", nav.speed);
        anim.SetBool("isHitEMP", isHitEMP);
        StartCoroutine(EMPeffect());
    }

    IEnumerator EMPeffect()
    {
        yield return new WaitForSeconds(10.0f);
        isHitEMP = false;
        anim.SetBool("isHitEMP", isHitEMP);
        yield return new WaitForSeconds(2.0f);
    }
}
