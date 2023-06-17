using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMechController1 : MonoBehaviour
{
    public Camera FPSCam;
    public GameObject FPSUI;

    [SerializeField] GameObject PlayerMech;
    [SerializeField] ImgsFillDynamic HPbar;
    [SerializeField] Image crosshair;
    Animator animator;


    public float speed;
    float xmove, ymove;
    public float rot;
    bool isMove;

    public bool bossAppear;

    public bool isHitMech;
    float shakeTimer;
    float shakeTime;
    float shakeAmount;
    Vector3 SaveCamPos;

    public float HP;

    [SerializeField] GameObject EMPUI;
    public bool HitEMP;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        Cursor.lockState = CursorLockMode.Locked;
    }
    void Start()
    {
        HP = 100f;
        speed = 8f;
        rot = 200f;

        shakeTime = 0.4f;
        shakeTimer = 0;
        shakeAmount = 0.3f;
        isHitMech = false;
        HitEMP = false;
        bossAppear = false;
    }

    void Update()
    {
        HPbar.SetValue(HP/100f);
        HitCameraShake();
        if (!HitEMP)
        {
            if (!isHitMech)
                Move();
            CamControl();
        }
        else
            StartCoroutine(EMPeffect());
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

    IEnumerator EMPeffect()
    {
        EMPUI.SetActive(true);
        animator.SetBool("isMove", false);
        yield return new WaitForSeconds(2.5f);
        HitEMP = false;
        EMPUI.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("EMP"))
            HitEMP = true;

        if (other.gameObject.CompareTag("JoomTrigger"))
        {
            bossAppear = true;
        }
    }
}
