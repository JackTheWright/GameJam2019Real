using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal1 : MonoBehaviour
{
    GameObject player;
    PlayerController PC;
    // Start is called before the first frame update
    void Start()
    {
        
        player = GameObject.Find("player");
        PC = player.GetComponent<PlayerController>();
        print("starttest");
    }

    void OnTriggerEnter2D(Collider2D playerCollider)
    {
        print("player entered collider");
        //if (playerCollider.tag == "Gem")
        //{
        // destroys crystal
        
        // increments count for number of crystals consumes 
        PC.crystalCount += 1;
        // retuns2
        print("Crystal Count =" + PC.crystalCount);

        //}
        //else
        //{
        //    print("object wasnt destroyed");
        //}

        if (playerCollider.gameObject.layer == 12) { // Collided with playte
            PlayerController PC = playerCollider.gameObject.GetComponent<PlayerController>();
            PC.SetEnergy(PC.energy + PlayerController.crystalEnergyBoost);
            if (PC.crystalCount >= 8) {
                PC.trueMinEnergy();
            }
            
        }
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
