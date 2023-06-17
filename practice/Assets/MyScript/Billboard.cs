using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    [SerializeField] GameObject FPScam;
    [SerializeField] GameObject TPScam;
    Transform cam;
    void Start()
    {
        cam = TPScam.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(TPScam.gameObject.activeSelf)
            transform.LookAt(transform.position + cam.rotation * Vector3.forward, cam.rotation * Vector3.up);
        else
        {
            cam = FPScam.transform;
            transform.LookAt(transform.position + cam.rotation * Vector3.forward, cam.rotation * Vector3.up);
        }

    }
}
