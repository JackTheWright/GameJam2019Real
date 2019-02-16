using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRespawnWall : MonoBehaviour
{
    public bool Despawn;
    public float Timer;
    public float Cooldown;
    public GameObject Enemy;
    public string EnemyName;
    GameObject LastEnemy;
    WallEnemy wallenemy;

    void Start()
    {

        this.gameObject.name = EnemyName + "wall spawn point";
        Despawn = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Despawn == true)
        {
            print("Timer");
            Timer += Time.deltaTime;
        }

        if (Timer >= Cooldown)
        {
            print("timer > cooldown");
            //It will create a new Enemy of the same class, at this position.
            Enemy.transform.position = transform.position;

            Instantiate(Enemy);
            LastEnemy = GameObject.Find(Enemy.name + "(Clone)");
            LastEnemy.name = EnemyName;
            //My enemy won't be dead anymore.
            Despawn = false;
            //Timer will restart.
            Timer = 0;
        }
    }
}
