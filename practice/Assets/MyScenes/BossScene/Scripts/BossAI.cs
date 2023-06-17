using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Panda;

namespace Panda.BossBT
{
    public class BossAI : MonoBehaviour
    {
        [SerializeField] GameObject PlayerMech;
        [SerializeField] GameObject ActionCam;
        [SerializeField] GameObject ActionCamUI;

        NavMeshAgent nav;
        Animator anim;
        public float BossHP;
        public float MaxBossHP;

        public bool phaseStart;
        public bool BossAlive;


        [SerializeField] GameObject shotEffect;
        [SerializeField] Transform gunRoot;
        float DistBossToPlayer;
        float ShotDist;

        [SerializeField] GameObject MeleeCol;
        float MeleeDist;
        bool UseMeleeAttack;

        [SerializeField] GameObject EMPEffect;
        public bool CanUseEMP;
        public bool usedEMP;
        float EMPCool;
        float EMPTimer;

        [SerializeField] GameObject Phase2Effect;
        [SerializeField] GameObject UltEffect;
        public float UltGauge;
        public float MaxUltGauge;
        public bool CanUseUlt;
        public bool useUlt;
        Vector3 RandUltPos;

        public bool playerFar;
        public bool playerMiddle;
        public bool playerClose;
        public bool isHitted;



        #region tasks

        //Tree Root
        [Task]
        public bool Phase1;
        [Task]
        public bool Phase2;


        [Task]
        bool CheckAlive()
        {
            CheckHP();
            return BossAlive;
        }

        [Task]
        bool CheckPhase1()
        {
            return Phase1;
        }

        [Task]
        bool CheckPhase2()
        {
            CheckUltGauge();
            return Phase2;
        }

        [Task]
        bool UpdateHit()
        {
            anim.SetBool("isHitted", isHitted);
            Invoke("UpdateBoolHit", 3f);
            return true;
        }

        [Task]
        bool ControlAnim()
        {
            anim.SetFloat("isMove", nav.speed);
            anim.SetBool("RangeAttack", playerMiddle);
            anim.SetBool("MeleeAttack", playerClose);
            anim.SetBool("isAlive", BossAlive);
            anim.SetBool("isHitted", isHitted);
            anim.SetBool("useEMP", usedEMP);
            if(Phase2)
                anim.SetBool("useUlt", CanUseUlt);
            return true;
        }

        [Task]
        void MovetoPlayer()
        {
            nav.SetDestination(PlayerMech.transform.position);
            nav.speed = 4.5f;
        }

        [Task]
        bool IsPlayerRangeFar()
        {
            CheckRange();
            MovetoPlayer();

            return playerFar;
        }

        [Task]
        bool IsPlayerRangeMiddle()
        {
            CheckRange();
            RangeAttack();

            return playerMiddle;
        }

        [Task]
        bool IsPlayerRangeClose()
        {
            CheckRange();
            MeleeAttack();

            return playerClose;
        }

        [Task]
        void RangeAttack()
        {
            nav.SetDestination(this.transform.position);
            nav.speed = 0.0f;
            transform.LookAt(PlayerMech.transform.position);
            Debug.Log("RangeAttack");
        }

        [Task]
        void MeleeAttack()
        {
            nav.SetDestination(this.transform.position);
            nav.speed = 0.0f;
            transform.LookAt(PlayerMech.transform.position);
            Debug.Log("MeleeAttack");
        }

        [Task] 
        bool EMPExplosion()
        {
            CheckEMPCoolTime();
            return CanUseEMP;
        }

        [Task]
        bool UltUse()
        {
            //CheckUltGauge();
            return CanUseUlt;
        }

        [Task]
        void CheckUltGauge()
        {
            UpdateUltGauge();
        }

        #endregion

        void Awake()
        {
            nav = GetComponent<NavMeshAgent>();
            anim = GetComponent<Animator>();
        }

        void Start()
        {
            MaxBossHP = 100.0f;
            BossHP = MaxBossHP;
            BossAlive = true;
            //anim.SetBool("isAlive", BossAlive);

            phaseStart = false;
            Phase1 = false;
            Phase2 = false;

            ShotDist = 40.0f;

            usedEMP = false;
            CanUseEMP = true;
            EMPCool = 8.0f;
            EMPTimer = 0.0f;

            MeleeDist = 20.0f;

            CanUseUlt = true;
            MaxUltGauge = 50.0f;
            UltGauge = MaxUltGauge;
        }

        void Update()
        {
            if (PlayerMech.GetComponent<PlayerMechController1>().bossAppear)
            {

                StartCoroutine(StartPhase());
            }
        }

