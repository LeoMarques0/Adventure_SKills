using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class Boss : BaseStats
{
    [SerializeField]
    private GameObject skeleton = null;
    [SerializeField]
    private Transform[] spawnPositions = null;
    private List<GameObject> skeletonsSpawned = new List<GameObject>();

    [SerializeField]
    private GameObject[] rocks = null;
    private Rigidbody2D[] rocksRb;

    [SerializeField]
    private Vector2 spawnPoint, xPos, yPos;

    public bool activated = false;
    bool canAttack = true;
    bool validAttack = false;
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
                    while (!validAttack)
                    {
                        attackIndex = Random.Range(0, 3);
                        if (!(attackIndex == 2 && skeletonsSpawned.Count > 0))
                            validAttack = true;
                    }
                    state = BaseState.ATTACKING;
                    canAttack = false;
                    validAttack = false;
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
            rocksRb[i].velocity = Vector2.zero;
            Vector2 spawnPosition = (Vector2)transform.position + spawnPoint + new Vector2(Random.Range(xPos.x, xPos.y), Random.Range(yPos.x, yPos.y));
            photonView.RPC("ActivateRock", RpcTarget.AllBuffered, i, spawnPosition);
            yield return new WaitForSeconds(.375f);
        }
        foreach (GameObject rock in rocks)
            rock.SetActive(false);
    }

    [PunRPC]
    void ActivateRock(int i, Vector2 spawnPos)
    {
        rocks[i].SetActive(true);
        rocks[i].transform.position = spawnPos;
        rocks[i].transform.eulerAngles = new Vector3(0, 0, Random.Range(0f, 360f));
    }

    public void SpawnSkeletons()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Transform[] spawnsChosen;
            spawnsChosen = ChooseSpawners();

            for (int i = 0; i < spawnsChosen.Length; i++)
            {
                GameObject newSkeleton = PhotonNetwork.Instantiate(skeleton.name, spawnsChosen[i].position, Quaternion.identity);
                skeletonsSpawned.Add(newSkeleton);
            }
        }
        StartCoroutine(CheckSkeletonAmount());
    }

    Transform[] ChooseSpawners()
    {
        Transform[] spawnsChosen = new Transform[3];
        for (int i = 0; i < 3; i++)
        {
            switch (i)
            {
                case 0:

                    spawnsChosen[i] = spawnPositions[Random.Range(0, spawnPositions.Length)];

                    break;
                case 1:

                    spawnsChosen[i] = spawnsChosen[i - 1];
                    while (spawnsChosen[i] == spawnsChosen[i - 1])
                        spawnsChosen[i] = spawnPositions[Random.Range(0, spawnPositions.Length)];

                    break;
                case 2:

                    spawnsChosen[i] = spawnsChosen[i - 1];
                    while (spawnsChosen[i] == spawnsChosen[i - 1] || spawnsChosen[i] == spawnsChosen[i - 2])
                        spawnsChosen[i] = spawnPositions[Random.Range(0, spawnPositions.Length)];

                    break;
            }
        }
        return spawnsChosen;
    }

    IEnumerator CheckSkeletonAmount()
    {
        while(skeletonsSpawned.Count > 0)
        {
            foreach (GameObject skeleton in skeletonsSpawned)
            {
                if (skeleton == null)
                    skeletonsSpawned.Remove(skeleton);
                yield return null;
            }
        }
    }

    public override void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube((Vector2)transform.position + spawnPoint + new Vector2(xPos.x, yPos.x) + new Vector2(xPos.y, yPos.y), new Vector2(xPos.x - xPos.y, yPos.x - yPos.y));
    }
}
