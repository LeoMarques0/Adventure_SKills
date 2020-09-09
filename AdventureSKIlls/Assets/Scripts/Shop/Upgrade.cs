using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgrade : MonoBehaviour
{

    [SerializeField]
    private SpriteRenderer my_Renderer;
    [SerializeField]
    private GameObject upArrowPrefab = null;

    private GameObject upArrowInstance;

    [SerializeField]
    private Vector2 checkSize = Vector2.zero, checkOffset = Vector2.zero;
    [SerializeField]
    private LayerMask checkMask = new LayerMask();

    [SerializeField]
    private int price = 10;

    private bool buyable = true;

    private void Start()
    {
        SetUpUI();
    }

    // Update is called once per frame
    void Update()
    {
        if(DetectPlayer() && buyable)
        {
            Buy();
        }
    }

    private bool DetectPlayer()
    { 
        if(Physics2D.OverlapBox((Vector2)transform.position + checkOffset, checkSize, 0, checkMask))
        {
            return true;
        }

        if(upArrowInstance.activeSelf)
            upArrowInstance.SetActive(false);

        return false;
    }

    private void Buy()
    {
        upArrowInstance.SetActive(true);

        if (Input.GetAxisRaw("Vertical") == 1 && (GameManager.singleton.coins - price) >= 0)
        {
            upArrowInstance.SetActive(false);

            my_Renderer.enabled = false;
            buyable = false;

            GameManager.singleton.coins -= price;
            UpgradeEffect();
        }
    }

    public virtual void UpgradeEffect()
    {

    }

    private void SetUpUI()
    {
        Camera mainCam = FindObjectOfType<Camera>();
        Transform canvas = GameObject.Find("ShopUI").transform;

        upArrowInstance = Instantiate(upArrowPrefab);
        upArrowInstance.transform.SetParent(canvas, false);
        upArrowInstance.transform.position = (Vector2)mainCam.WorldToScreenPoint(transform.position);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube((Vector2)transform.position + checkOffset, checkSize);
    }
}
