using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingHookCollect : MonoBehaviour
{
    GameObject powerup;
    CapsuleCollider2D collide;
    GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        collide = GetComponent<CapsuleCollider2D>();
        player = GameObject.Find("player");
    }

    // Update is called once per frame
    void Update()
    {
        if (collide.IsTouchingLayers(LayerMask.GetMask("Player"))) {
            Destroy(gameObject);
            player.GetComponent<PlayerController>().chooseGrapplingHook();
        }
    }
}