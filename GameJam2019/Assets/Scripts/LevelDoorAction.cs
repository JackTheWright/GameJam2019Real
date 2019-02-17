using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelDoorAction : MonoBehaviour {
    CapsuleCollider2D PlayerProximityCollider;
    BoxCollider2D BlockPlayer;
    GameObject player;
    PlayerController pc;
    GameObject canvInteract;
    GameObject canvNeedKey;
    SpriteRenderer rend;
    // Start is called before the first frame update
    void Start() {
        PlayerProximityCollider = GetComponent<CapsuleCollider2D>();
        BlockPlayer = GetComponent<BoxCollider2D>();
        player = GameObject.Find("player");
        pc = player.GetComponent<PlayerController>();
        canvInteract = GameObject.FindGameObjectWithTag("interactUI");
        canvNeedKey = GameObject.FindGameObjectWithTag("needtoUnlock");
        rend = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update() {
        if (CheckUnlocked() == true) {
            if (PlayerProximityCollider.IsTouchingLayers(LayerMask.GetMask("Player"))) {
                canvInteract.transform.GetChild(0).gameObject.SetActive(true);
                if (Input.GetKeyDown(KeyCode.Space)) {
                    BlockPlayer.enabled = false;
                    rend.enabled = false; // door open animation
                    canvInteract.transform.GetChild(0).gameObject.SetActive(false);
                    Destroy(gameObject);
                }
            }
            else if (!PlayerProximityCollider.IsTouchingLayers(LayerMask.GetMask("Player"))) {
                canvInteract.transform.GetChild(0).gameObject.SetActive(false);
            }
        }
        else if (CheckUnlocked() == false) {
            if (PlayerProximityCollider.IsTouchingLayers(LayerMask.GetMask("Player"))) {
                canvNeedKey.transform.GetChild(0).gameObject.SetActive(true);
            }
            else if (!PlayerProximityCollider.IsTouchingLayers(LayerMask.GetMask("Player"))) {
                canvNeedKey.transform.GetChild(0).gameObject.SetActive(false);
            }
        }
    }
    
    bool CheckUnlocked() {
        if (player.GetComponent<PlayerController>().checkKey() == true && pc.checkMinEnergy() == true) {
            return true;
        }
        return false;
    }
}
