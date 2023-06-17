using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleControl : MonoBehaviour
{
    public GameObject Info;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TurnOnInfo()
    {
        Info.SetActive(true);
    }

    public void TurnOffInfo()
    {
        Info.SetActive(false);
    }
}
