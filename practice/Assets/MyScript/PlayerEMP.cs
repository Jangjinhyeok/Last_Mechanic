using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEMP : MonoBehaviour
{
    [SerializeField] GameObject EMPeffect;
    void Start()
    {
        StartCoroutine(Explosion());
    }

    void Update()
    {
    }

    IEnumerator Explosion()
    {
        yield return new WaitForSeconds(3.0f);
        GameObject instanceEMP = Instantiate(EMPeffect, transform.position, transform.rotation);
        RaycastHit[] rayHits = Physics.SphereCastAll(transform.position, 8, Vector3.up, 0f, LayerMask.GetMask("Enemy"));
        foreach(RaycastHit hitObj in rayHits)
        {
            if(hitObj.collider.CompareTag("BigEnemy"))
            {
                hitObj.transform.GetComponent<BigEnemy>().HitEMP();
            }
        }
        Destroy(instanceEMP, 2.0f);
        Destroy(gameObject);
    }
}
