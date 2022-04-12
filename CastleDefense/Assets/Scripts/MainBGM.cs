using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainBGM : MonoBehaviour
{
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }
}