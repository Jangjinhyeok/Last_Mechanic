using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MechDisplayControl1 : MonoBehaviour
{
    PlayerMechController1 mechcon;
    // Start is called before the first frame update
    void Start()
    {
        mechcon = GameObject.Find("PlayerMech").GetComponent<PlayerMechController1>();
    }

    // Update is called once per frame
    void Update()
    {
        if (mechcon.HP > 60)
            this.GetComponent<Image>().color = new Color32(255,255,255,80);
        else if (mechcon.HP < 60 && mechcon.HP > 30)
            this.GetComponent<Image>().color = new Color32(255,188,0,80);
        else if (mechcon.HP < 30)
            this.GetComponent<Image>().color = new Color32(255,0,0,80);
    }
}
