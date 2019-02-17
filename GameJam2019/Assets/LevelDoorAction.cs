using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelDoorAction : MonoBehaviour {
    CapsuleCollider2D PlayerProximityCollider;
    BoxCollider2D BlockPlayer;
    GameObject player;
    bool check;
    public GameObject canv;
    // Start is called before the first frame update
    void Start() {
        PlayerProximityCollider = GetComponent<CapsuleCollider2D>();
        BlockPlayer = GetComponent<BoxCollider2D>();
        player = GameObject.Find("player");
        check = true;
        canv = GameObject.FindGameObjectWithTag("interactUI");
    }

    // Update is called once per frame
    void Update() {
        if (check == true) {//CheckUnlocked() == True) {
            if (PlayerProximityCollider.IsTouchingLayers(LayerMask.GetMask("Player")) == true) {
                canv.transform.GetChild(0).gameObject.SetActive(true);
                print("yeet");
                //BlockPlayer.Enabled = False;
                //Door open animation
                //Disable Player Controls
                //while (player.getComponent<CapsuleCollider)
                 //   player.getComponent<RigidBody2D>.velocity += new Vector2(3, 0);
            }
            else {
                canv.transform.GetChild(0).gameObject.SetActive(false);
            }
        }
    }
    
    /*bool CheckUnlocked() {
        if (player.GetKey == True && player.AtMinEnergy == True) {
            return True;
        }
        return False;
    }*/
}
