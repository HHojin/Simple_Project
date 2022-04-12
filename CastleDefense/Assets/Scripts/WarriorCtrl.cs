using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorCtrl : MonoBehaviour
{
    private Animator animator;

    public int HP;
    public int DMG;
    private int unitType;
    public int line;

    private bool isAttack = false; //knight
    private bool isSoundActive = false;

    public GameObject[] archer_arrow;
    public GameObject[] mage_fireball;
    public GameObject throws;

    AudioSource audioSource;
    public AudioClip sword_collide;
    public AudioClip warrior_dead;

    private UnitSpawnController unitSpawnController;

    void Start()
    {
        animator = this.GetComponent<Animator>();
        audioSource = this.gameObject.transform.parent.GetComponent<AudioSource>();
        throws = GameObject.FindWithTag("throws");
        unitSpawnController = GameObject.FindWithTag("GameController").GetComponent<UnitSpawnController>();
        UnitCheck(this.transform.name);
    }

    void Update()
    {
        if (unitType != 1)
        {
            if (unitSpawnController.line_count[int.Parse(this.transform.parent.parent.parent.name)] > 0)
            {
                animator.SetBool("isEnemy", true);
            } else
            {
                animator.SetBool("isEnemy", false);
            }
        }

        if (HP <= 0 || unitSpawnController.isDefeat)
        {
            animator.SetBool("isDead", true);

            if (!isSoundActive)
            {
                isSoundActive = true;
                audioSource.PlayOneShot(warrior_dead);
            }

            Vector3 target_position = new Vector3(this.transform.position.x, 0.0f, this.transform.position.z);
            this.transform.position = Vector3.MoveTowards(this.transform.position, target_position, 0.05f);
            if (this.transform.position.y <= 0.0f)
                Destroy(this.gameObject);
        }
    }

    private void UnitCheck(string name)
    {
        line = int.Parse(this.transform.parent.parent.parent.name);

        switch (name)
        {
            case "Archer":
                HP = 15;
                DMG = 3;
                unitType = 0;
                break;
            case "Knight":
                HP = 20;
                DMG = 2;
                unitType = 1;
                break;
            case "Mage":
                HP = 10;
                DMG = 4;
                unitType = 2;
                break;
        }
    }

    //animator event
    public void ShotThrows(int unit_type)
    {
        if (unit_type == 0)
        {
            GameObject arrow = Instantiate(archer_arrow[0]);
            arrow.transform.parent = throws.transform;
            arrow.transform.position = archer_arrow[1].transform.position;
            StartCoroutine(ArrowCollisionCheck(arrow));
        }
        else
        {
            GameObject fireBall = Instantiate(mage_fireball[0]);
            fireBall.transform.parent = throws.transform;
            fireBall.transform.position = mage_fireball[1].transform.position;
            StartCoroutine(FirBallCollisionCheck(fireBall));
        }
    }

    IEnumerator ArrowCollisionCheck(GameObject arrow)
    {
        Arrow arrow_prefab = arrow.GetComponent<Arrow>();
        while (true)
        {
            if (arrow_prefab.isArrowCollision)
            {
                SkeletonCtrl skeleton = arrow_prefab.arrowDectedEnemy.GetComponent<SkeletonCtrl>();
                skeleton.HP -= DMG;
                Destroy(arrow.gameObject);
                yield break;
            }
            yield return null;
        }
    }

    IEnumerator FirBallCollisionCheck(GameObject fireBall)
    {
        Fireball fireBall_prefab = fireBall.GetComponent<Fireball>();
        while (true)
        {
            if (fireBall_prefab.isFireBallCollision)
            {
                SkeletonCtrl skeleton = fireBall_prefab.fireBallDectedEnemy.GetComponent<SkeletonCtrl>();
                skeleton.HP -= DMG;
                Destroy(fireBall.gameObject);
                yield break;
            }
            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Enemy") && unitType == 1)
        {
            animator.SetBool("isEnemy", true);
            StartCoroutine(AttackCheck_melee(other.gameObject));
        }
    }

    IEnumerator AttackCheck_melee(GameObject unit)
    {
        SkeletonCtrl skeletonCtrl = unit.GetComponent<SkeletonCtrl>();

        while (unit.name != null)
        {
            if (skeletonCtrl.HP <= 0 || unit.name == null)
            {
                animator.SetBool("isEnemy", false);
                yield break;
            }

            if (isAttack)
            {
                skeletonCtrl.HP -= DMG;
                isAttack = false;
            }

            yield return null;
        }

        animator.SetBool("isEnemy", false);
        yield break;
    }

    //Animator event
    public void SwordSound()
    {
        isAttack = true;
        audioSource.clip = sword_collide;
        audioSource.Play();
    }
}