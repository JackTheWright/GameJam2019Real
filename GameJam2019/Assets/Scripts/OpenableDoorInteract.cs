using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenableDoorInteract : MonoBehaviour
{
    CapsuleCollider2D PlayerProximityCollider;
    BoxCollider2D BlockPlayer;
    GameObject player;
    public GameObject canv;
    SpriteRenderer rend;
    // Start is called before the first frame update
    void Start()
    {
        PlayerProximityCollider = GetComponent<CapsuleCollider2D>();
        BlockPlayer = GetComponent<BoxCollider2D>();
        player = GameObject.Find("player");
        canv = GameObject.FindGameObjectWithTag("interactUI");
        rend = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerProximityCollider.IsTouchingLayers(LayerMask.GetMask("Player"))) {
                canv.transform.GetChild(0).gameObject.SetActive(true);
                if (Input.GetKeyDown(KeyCode.Space)) {
                    BlockPlayer.enabled = false;
                    rend.enabled = false; // door open animation
                    canv.transform.GetChild(0).gameObject.SetActive(false);
                    Destroy(gameObject);
                }
            }
            else if (!PlayerProximityCollider.IsTouchingLayers(LayerMask.GetMask("Player"))) {
                canv.transform.GetChild(0).gameObject.SetActive(false);
            }
    }
}
