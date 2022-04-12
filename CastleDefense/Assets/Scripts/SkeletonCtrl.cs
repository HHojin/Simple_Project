using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonCtrl : MonoBehaviour
{
    private Animator animator;
    private bool isCheck = false;

    public int HP = 15;
    public int DMG = 5;
    public int line;

    private bool isAttack = false;
    private bool isSoundActive = false;

    AudioSource audioSource;
    public AudioClip sword_collide;
    public AudioClip skeleton_dead;

    private UnitSpawnController unitSpawnController;

    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = this.gameObject.GetComponent<AudioSource>();
        unitSpawnController = GameObject.FindWithTag("GameController").GetComponent<UnitSpawnController>();
    }

    void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
        {
            transform.position += new Vector3(-1f * Time.deltaTime, 0, 0);
        }

        if(HP <= 0)
        {
            animator.SetBool("isDead", true);
            this.gameObject.GetComponent<BoxCollider>().enabled = false;

            if (!isSoundActive)
            {
                isSoundActive = true;
                audioSource.PlayOneShot(skeleton_dead);
            }

            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Death") &&
                animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.99f)
            {
                unitSpawnController.money += 20;
                unitSpawnController.score += 100;
                unitSpawnController.line_count[line] -= 1;
                Destroy(this.gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Unit"))
        {
            animator.SetBool("isUnit", true);
            if (!isCheck)
            {
                StartCoroutine(AttackCheck(other));
            }
        }

        if (other.transform.CompareTag("Respawn"))
        {
            line = int.Parse(other.name);
            unitSpawnController.line_count[line] += 1;
        }

        if (other.transform.CompareTag("Castle"))
        {
            animator.SetBool("isDefeat", true);
            unitSpawnController.isDefeat = true;
            StopAllCoroutines();
        }
    }

    IEnumerator AttackCheck(Collider unit)
    {
        isCheck = true;
        WarriorCtrl warriorCtrl = unit.GetComponent<WarriorCtrl>();

        while (unit.name != null)
        {
            if (warriorCtrl.HP <= 0 || unit.name == null)
            {
                isCheck = false;
                animator.SetBool("isUnit", false);
                yield break;
            }

            if(isAttack)
            {
                warriorCtrl.HP -= DMG;
                isAttack = false;
            }

            yield return null;
        }

        isCheck = false;
        animator.SetBool("isUnit", false);
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
