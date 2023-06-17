using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossUIControl : MonoBehaviour
{

    [SerializeField] Slider HPbar;
    [SerializeField] Slider Ultbar;
    [SerializeField] GameObject bossBar;
    [SerializeField] GameObject missonBox;
    [SerializeField] GameObject ClearUI;

    GameObject player;
    Panda.BossBT.BossAI bossai;
    private void Start()
    {
        bossai = GameObject.Find("Boss").GetComponent<Panda.BossBT.BossAI>();
        player = GameObject.Find("PlayerMech");
    }

    // Update is called once per frame
    void Update()
    {

        if(bossai.BossAlive)
        {
            HPbar.maxValue = bossai.MaxBossHP;
            HPbar.value = bossai.BossHP;

            if(bossai.phaseStart)
            {
                bossBar.SetActive(true);
                missonBox.SetActive(false);
            }

            if(bossai.Phase2)
            {
                Ultbar.gameObject.SetActive(true);
                Ultbar.maxValue = bossai.MaxUltGauge;
                Ultbar.value = bossai.UltGauge;
            }
        }
        else
        {
            StartCoroutine(Clear());
        }
    }

    IEnumerator Clear()
    {
        yield return new WaitForSeconds(3.0f);
        player.GetComponent<PlayerMechController1>().FPSUI.SetActive(false);
        player.GetComponent<PlayerMechController1>().enabled = false;
        player.GetComponent<PlayerMechShooting>().enabled = false;
        ClearUI.gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.None;

    }
}
