using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerController : MonoBehaviour
{
    CapsuleCollider2D mybodycollider;
    BoxCollider2D myfeetcollider;
    private Rigidbody2D rb2d;

    [SerializeField] float jumpSpeed = 5f;

    public float speed = 10f;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        mybodycollider = GetComponent<CapsuleCollider2D>();
        myfeetcollider = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        if (myfeetcollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            //myanimator.SetBool("jump", false);
            //myanimator.SetBool("Fall", false);
            //if (Input.GetKey(KeyCode.Mouse0))
            //{
            //    xpos = myrigidbody.position.x;
            //    print("getting stuck in here?");
            //    HitIt();
            //    lastattacktime = Time.time;
            //    animated = true;

            //}
            Run();
           //FlipSprite();
            Jump();
        }
        else if (!myfeetcollider.IsTouchingLayers(LayerMask.GetMask("Ground")) && (rb2d.velocity.y < -1))
        {
            print("does reach?");
            //myanimator.SetBool("Fall", true);
            Run();
            //FlipSprite();
        }
        else
        {
            Run();
            //FlipSprite();
        }
    }

    void Run() {
        if (Input.GetKey("a"))
        {
            rb2d.position = rb2d.position + (Vector2.left * speed * Time.deltaTime);
            transform.localScale = new Vector2(-1, 1);
        }
        if (Input.GetKey("d"))
        {
            rb2d.position = rb2d.position + (Vector2.right * speed * Time.deltaTime);
            transform.localScale = new Vector2(1, 1);
        }
    }

    private void FlipSprite()
    {

        bool playerHorizonVelocity = Mathf.Abs(rb2d.velocity.x) > Mathf.Epsilon;

        if (playerHorizonVelocity)
        {
            transform.localScale = new Vector2((float)(Mathf.Sign(rb2d.velocity.x) * 1f), 1f);
        }
    }

    private void Jump()
    {
        if (!myfeetcollider.IsTouchingLayers(LayerMask.GetMask("Ground"))) { return; }

        if (CrossPlatformInputManager.GetButtonDown("Jump"))
        {
            //attackflag = 0;
            //myanimator.SetBool("jump", true);
            Vector2 jumpUp = new Vector2(0f, jumpSpeed);
            rb2d.velocity += jumpUp;
        }
    }
}
