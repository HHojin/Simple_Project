using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainSceneCtrl : MonoBehaviour
{
    public GameObject game_title;
    public GameObject main_object;

    void Update()
    {
        game_title.transform.LookAt(GameObject.FindGameObjectWithTag("MainCamera").transform);

        main_object.transform.Rotate(new Vector3(0, 10.0f * Time.deltaTime, 0));

        if (Input.GetMouseButton(0))
            Turn_GameScene();
    }

    public void Turn_GameScene()
    {
        SceneManager.LoadScene("Game");
    }
}
