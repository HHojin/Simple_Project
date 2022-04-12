using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public bool isArrowCollision = false;
    private float time = 0.0f;
    public GameObject arrowDectedEnemy;

    void Update()
    {
        time += Time.deltaTime;
        if (time >= 5.0f)
        {
            Destroy(this.gameObject);
        }
        transform.position += new Vector3(3.0f * Time.deltaTime, 0, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Enemy"))
        {
            arrowDectedEnemy = other.gameObject;
            isArrowCollision = true;
        }
    }
}
