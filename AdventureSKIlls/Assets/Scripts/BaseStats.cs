using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum BaseState
{
    STANDARD,
    ATTACKING,
    HURT,
    DYING
}
public class BaseStats : MonoBehaviourPun
{

    [SerializeField]
    private GameObject healthUI;

    [HideInInspector]
    public bool online;
    public float maxHealth = 100;
    public float health = 100;
    public BaseState state = new BaseState();
    public GameObject[] drops;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    public virtual void Awake()
    {
        online = PhotonNetwork.IsConnected;
        health = maxHealth;
        SetHealthUI();
    }

    public virtual void TakeDamage(float damageTaken)
    {
        if (!online || (online && photonView.IsMine))
        {
            health -= damageTaken;
            if (health <= 0)
                Die();
        }
        else if(online)
        {
            photonView.RPC("TakeDamageRPC", RpcTarget.AllBuffered, damageTaken);
        }
    }

    [PunRPC]
    public void TakeDamageRPC(float damageTaken)
    {
        health -= damageTaken;
        if (health <= 0)
            Die();
    }

    public virtual void Die()
    {
        StopAllCoroutines();
        state = BaseState.DYING;
        DropItems();
    }

    public virtual void DropItems()
    {
        foreach (GameObject drop in drops)
        {
            GameObject newDrop = Instantiate(drop, transform.position, Quaternion.identity);
            Rigidbody2D dropRb = newDrop.GetComponent<Rigidbody2D>();
            Vector2 dropDir = new Vector2(Random.Range(-5, 5), 10);
            dropRb.AddForce(dropDir, ForceMode2D.Impulse);
        }
    }

    private void SetHealthUI()
    {
        PlayerUI healthInstance = Instantiate(healthUI).GetComponent<PlayerUI>();

        healthInstance.SetParent(transform, photonView);
    }

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        SetHealthUI();
    }
}
