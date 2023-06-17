using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPSUIControl : MonoBehaviour
{
    [SerializeField] Image Check;
    [SerializeField] GameObject BigEnemy;
    [SerializeField] GameObject BigEnemy1;
    [SerializeField] GameObject ClearUI;

    GameObject playerMech;

    void Start()
    {
        playerMech = GameObject.Find("PlayerMech");
    }

    // Update is called once per frame
    void Update()
    {
        if (BigEnemy.GetComponent<BigEnemy>().isDead && BigEnemy1.GetComponent<BigEnemy>().isDead)
        {
            Check.gameObject.SetActive(true);
            StartCoroutine(Clear());
        }    
    }

    IEnumerator Clear()
    {
        yield return new WaitForSeconds(4.0f);
        playerMech.GetComponent<PlayerMechController>().FPSUI.SetActive(false);
        playerMech.GetComponent<PlayerMechController>().enabled = false;
        playerMech.GetComponent<MechShooting>().enabled = false;
        ClearUI.gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
    }
}
