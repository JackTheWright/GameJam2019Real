using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float Timer;
    public float moveSpeed = 1f;
    Rigidbody2D myRigidBody;
    public int hp = 1;
    public int dmg = 1;
    public int countToDespawn = 0;
    public Vector3 position;
    Animator myanimator;
    // Start is called before the first frame update
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        countToDespawn = 0;
        myanimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (countToDespawn >= 6)
        {
            GameObject.Find(gameObject.name + ("spawn point")).GetComponent<EnemyRespawn>().Despawn = true;
            myanimator.SetTrigger("Dissapear");
            myRigidBody.velocity = new Vector2(0f, 0f);
            Timer += Time.deltaTime;
            if (Timer >= 2f) {
                Destroy(gameObject);
            }

        }
        if (IsFacingRight())
        {
            myRigidBody.velocity = new Vector2(-moveSpeed, 0f);
        }
        else
        {
            myRigidBody.velocity = new Vector2(moveSpeed, 0f);
        }


    }

    bool IsFacingRight()
    {
        return transform.localScale.x > 0;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        countToDespawn++;

        transform.localScale = new Vector2((Mathf.Sign(myRigidBody.velocity.x)), 1f);
    }
}
