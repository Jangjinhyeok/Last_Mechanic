using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
public class TPSCharacterController : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject playerMech;
    public Camera TPScam;
    [SerializeField] GameObject FPSCam;
    [SerializeField] Slider HPbar;
    [SerializeField] GameObject TPSUI;
    public Camera actionCam;
    public Image Crosshair;
    public bool shoulderPatching;
    public bool ride;
    public bool isMove;
    public float moveSpeed;
    Animator animator;

    Vector3 Player_Height, Player_Side;
    public float Camdist = 10;
    float saveShotDist;
    float shoulderPatchingShotDist;
    float xmove; // X축 누적 이동량 
    float ymove; // Y축 누적 이동량 

    float maxHP;
    public float HP;
    public bool isHitted;
    

    private void Awake()
    {
        animator = player.GetComponent<Animator>();
        Cursor.lockState = CursorLockMode.Locked;
    }
    void Start()
    {
        playerMech.GetComponent<CapsuleCollider>().center = new Vector3(0, 1, 0.7f);
        playerMech.GetComponent<PlayerMechController>().enabled = false;
        playerMech.GetComponent<MechShooting>().enabled = false;
        shoulderPatching = false;
        Crosshair.enabled = false;
        ride = false;
        Player_Height = new Vector3(0, 2.0f, 0f);
        Player_Side = new Vector3(1.0f, 0f, 0f);
        saveShotDist = this.GetComponent<PlayerShooting>().Shotdistance;
        shoulderPatchingShotDist = saveShotDist + 5f;

        maxHP = 100.0f;
        HP = maxHP;
        isHitted = false;
        moveSpeed = 6.0f;
    }

    void Update()
    {
        LookAround();
        if(!isHitted && !this.GetComponent<PlayerShooting>().isReload)
            Move();
        isShoot();
        CheckRide();
        CheckHit();
        CheckHP();
    }

    void CheckHP()
    {
        HPbar.maxValue = maxHP;
        HPbar.value = HP;
    }

    void isShoot()
    {
        // 조준키
        if (Input.GetMouseButtonDown(1) && !shoulderPatching)
        {
            shoulderPatching = true;
        }
        else if(Input.GetMouseButtonDown(1) && shoulderPatching)
        {
            shoulderPatching = false;
        }

        if (shoulderPatching)
            Crosshair.enabled = true;
        else
            Crosshair.enabled = false;
    }
    void LookAround()
    {
        xmove += Input.GetAxis("Mouse X");
        ymove -= Input.GetAxis("Mouse Y");
        

        if (!shoulderPatching)
        {
            moveSpeed = 6.0f;
            ymove = Mathf.Clamp(ymove, 0, 55);
            this.GetComponent<PlayerShooting>().Shotdistance = saveShotDist;
            Vector3 Eye = player.transform.position + TPScam.transform.rotation * Vector3.zero + Player_Height;
            Vector3 reverseDistance = new Vector3(0.0f, 0.0f, Camdist);
            TPScam.transform.position = Eye - TPScam.transform.rotation * reverseDistance;
            animator.SetBool("isShoulderPatching", shoulderPatching);
        }

        else if(shoulderPatching)
        {
            moveSpeed = 4.0f;
            ymove = Mathf.Clamp(ymove, -20, 55);
            this.GetComponent<PlayerShooting>().Shotdistance = shoulderPatchingShotDist;
            Vector3 Eye = player.transform.position + TPScam.transform.rotation * Player_Side + Player_Height;
            Vector3 reverseDistance = new Vector3(0.0f, 0.0f, 3.0f);
            TPScam.transform.position = Eye - TPScam.transform.rotation * reverseDistance;
            animator.SetBool("isShoulderPatching", shoulderPatching);
        }

        TPScam.transform.rotation = Quaternion.Euler(ymove, xmove, 0); // 이동량에 따라 카메라의 바라보는 방향을 조정합니다. 
    }
    void Move()
    {
        Vector2 moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        isMove = moveInput.magnitude != 0;
        animator.SetBool("isMove", isMove);

        if (isMove || shoulderPatching)
        {
            Vector3 lookForward = new Vector3(TPScam.transform.forward.x, 0f, TPScam.transform.forward.z).normalized;
            Vector3 lookRight = new Vector3(TPScam.transform.right.x, 0f, TPScam.transform.right.z).normalized;
            Vector3 moveDir = lookForward * moveInput.y + lookRight * moveInput.x;

            player.transform.forward = lookForward;
            transform.position += moveDir * Time.deltaTime * moveSpeed;
        }
    }

    void CheckRide()
    {
        RaycastHit hit;
        if (Physics.Raycast(new Vector3(transform.position.x, 1.0f, transform.position.z),
            transform.forward, out hit, 20.0f))
        {
            if (hit.collider.CompareTag("PlayerMech"))
            {
                if (Input.GetKeyDown(KeyCode.F) && !shoulderPatching && 
                    GetComponent<KeyItemControl>().KeyItem1Get && GetComponent<KeyItemControl>().KeyItem2Get)
                {
                    Debug.Log(hit.transform.name);
                    ride = true;
                    playerMech.GetComponent<Animator>().enabled = true;
                    playerMech.GetComponent<PlayerMechController>().enabled = true;
                    playerMech.GetComponent<MechShooting>().enabled = true;
                }

                if (Input.GetKeyDown(KeyCode.Y))
                {
                    ride = true;
                    playerMech.GetComponent<Animator>().enabled = true;
                    playerMech.GetComponent<PlayerMechController>().enabled = true;
                    playerMech.GetComponent<MechShooting>().enabled = true;
                }
            }
        }
        if (ride)
        {
            GetComponent<KeyItemControl>().rideZone.gameObject.SetActive(false);
            TPSUI.gameObject.SetActive(false);
            TPScam.gameObject.SetActive(false);
            this.gameObject.SetActive(false);
            FPSCam.SetActive(true);
            actionCam.gameObject.SetActive(true);

            this.GetComponent<PlayerShooting>().enabled = false;
            this.enabled = false;
        }
    }

    void CheckHit()
    {
        if(isHitted)
        {
            animator.SetBool("isHit", isHitted);
        }
    }

    void HitAnimationEnd()
    {
        isHitted = false;
        animator.SetBool("isHit", isHitted);
    }
}
