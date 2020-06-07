using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class Boss : BaseStats
{
    [SerializeField]
    private GameObject[] rocks = null;
    private Rigidbody2D[] rocksRb;
    [SerializeField]
    private Vector2 spawnPoint, xPos, yPos;

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

        rocksRb = new Rigidbody2D[rocks.Length];
        for(int i = 0; i < rocks.Length; i++)
            rocksRb[i] = rocks[i].GetComponent<Rigidbody2D>();
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

    public void StartFallingRocks()
    {
        StartCoroutine(FallingRocks());
    }

    IEnumerator FallingRocks()
    {
        for (int i = 0; i < rocks.Length; i++)
        {
            GameObject currentRock = rocks[i];
            rocksRb[i].velocity = Vector2.zero;
            currentRock.SetActive(true);
            currentRock.transform.position = (Vector2)transform.position + spawnPoint + new Vector2(Random.Range(xPos.x, xPos.y), Random.Range(yPos.x, yPos.y));
            yield return new WaitForSeconds(.375f);
        }
        foreach (GameObject rock in rocks)
            rock.SetActive(false);
    }

    public override void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube((Vector2)transform.position + spawnPoint + new Vector2(xPos.x, yPos.x) + new Vector2(xPos.y, yPos.y), new Vector2(xPos.x - xPos.y, yPos.x - yPos.y));
    }
}
