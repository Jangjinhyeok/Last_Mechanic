using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMechController : MonoBehaviour
{
    [SerializeField] Camera FPSCam;
    [SerializeField] GameObject PlayerMech;

    public GameObject FPSUI;
    [SerializeField] ImgsFillDynamic HPbar;
    [SerializeField] Image crosshair;

    TPSCharacterController tpscharactercontroller;
    Animator animator;


    public float speed;
    float xmove, ymove;
    public float rot;
    bool isMove;
    public bool isStart;

    public bool isHitMech;
    float shakeTimer;
    float shakeTime;
    float shakeAmount;
    Vector3 SaveCamPos;

    public float HP;

    private void Awake()
    {
        tpscharactercontroller = GameObject.Find("Player").GetComponent<TPSCharacterController>();
        animator = GetComponent<Animator>();
    }
    void Start()
    {
        HP = 100f;
        speed = 5f;
        rot = 200f;
        isStart = false;

        shakeTime = 0.4f;
        shakeTimer = 0;
        shakeAmount = 0.3f;
        isHitMech = false;
    }

    void Update()
    {
        if(!isStart)
        {
            StartCoroutine(StartRide());
        }
        if(isStart)
        {
            HPbar.SetValue(HP/100f);
            if(!isHitMech)
                Move();
            CamControl();
            HitCameraShake();
        }

        if(Input.GetKeyDown(KeyCode.Escape))
            Cursor.lockState = CursorLockMode.Confined;

    }

    IEnumerator StartRide()
    {
        animator.SetFloat("isRide", 1.0f);
        yield return new WaitForSeconds(3.5f);
        this.GetComponent<CapsuleCollider>().center = new Vector3(0, 1, 0);
        isStart = true;
        animator.SetBool("isStart", true);
        tpscharactercontroller.actionCam.gameObject.SetActive(false);
        FPSUI.SetActive(true);
    }

    void Move()
    {
        Vector2 moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        isMove = moveInput.magnitude != 0;
        animator.SetBool("isMove", isMove);
        Vector3 lookForward = new Vector3(FPSCam.transform.forward.x, 0, FPSCam.transform.forward.z).normalized;
        Vector3 lookRight = new Vector3(FPSCam.transform.right.x, FPSCam.transform.forward.y, FPSCam.transform.right.z).normalized;
        Vector3 moveDir = lookForward * moveInput.y + lookRight * moveInput.x;

        transform.forward = lookForward;
        if (transform.position.y < 0.0f)
            transform.position = new Vector3(transform.position.x, 0.0f, transform.position.z);
        transform.position += moveDir * Time.deltaTime * speed;
    }

    void CamControl()
    {
        xmove += Input.GetAxis("Mouse X");
        ymove -= Input.GetAxis("Mouse Y");

        ymove = Mathf.Clamp(ymove, -40, 20);
        FPSCam.transform.eulerAngles = new Vector3(ymove, xmove, 0);
    }

    void HitCameraShake()
    {
        if(isHitMech)
        {
            shakeTimer += Time.deltaTime;

            if (shakeTimer < shakeTime)
            {
                FPSCam.transform.position = Random.insideUnitSphere * shakeAmount + SaveCamPos;
            }
            else
            {
                isHitMech = false;
                shakeTimer = 0;
                FPSCam.transform.position = SaveCamPos;
            }
        }
        else
        {
            SaveCamPos = FPSCam.transform.position;
        }

    }
}
