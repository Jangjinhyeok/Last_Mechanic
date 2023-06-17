using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGunEffectControl : MonoBehaviour
{
    [SerializeField] float bulletSpeed;
    TPSCharacterController tpscharactercontroller;

    private void Awake()
    {
        tpscharactercontroller = GameObject.Find("Player").GetComponent<TPSCharacterController>();
    }
    void Start()
    {
    }

    void Update()
    {
        transform.position += tpscharactercontroller.transform.forward.normalized * bulletSpeed;
    }
}
