using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoint : MonoBehaviour
{
    //public GameObject m_character;

    //private bool m_doorIsOpening;
    public Animator m_openandclose;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            StartCoroutine(Opening());
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            StartCoroutine(Closing());
        }

    }

    IEnumerator Opening()
    {
        Debug.Log("opening the door");
        m_openandclose.Play("Opening");
        //m_doorIsOpening = true;
        yield return new WaitForSeconds(.5f);
    }

    IEnumerator Closing()
    {
        Debug.Log("closing the door");
        m_openandclose.Play("Closing");
        //m_doorIsOpening = false;
        yield return new WaitForSeconds(.5f);
    }
}
