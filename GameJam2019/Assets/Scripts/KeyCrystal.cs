using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyCrystal : MonoBehaviour
{

    public bool crystalKeyGot;

    void Start()
    {
        crystalKeyGot = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D playerCollider)
    {
        print("player entered collider");
        //if (playerCollider.tag == "Gem")
        //{
        // destroys crystal
        // IM jack and Im cool
        Destroy(gameObject);
        // retuns2

        //}
        //else
        //{
        //    print("object wasnt destroyed");
        //}

        if (playerCollider.gameObject.layer == 12)
        { // Collided with playte
            PlayerController PC = playerCollider.gameObject.GetComponent<PlayerController>();
            PC.hasKey = true;
        }
    }
}
