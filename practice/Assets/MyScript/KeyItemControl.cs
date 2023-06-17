using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyItemControl : MonoBehaviour
{
    TPSCharacterController tpsCon;
    [SerializeField] Image Castingbar;
    [SerializeField] Text Castingtext;
    public GameObject rideZone;
    public bool KeyItem1Get;
    public bool KeyItem2Get;
    float CastingTime;
    float CastingTimer;
    private void Awake()
    {
        tpsCon = GameObject.Find("Player").GetComponent<TPSCharacterController>();
    }
    void Start()
    {
        CastingTime = 3.0f;
        CastingTimer = 0.0f;
        KeyItem1Get = false;
        KeyItem2Get = false;
        Castingbar.GetComponent<Image>().enabled = false;
        Castingtext.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        CheckKeyitem();
        if (KeyItem1Get && KeyItem2Get)
            rideZone.SetActive(true);
    }

    void CheckKeyitem()
    {
        Castingbar.type = Image.Type.Filled;
        Castingbar.fillMethod = Image.FillMethod.Radial360;
        Castingbar.fillOrigin = (int)Image.Origin360.Top;
        Castingbar.fillClockwise = false;

        RaycastHit hit;
        if(Physics.Raycast(transform.position,transform.forward, out hit, 10f))
        {
            if(hit.collider.gameObject.name == "KeyItem1")
            {

                if(Input.GetKey(KeyCode.F) && !KeyItem1Get)
                {
                    Castingbar.GetComponent<Image>().enabled = true;
                    Castingtext.enabled = true;
                    tpsCon.shoulderPatching = false;
                    CastingTimer += Time.deltaTime;
                    Castingbar.fillAmount = CastingTimer / CastingTime;
                    if (CastingTimer >= CastingTime)
                    {
                        CastingTimer = 0;
                        KeyItem1Get = true;
                        hit.collider.GetComponent<Animator>().enabled = true;
                        Destroy(hit.collider.gameObject,2);
                        Castingbar.GetComponent<Image>().enabled = false;
                        Castingtext.enabled = false;
                    }
                }

                if(Input.GetKeyUp(KeyCode.F))
                {
                    Castingbar.fillAmount = 0;
                    Castingbar.GetComponent<Image>().enabled = false;
                    Castingtext.enabled = false;
                }
            }

            else if(hit.collider.gameObject.name == "KeyItem2")
            {
                if(Input.GetKey(KeyCode.F) && !KeyItem2Get)
                {
                    Castingbar.GetComponent<Image>().enabled = true;
                    Castingtext.enabled = true;
                    tpsCon.shoulderPatching = false;
                    CastingTimer += Time.deltaTime;
                    Castingbar.fillAmount = CastingTimer / CastingTime;
                    if (CastingTimer >= CastingTime)
                    {
                        CastingTimer = 0;
                        KeyItem2Get = true;
                        hit.collider.GetComponent<Animator>().enabled = true;
                        Destroy(hit.collider.gameObject, 2);
                        Castingbar.GetComponent<Image>().enabled = false;
                        Castingtext.enabled = false;
                    }
                }
                if (Input.GetKeyUp(KeyCode.F))
                {
                    Castingbar.fillAmount = 0;
                    Castingbar.GetComponent<Image>().enabled = false;
                    Castingtext.enabled = false;
                }
            }
        }    
    }
}
