using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UltControl : MonoBehaviour
{
    [SerializeField] GameObject UltEffect;
    Vector3 RandomPos;
    float Timer;
    [SerializeField] float Cool;
    void Start()
    {
        Timer = 0;
        Cool = 2.0f;
    }

    void Update()
    {
        Timer += Time.deltaTime;
        if(Timer >= Cool)
        {
            GetRandomPos();
            Instantiate(UltEffect, RandomPos, this.transform.rotation);
            Timer = 0;
        }
    }

    void GetRandomPos()
    {
        float r = 30f;
        float a = transform.position.x;
        float b = transform.position.z;

        float x = Random.Range(-r + a, r + a);
        float y_b = Mathf.Sqrt(Mathf.Pow(r, 2) - Mathf.Pow(x - a, 2));
        y_b *= Random.Range(0, 2) == 0 ? -1 : 1;
        float y = y_b + b;
        RandomPos = new Vector3(x, 1, y);
    }
}
