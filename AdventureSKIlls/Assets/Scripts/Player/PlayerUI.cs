using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    private Camera mainCam;
    private Canvas canvas;
    [SerializeField]
    private Text nameText;
    [SerializeField]
    private Slider healthSlider;
    private Transform playerTransform;
    private Vector2 healthPos;

    [SerializeField]
    private float heightOffset;
    [SerializeField]
    private Vector2 healthOffset = new Vector2(0, 5);

    BaseStats main;

    // Start is called before the first frame update
    void Start()
    {
        mainCam = Camera.main;

        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        transform.SetParent(canvas.transform);
    }

    // Update is called once per frame
    void Update()
    {
        if(main == null)
        {
            Destroy(gameObject);
            return;
        }
        print(main.name + ": " + main.health);
        healthSlider.value = main.health;
    }

    private void LateUpdate()
    {
        healthPos = playerTransform.position;
        healthPos.y += heightOffset;

        if (canvas.renderMode == RenderMode.ScreenSpaceCamera)
        {
            transform.position = healthPos + healthOffset;
        }
        else if(canvas.renderMode == RenderMode.ScreenSpaceOverlay)
        {
            if (mainCam == null)
                mainCam = FindObjectOfType<Camera>();
            transform.position = (Vector2)mainCam.WorldToScreenPoint(healthPos) + healthOffset;
        }
    }

    public void SetParent(Transform _main, PhotonView _mainNetwork)
    {

        if (_mainNetwork != null && _main.tag == "Player")
        {
            playerTransform = _mainNetwork.transform;
            nameText.text = _mainNetwork.Owner.NickName;
        }
        else
        {
            playerTransform = _main;
            nameText.text = "";
        }

        main = _main.GetComponent<BaseStats>();
        mainCam = FindObjectOfType<Camera>();

        healthSlider.maxValue = main.maxHealth;
    }
}
