using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class Boss : BaseStats
{

    public bool activated = false;
    bool canAttack = true;
    int attackIndex = 0;

    Animator anim;

    public float attackDelay = 2;

    public override void Awake()
    {
        online = PhotonNetwork.IsConnected;
        health = maxHealth;

        foreach (SpriteRenderer sprite in sprites)
        {
            materials.Add(sprite.material);
        }

        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        switch(state)
        {
            case BaseState.STANDARD:

                if(activated && canAttack)
                {
                    attackIndex = Random.Range(0, 3);
                    state = BaseState.ATTACKING;
                    canAttack = false;
                }
                break;
        }

        UpdateAnimation();
    }

    void UpdateAnimation()
    {
        anim.SetInteger("AttackIndex", attackIndex);
        anim.SetBool("IsAttacking", state == BaseState.ATTACKING);

        anim.SetBool("IsActive", activated);
    }

    public void EndAttack()
    {
        StopAllCoroutines();
        StartCoroutine(AttackDelay());
        state = BaseState.STANDARD;
    }

    IEnumerator AttackDelay()
    {
        yield return new WaitForSeconds(attackDelay);
        canAttack = true;
    }

    public override void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        
    }
}
