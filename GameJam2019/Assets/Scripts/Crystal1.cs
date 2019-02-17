using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal1 : MonoBehaviour
{
    public int crystalcount = 0;
    // Start is called before the first frame update
    void Start()
    {
        print("starttest");
        crystalcount = 0;
    }

    void OnTriggerEnter2D(Collider2D playerCollider)
    {
        print("player entered collider");
        //if (playerCollider.tag == "Gem")
        //{
        // destroys crystal
        Destroy(gameObject);
        // increments count for number of crystals consumes 
        crystalcount = crystalcount+1;
        // retuns2
        print("Crystal Count =" + crystalcount);

        //}
        //else
        //{
        //    print("object wasnt destroyed");
        //}

        if (playerCollider.gameObject.layer == 10) { // Collided with playte
            PlayerController PC = playerCollider.gameObject.GetComponent<PlayerController>();
            PC.SetEnergy(PC.energy + PlayerController.crystalEnergyBoost);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
