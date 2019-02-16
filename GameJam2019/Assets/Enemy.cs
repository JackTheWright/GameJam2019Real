using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1f;
    Rigidbody2D myRigidBody;
    public int hp = 1;
    public int dmg = 1;
    private int countToDespawn = 0;
    public Vector3 position; 
    // Start is called before the first frame update
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        countToDespawn = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsFacingRight())
        {
            myRigidBody.velocity = new Vector2(-moveSpeed, 0f);
            countToDespawn++;
        }
        else
        {
            myRigidBody.velocity = new Vector2(moveSpeed, 0f);
            countToDespawn++;
        }

        if (countToDespawn == 3) {
            Destroy(gameObject);

        }
    }

    bool IsFacingRight()
    {
        return transform.localScale.x > 0;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        transform.localScale = new Vector2(-(Mathf.Sign(myRigidBody.velocity.x)), 1f);
    }
}
