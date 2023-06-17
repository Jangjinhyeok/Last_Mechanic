using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TPSCheckBoxCon : MonoBehaviour
{
    KeyItemControl keyitemcon;
    [SerializeField] GameObject MissionUI;
    [SerializeField] Text rideText;
    [SerializeField] Image check1;
    [SerializeField] Image check2;
    void Awake()
    {
        keyitemcon = GameObject.Find("Player").GetComponent<KeyItemControl>();
    }

    // Update is called once per frame
    void Update()
    {
        if (keyitemcon.KeyItem1Get)
            check1.gameObject.SetActive(true);
        if (keyitemcon.KeyItem2Get)
            check2.gameObject.SetActive(true);

        if (keyitemcon.KeyItem1Get && keyitemcon.KeyItem2Get)
        {
            MissionUI.SetActive(false);
            rideText.gameObject.SetActive(true);
        }

    }
}
