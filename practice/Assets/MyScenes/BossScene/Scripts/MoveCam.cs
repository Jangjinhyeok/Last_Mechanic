using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCam : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] Transform camGoal;
    GameObject playerCam;

    void Start()
    {
        playerCam = GameObject.Find("PlayerMech").transform.Find("FPScam").gameObject;
        transform.position = playerCam.transform.position;
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, camGoal.position, speed * Time.deltaTime);
    }
}
