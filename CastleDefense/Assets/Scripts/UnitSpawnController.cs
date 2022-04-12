using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class UnitSpawnController : MonoBehaviour
{
    public GameObject[] tmp_unit;
    public GameObject[] warrior_unit;
    public GameObject skeleton;

    public int[] line_count = new int[5] { 0, 0, 0, 0, 0 };
    private int unitType;
    public GameObject tmp; //드래그 하는 동안 생성되는 유닛 저장
    public GameObject enemy; //skeleton 저장
    private GameObject cur_money;
    private GameObject target_object;
    private GameObject cur_hit_object;
    public GameObject[] castleParticles;
    public Camera UI_Camera;

    private RaycastHit hit;

    public Transform[] enemy_spawn_position;

    private bool isSpawned = false;
    private bool isEmpty = true;
    public bool isDefeat = false;
    private bool isEnd = false;

    private int cur_cost;
    private string cur_unit = null;

    public int money = 100;
    public int score = 0;

    public Text[] scoreText;
    public Image endUI;

    void Start()
    {
        cur_money = transform.GetChild(0).GetChild(1).gameObject;
        StartCoroutine(AddMoney());
        StartCoroutine(AddScore());
        StartCoroutine(Spawn_skeleton());
    }

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        cur_money.GetComponent<Text>().text = money.ToString();
        scoreText[0].GetComponent<Text>().text = "SCORE : " + score;

        //UnitBar에서 유닛 터치시 prefab생성
        if (Input.GetMouseButton(0) && cur_unit != null)
        {
            if (!isSpawned)
            {
                if (money >= cur_cost)
                {
                    switch (cur_unit)
                    {
                        case "archer":
                            unitType = 0;
                            break;
                        case "knight":
                            unitType = 1;
                            break;
                        case "magician":
                            unitType = 2;
                            break;
                        default:
                            break;
                    }
                    Spawn_tmp(unitType, -cur_cost);
                }
                else
                    cur_money.GetComponent<Text>().color = new Color(255, 0, 0);
            }
        }

        //드래그중인 유닛 이동
        if (isSpawned)
        {
            Vector3 pos = Input.mousePosition;
            pos.z = UI_Camera.farClipPlane - 4;

            tmp.transform.GetChild(0).position = UI_Camera.ScreenToWorldPoint(pos);
            tmp.transform.GetChild(0).transform.LookAt(GameObject.FindGameObjectWithTag("MainCamera").transform);
        }

        if (Physics.Raycast(ray, out hit))
        {
            cur_hit_object = hit.transform.gameObject;
            if(cur_hit_object.transform.childCount == 0 && cur_hit_object.transform.CompareTag("PlayBoard"))
            {
                //Debug.Log("Empty!");
                isEmpty = true;
            }
            else
            {
                //Debug.Log("Not Empty!");
                isEmpty = false;
            }
        }

        if (Input.GetMouseButtonUp(0) && cur_unit != null)
        {
            if (isSpawned)
            {
                if(isEmpty)
                {
                    target_object = cur_hit_object;

                    Spawn_warrior(unitType);
                }
                else
                {
                    money += cur_cost;
                }

                Destroy(tmp.transform.GetChild(0).gameObject);
            }
            cur_unit = null;
            cur_cost = 0;
            isSpawned = false;

            cur_money.GetComponent<Text>().color = new Color(0, 0, 0);
        }

        //패배
        if (isDefeat && !isEnd)
        {
            isEnd = true;
            StopAllCoroutines();
            StartCoroutine(CastleExplosion());
            Transform[] allEnemies = enemy.GetComponentsInChildren<Transform>();
            foreach(Transform skeleton in allEnemies)
            {
                if (skeleton.CompareTag("Enemy"))
                    skeleton.GetComponent<Animator>().SetBool("isDefeat", true);
            }

            StartCoroutine(ShowResult()); //패배시 결과창
        }
    }

    IEnumerator AddMoney()
    {
        while (true)
        {
            yield return new WaitForSeconds(2f);
            ChangeMoney(2);
        }
    }

    IEnumerator AddScore()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f);
            score += 10;
        }
    }

    IEnumerator Spawn_skeleton()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f);
            int random = Random.Range(0, 5);
            GameObject spawned_skeleton = Instantiate(skeleton, enemy.transform);
            spawned_skeleton.transform.position = new Vector3(enemy_spawn_position[random].position.x,
                                                              enemy_spawn_position[random].position.y,
                                                              enemy_spawn_position[random].position.z);
        }
    }

    private void Spawn_tmp(int i, int cost)
    {
        isSpawned = true;
        Instantiate(tmp_unit[i], tmp.transform);
        ChangeMoney(cost);
    }

    private void Spawn_warrior(int i)
    {
        GameObject clone_warrior = Instantiate(warrior_unit[i], target_object.transform);
        clone_warrior.transform.position = new Vector3(target_object.transform.position.x,
                                                     target_object.transform.position.y + 0.5f,
                                                     target_object.transform.position.z);
        clone_warrior.transform.rotation = target_object.transform.rotation;
    }

    public void ChangeMoney(int cost)
    {
        money += cost;
    }

    public void OnPointerDown(GameObject slot)
    {
        cur_unit = slot.name;
        cur_cost = int.Parse(slot.transform.GetChild(0).GetComponent<Text>().text);
    }

    IEnumerator CastleExplosion()
    {
        foreach(GameObject particle in castleParticles)
        {
            particle.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);
        }
        yield break;
    }

    IEnumerator ShowResult()
    {
        scoreText[1].GetComponent<Text>().text = "SCORE : " + score;
        this.transform.GetComponent<Image>().color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
        for(int i = 0; i < 4; i++)
        {
            this.transform.GetChild(i).gameObject.SetActive(false);
        }
        
        yield return new WaitForSeconds(2f);
        while (true)
        {
            endUI.transform.position = Vector3.MoveTowards(endUI.transform.position, this.gameObject.transform.parent.position, 50.0f * Time.deltaTime);
            yield return null;
            if (endUI.transform.position == this.gameObject.transform.parent.position)
                yield break;
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene("Game");
    }
}