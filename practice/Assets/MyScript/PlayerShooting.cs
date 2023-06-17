using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerShooting : MonoBehaviour
{
    TPSCharacterController tpscharactercontroller;
    PlayerMechController playermechcontroller;
    MechShooting mechshooting;
    Animator animator;

    [SerializeField] GameObject TPScam;
    [SerializeField] Transform player;

    [SerializeField] GameObject EMP;
    [SerializeField] Text empCount;

    [SerializeField] Text bulletCount;
    [SerializeField] GameObject shotEffect;
    [SerializeField] GameObject shotHitEffect;
    [SerializeField] Transform gunRoot;

    bool isshot;
    public float Shotdistance;
    float shotDelay;
    float shotDelayTimer;
    int maxBullet;
    int currentBullet;
    bool needReload;
    public bool isReload;

    int maxEMP;
    int currentEMP;

    // Start is called before the first frame update
    void Awake()
    {
        tpscharactercontroller = GameObject.Find("Player").GetComponent<TPSCharacterController>();
        animator = player.GetComponent<Animator>();
    }

    void Start()
    {
        Shotdistance = 20f;
        shotDelay = 0.2f;
        shotDelayTimer = 0;
        maxBullet = 30;
        currentBullet = maxBullet;
        maxEMP = 3;
        currentEMP = maxEMP;

        needReload = false;
        isReload = false;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(TPScam.transform.position, TPScam.transform.forward * 20f, Color.red);
        Shot();
        ThrowEMP();
        ReloadCheck();
    }

    void Shot()
    {
        shotDelayTimer += Time.deltaTime;
        bulletCount.text = currentBullet + " / " + maxBullet;
        RaycastHit hit;

        //√— ø°¿”
        if (Physics.Raycast(TPScam.transform.position, TPScam.transform.forward, out hit, Shotdistance))
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                tpscharactercontroller.Crosshair.color = Color.red;
                if (Input.GetMouseButtonDown(0) && tpscharactercontroller.shoulderPatching && !needReload)
                {
                    Debug.Log(hit.transform.name);
                    GameObject.Find(hit.transform.name).GetComponent<SmallEnemy>().HP -= 3.0f;
                }
            }

            if (hit.collider.CompareTag("BigEnemy"))
            {
                tpscharactercontroller.Crosshair.color = Color.red;
                if (Input.GetMouseButtonDown(0) && tpscharactercontroller.shoulderPatching && !needReload)
                {
                    Debug.Log(hit.transform.name);
                }
            }
        }
        else
        {
            tpscharactercontroller.Crosshair.color = Color.white;
        }

        if (Input.GetMouseButtonDown(0) && tpscharactercontroller.shoulderPatching 
            && !needReload && !isReload && shotDelayTimer > shotDelay)
        {
            shotDelayTimer = 0;
            animator.SetBool("isShoot", Input.GetMouseButtonDown(0));
            currentBullet -= 1;
            bulletCount.text = currentBullet + " / " + maxBullet;
            Instantiate(shotEffect, gunRoot.position, gunRoot.rotation);
            Instantiate(shotHitEffect, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
        }
        else
            animator.SetBool("isShoot", Input.GetMouseButtonDown(0));
    }

    void ThrowEMP()
    {
        empCount.text = currentEMP + " / " + maxEMP;
        if (Input.GetKeyDown(KeyCode.E) && currentEMP > 0)
        {
            currentEMP -= 1;
            float throwSpeed = 28.0f;
            GameObject instanceEMP = Instantiate(EMP, new Vector3(transform.position.x, transform.position.y + 2, transform.position.z), TPScam.transform.rotation);
            Rigidbody rigidEMP = instanceEMP.GetComponent<Rigidbody>();
            rigidEMP.AddForce(TPScam.transform.forward * throwSpeed, ForceMode.Impulse);
        }
    }

    void ReloadCheck()
    {
        if (currentBullet <= 0)
        {
            needReload = true;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(ReloadAnimPlay());
        }
    }

    void RefillBullet()
    {
        currentBullet = maxBullet;
    }

    IEnumerator ReloadAnimPlay()
    {
        isReload = true;
        animator.SetBool("isReload", isReload);
        yield return new WaitForSeconds(2.0f);
        isReload = false;
        animator.SetBool("isReload", isReload);
        needReload = false;
    }
}
