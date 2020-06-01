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
    private List<Material> materials = new List<Material>();

    [HideInInspector]
    public bool online;
    public float maxHealth = 100;
    public float health = 100;
    public BaseState state = new BaseState();
    public GameObject[] drops;
    public SpriteRenderer[] sprites;

    public AudioSource damage;
    public AudioClip[] PDamage;

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

        foreach(SpriteRenderer sprite in sprites)
        {
            materials.Add(sprite.material);
        }

        damage = GetComponent<AudioSource>();
    }

    public virtual void TakeDamage(float damageTaken, Collider2D col)
    {
        health -= damageTaken;
        damage.clip = PDamage[Random.Range(0, 4)];

        if (health <= 0)
            Die();
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

    public IEnumerator FlashSprite(float timeFlashing, int amountToFlash)
    {
        float timeEachFlash = timeFlashing / amountToFlash;
        int flassTransparency = 1;

        for(int x = 0; x < amountToFlash; x++)
        {
            foreach(Material m in materials)
            {
                m.SetFloat("_FlashTransparency", flassTransparency);
            }
            flassTransparency = flassTransparency == 1 ? 0 : 1;
            yield return new WaitForSeconds(timeEachFlash);
        }

        foreach (Material m in materials)
        {
            m.SetFloat("_FlashTransparency", 0);
        }
    }

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        SetHealthUI();
    }
}