        IEnumerator StartPhase()
        {
            //플레이어 기능 정지
            ActionCam.gameObject.SetActive(true);
            PlayerMech.GetComponent<PlayerMechController1>().FPSCam.gameObject.SetActive(false);
            PlayerMech.GetComponent<PlayerMechController1>().FPSUI.gameObject.SetActive(false);
            PlayerMech.GetComponent<PlayerMechController1>().enabled = false;

            //카메라 이동 시간
            yield return new WaitForSeconds(1.8f);
            //애니메이션 실행 및 실행 시간동안 정지
            anim.SetBool("isAppear", true);
            yield return new WaitForSeconds(1.7f);
            ActionCamUI.gameObject.SetActive(true);
            yield return new WaitForSeconds(1.3f);

            ActionCam.gameObject.SetActive(false);
            PlayerMech.GetComponent<PlayerMechController1>().enabled = true;
            PlayerMech.GetComponent<PlayerMechController1>().FPSCam.gameObject.SetActive(true);
            PlayerMech.GetComponent<PlayerMechController1>().FPSUI.gameObject.SetActive(true);


            phaseStart = true;
            anim.SetBool("isStart", phaseStart);
            Phase1 = true;
            this.GetComponent<PandaBehaviour>().enabled = true;
            PlayerMech.GetComponent<PlayerMechController1>().bossAppear = false;
        }


        void CheckHP()
        {
            if(BossHP > 50.0f)
            {
                Phase1 = true;
                Phase2 = false;
            }

            else if(BossHP <= 50.0f && BossHP > 0.0f)
            {
                Phase1 = false;
                Phase2 = true;
                EMPCool = 5.0f;
                Phase2Effect.SetActive(true);
            }

            else if(BossHP <= 0.0f)
            {
                Phase1 = false;
                Phase2 = false;
                BossAlive = false;
                nav.speed = 0.0f;
                Phase2Effect.SetActive(false);
            }
        }

        void CheckRange()
        {
            DistBossToPlayer = Vector3.Distance(PlayerMech.transform.position, transform.position);
            if(DistBossToPlayer > ShotDist)
            {
                playerFar = true;
                playerMiddle = false;
                playerClose = false;
                MeleeCol.SetActive(false);
            }

            else if (DistBossToPlayer < ShotDist && DistBossToPlayer > MeleeDist)
            {
                playerFar = false;
                playerMiddle = true;
                playerClose = false;
                MeleeCol.SetActive(false);
            }

            else if(DistBossToPlayer <= MeleeDist)
            {
                playerFar = false;
                playerMiddle = false;
                playerClose = true;
                MeleeCol.SetActive(true);
            }
        }

        void UpdateBoolHit()
        {
            isHitted = false;
        }

        void UpdateLookAt()
        {
            transform.LookAt(PlayerMech.transform.position);
        }

        void MakeShotEffect()
        {
            GameObject instantShotEffect = Instantiate(shotEffect, gunRoot.position, gunRoot.rotation);
            Destroy(instantShotEffect, 2.5f);
        }

        void MakeEMPEffect()
        {
            Instantiate(EMPEffect, new Vector3(transform.position.x,1, transform.position.z), transform.rotation);
            if(phaseStart)
            {
                CanUseEMP = false;
                usedEMP = false;
            }
        }

        void UpdateUltGauge()
        {
            if(Phase2)
            {
                if (UltGauge == MaxUltGauge)
                {
                    CanUseUlt = true;
                }
            }
        }

        void GetRandomPos()
        {
            float r = 40f;
            float a = transform.position.x;
            float b = transform.position.z;

            float x = Random.Range(-r + a, r + a);
            float y_b = Mathf.Sqrt(Mathf.Pow(r, 2) - Mathf.Pow(x - a, 2));
            y_b *= Random.Range(0, 2) == 0 ? -1 : 1;
            float y = y_b + b;
            RandUltPos = new Vector3(x, 1, y);
        }

        void MakeUltEffect()
        {
            for(int i = 0; i < 3; i++)
            {
                GetRandomPos();
                Instantiate(UltEffect, RandUltPos, transform.rotation);
            }
            CanUseUlt = false;
            UltGauge = 0.0f;
        }

        void CheckEMPCoolTime()
        {
            if(!CanUseEMP)
            {
                EMPTimer += Time.deltaTime;
                if(EMPTimer >= EMPCool)
                {
                    CanUseEMP = true;
                    EMPTimer = 0;
                }
            }
        }

        void UpdateMeleeAttackBool()
        {
            UseMeleeAttack = true;
        }

        private void OnTriggerStay(Collider other)
        {
            if(other.gameObject.CompareTag("PlayerMech"))
            {
                if (playerClose && CanUseEMP)
                {
                    usedEMP = true; 
                }

                else if(UseMeleeAttack && !CanUseEMP)
                {
                    UseMeleeAttack = false;
                    PlayerMech.GetComponent<PlayerMechController1>().HP -= 15.0f;
                    PlayerMech.GetComponent<PlayerMechController1>().isHitMech = true;
                    UltGauge += 10.0f;
                }
            }
        }
    }
}
