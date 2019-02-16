using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerController : MonoBehaviour {
    CapsuleCollider2D mybodycollider;
    BoxCollider2D myfeetcollider;
    private Rigidbody2D rb2d;


    enum UpgradeState { None, Jetpack, MagnetTreads };
    UpgradeState currentUpgradeState = UpgradeState.Jetpack;
    [SerializeField] float jumpSpeed = 12f;


    // Jetpack Variables
    private const float jetPackVelocity = 2f;
    private const float maxUpwardsVelocity = 23f;
    private const float maxJetpackTime = 2.0f;
    private float remainingJetpackTime = maxJetpackTime;

    public float speed = 7f;

    void Start() {
        rb2d = GetComponent<Rigidbody2D>();
        mybodycollider = GetComponent<CapsuleCollider2D>();
        myfeetcollider = GetComponent<BoxCollider2D>();
    }

    void Update() {
        if (myfeetcollider.IsTouchingLayers(LayerMask.GetMask("Ground"))) {
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
            Jump();
        }
        else if (!myfeetcollider.IsTouchingLayers(LayerMask.GetMask("Ground")) && (rb2d.velocity.y < -1)) {
            //myanimator.SetBool("Fall", true);
        }
        Run();
        //FlipSprite();
        UseUpgrade();
    }

    void Run() {
        if (Input.GetKey("a")) {
            rb2d.position = rb2d.position + (Vector2.left * speed * Time.deltaTime);
            transform.localScale = new Vector2(-1, 1);
        }
        if (Input.GetKey("d")) {
            rb2d.position = rb2d.position + (Vector2.right * speed * Time.deltaTime);
            transform.localScale = new Vector2(1, 1);
        }
    }

    private void FlipSprite() {

        bool playerHorizonVelocity = Mathf.Abs(rb2d.velocity.x) > Mathf.Epsilon;

        if (playerHorizonVelocity) {
            transform.localScale = new Vector2((float)(Mathf.Sign(rb2d.velocity.x) * 1f), 1f);
        }
    }

    private void Jump() {
        if (!myfeetcollider.IsTouchingLayers(LayerMask.GetMask("Ground"))) { return; }

        if (CrossPlatformInputManager.GetButtonDown("Jump")) {
            //attackflag = 0;
            //myanimator.SetBool("jump", true);
            Vector2 jumpUp = new Vector2(0f, jumpSpeed);
            rb2d.velocity += jumpUp;
        }
    }

    // Use the current upgrade selected, based on currentUpgradeState
    private void UseUpgrade() {
        switch (currentUpgradeState) {
            case UpgradeState.None:
                break;
            case UpgradeState.Jetpack:
                UseJetpack();
                break;
        }
    }

    // Allow player to utilize jetpack
    // Handle resources of jetpack as the player uses it
    private void UseJetpack() {

        // Case 1: Player is using jetpack
        if (Input.GetKey(KeyCode.LeftShift) &&
                ((remainingJetpackTime > 0 && !IsGrounded()) ||
                (remainingJetpackTime > 1 && IsGrounded()))) {

            // Use jetpack
            Vector2 jetpackUp = new Vector2(0f, jetPackVelocity);
            rb2d.velocity += jetpackUp;
            if (rb2d.velocity.y > maxUpwardsVelocity) {
                rb2d.velocity = new Vector2(rb2d.velocity.x, maxUpwardsVelocity);
            }

            // Decrement resources
            remainingJetpackTime -= Time.deltaTime;
            remainingJetpackTime = Mathf.Clamp(remainingJetpackTime, 0, maxJetpackTime);
            Debug.Log(remainingJetpackTime);
        }

        // Case 2: Player is grounded and not using jetpack
        else if (IsGrounded()) {
            remainingJetpackTime += 2.5f * Time.deltaTime;
            remainingJetpackTime = Mathf.Clamp(remainingJetpackTime, 0, maxJetpackTime);
            Debug.Log(remainingJetpackTime);
        }
    }

    bool IsGrounded() {
        return myfeetcollider.IsTouchingLayers(LayerMask.GetMask("Ground"));
    }
}
