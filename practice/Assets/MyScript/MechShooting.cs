using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MechShooting : MonoBehaviour
{
    PlayerMechController playermechcontroller;
    [SerializeField] Camera FPScam;
    [SerializeField] GameObject PlayerMech;
    [SerializeField] Transform GunRoot;
    [SerializeField] Transform SkillRoot;
    [SerializeField] Image crosshair;
    [SerializeField] GameObject UltEffect;
    [SerializeField] GameObject ShotEffect;
    [SerializeField] GameObject dangercloseEffect;
    [SerializeField] Text UltCount;
    [SerializeField] Text BulletCount;
    [SerializeField] Image ESkill;
    [SerializeField] RawImage ultimage1;
    [SerializeField] RawImage ultimage2;

    Animator animator;
    bool shoot;
    Vector3 tempPos;

    float shotDelay;
    float shotDelayTimer;
    int currentBullet;
    int maxBullet;
    bool needReload;
    bool isReload;

    bool CanUseE;
    float ECooltime;
    float ETimer;

    public float UltGauge;
    float maxUltGauge;


    private void Awake()
    {
        animator = PlayerMech.GetComponent<Animator>();
    }
    void Start()
    {
        shoot = false;
        shotDelay = 0.4f;
        shotDelayTimer = 0f;
        maxBullet = 15;
        currentBullet = maxBullet;
        needReload = false;
        isReload = false;

        ECooltime = 3.0f;
        ETimer = 0f;
        CanUseE = true;

        UltGauge = 0;
        maxUltGauge = 100f;
    }

    void Update()
    {
        Debug.DrawRay(FPScam.transform.position, new Vector3(FPScam.transform.forward.x, FPScam.transform.forward.y, FPScam.transform.forward.z).normalized * 100, Color.red);
        Shot();
        Skill();
        CheckReload();
        UIupdate();
        CheckUltGauge();
    }

    void UIupdate()
    {
        UltCount.text = (int)UltGauge + " %";
        if (currentBullet > 5)
            BulletCount.color = Color.white;
        else if (currentBullet > 0 && currentBullet < 5)
            BulletCount.color = Color.yellow;
        else if(currentBullet <= 0)
            BulletCount.color = Color.red;
        BulletCount.text = currentBullet + " / " + maxBullet;

        ESkill.type = Image.Type.Filled;
        ESkill.fillMethod = Image.FillMethod.Radial360;
        ESkill.fillOrigin = (int)Image.Origin360.Top;
        ESkill.fillClockwise = false;

        if(!CanUseE)
        {
            ETimer += Time.deltaTime;
            if(ETimer < ECooltime)
            {
                ESkill.fillAmount = ETimer / ECooltime;
            }
            else
            {
                ETimer = 0;
                CanUseE = true;
            }
        }
        else
        {
            ESkill.fillAmount = 0;
        }

        
    }

    void CheckReload()
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
    IEnumerator ReloadAnimPlay()
    {
        isReload = true;
        animator.SetBool("isReload", isReload);
        yield return new WaitForSeconds(2.0f);
        isReload = false;
        animator.SetBool("isReload", isReload);
        needReload = false;
    }

    void Shot()
    {
        shotDelayTimer += Time.deltaTime;
        RaycastHit hit;
        if(Physics.Raycast(FPScam.transform.position, FPScam.transform.forward, out hit, 80.0f))
        {
            crosshair.color = Color.white;
            if(hit.collider.CompareTag("BigEnemy"))
            {
                crosshair.color = Color.red;
            }
        }

        //클릭 && 탄창모자라지 않을때 && 샷딜레이 && 재장전 중 아닐때
        if (Input.GetMouseButtonDown(0) && !needReload && shotDelayTimer > shotDelay && !isReload)
        {
            shotDelayTimer = 0f;
            shoot = true;
            animator.SetBool("isShoot", shoot);
            currentBullet -= 1;
            Instantiate(ShotEffect, GunRoot.transform.position, GunRoot.transform.rotation);
        }
        else
        {
            shoot = false;
            animator.SetBool("isShoot", shoot);
        }
    }

    void Skill()
    {
        if(Input.GetKeyDown(KeyCode.G) && UltGauge == 100f)
        {
            UltGauge = 0f;
            StartCoroutine(Ult());
            tempPos = this.transform.position;
        }

        if(UltEffect.activeSelf == true)
        {
            UltEffect.transform.position = GunRoot.position;
            UltEffect.transform.rotation = GunRoot.rotation;
            this.transform.position = tempPos;
        }

        if(Input.GetKeyDown(KeyCode.E) && CanUseE)
        {
            CanUseE = false;
            Vector3 effectPos = FPScam.transform.position + FPScam.transform.forward * 35.0f;
            effectPos = new Vector3(effectPos.x, 0, effectPos.z + 5f);
            Vector3 effectRot = transform.position + transform.forward;
            Instantiate(dangercloseEffect, effectPos, SkillRoot.transform.rotation);
        }

    }

    void CheckUltGauge()
    {
        if (UltGauge >= 100.0f)
        {
            UltGauge = maxUltGauge;
            ultimage1.GetComponent<Animation>().enabled = true;
            ultimage2.GetComponent<Animation>().enabled = true;
        } 
        
        else
        {
            ultimage1.GetComponent<Animation>().enabled = false;
            ultimage2.GetComponent<Animation>().enabled = false;
        }
    }
    IEnumerator Ult()
    {
        UltEffect.SetActive(true);
        animator.SetBool("isUlt", UltEffect.activeSelf);
        yield return new WaitForSeconds(4.0f);
        UltEffect.SetActive(false);
        animator.SetBool("isUlt", UltEffect.activeSelf);
    }

    void RefillBullet()
    {
        currentBullet = maxBullet;
    }
}
