using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {
    CapsuleCollider2D mybodycollider;
    BoxCollider2D myfeetcollider;
    private Rigidbody2D rb2d;
    Animator myanimator;
    public bool hasKey;
    public int crystalCount;
    public bool minEnergy;
    
    //Exit Trigger
    GameObject exitTrigger;
    BoxCollider2D bc2d;

    //Death Vars
    public bool hitdead = false;
    [SerializeField] Vector2 deathKick = new Vector2(25f, 25f);

    enum UpgradeState { None, Jetpack, GrapplingHook, GravityBoots };
    UpgradeState currentUpgradeState = UpgradeState.GravityBoots;
    [SerializeField] float jumpSpeed = 12f;

    // Energy consomption variables
    public Slider energySlider;

    public float energy;
    private const float maxEnergy = 100;
    private const float movementEnergyFactor = 8;
    private const float jumpEnergyFactor = 10;
    public const float crystalEnergyBoost = 10;


    // Jetpack Variables
    public Slider fuelSlider;

    private const float jetPackVelocity = 2f;
    private const float maxUpwardsVelocity = 17.5f;
    private const float maxJetpackTime = 1.0f;
    private float remainingJetpackTime = maxJetpackTime;

    // Grappling Hook Variables
    private const float grappleDistance = 6f;
    private const float grappleSpeed = 1000f;
    private Vector2 grapplePoint;
    private Vector2 grappleDirection;
    private bool isGrappling;

    public float speed = 7f;

    void Start() {
        rb2d = GetComponent<Rigidbody2D>();
        mybodycollider = GetComponent<CapsuleCollider2D>();
        myfeetcollider = GetComponent<BoxCollider2D>();
        hitdead = false;
        myanimator = GetComponent<Animator>();
        exitTrigger = GameObject.Find("ExitTrigger");
        bc2d = exitTrigger.GetComponent<BoxCollider2D>();

        SetEnergy(0);
    }

    void Update() {

        if (hitdead)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
        
        else {
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
            die();
        }
        nextLevel();

        GatherGems();
    }

    void Run() {

        if (!isGrappling) {
            rb2d.velocity = new Vector2(0, rb2d.velocity.y);
            if (Input.GetKey("a")) {
                rb2d.velocity += new Vector2(-speed, 0);
                transform.localScale = new Vector2(-1, transform.localScale.y);
                //SetEnergy(energy - movementEnergyFactor * Time.deltaTime);
            }
            if (Input.GetKey("d")) {
                rb2d.velocity += new Vector2(speed, 0);
                transform.localScale = new Vector2(1, transform.localScale.y);
                //SetEnergy(energy - movementEnergyFactor * Time.deltaTime);
            }
        }
        
    }

    private void FlipSprite() {

        bool playerHorizonVelocity = Mathf.Abs(rb2d.velocity.x) > Mathf.Epsilon;

        if (playerHorizonVelocity) {
            transform.localScale = new Vector2((float)(Mathf.Sign(rb2d.velocity.x) * 1f), 1f);
        }
    }

    private void Jump() {

        if (Input.GetKeyDown("w")) {
            if (IsGrounded()) {
                //attackflag = 0;
                //myanimator.SetBool("jump", true);
                Vector2 jumpUp = new Vector2(0f, jumpSpeed);
                rb2d.velocity += jumpUp * Mathf.Sign(rb2d.gravityScale);
                //SetEnergy(energy - jumpEnergyFactor);
            }
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
            case UpgradeState.GrapplingHook:
                UseGrapplingHook();
                break;
            case UpgradeState.GravityBoots:
                UseGravityBoots();
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
            fuelSlider.value = remainingJetpackTime;
        }

        // Case 2: Player is grounded and not using jetpack
        else if (IsGrounded()) {
            remainingJetpackTime += 2.5f * Time.deltaTime;
            remainingJetpackTime = Mathf.Clamp(remainingJetpackTime, 0, maxJetpackTime);
            fuelSlider.value = remainingJetpackTime;
        }
    }

    // Allow player to utilize grappling hook
    private void UseGrapplingHook() {

        // Initiate grapple
        if (Input.GetKeyDown(KeyCode.LeftShift)) {
            // Compute direction to send grappling hook
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = Vector3.Normalize(mousePos - (Vector2)rb2d.position);
            int layerMask = 1 << 8; // Bit sequence 1000 0000 -- Only "Ground" layer

            // Shoot out grappling hook
            RaycastHit2D hit = Physics2D.Raycast(rb2d.position, direction, grappleDistance, layerMask);
            if (hit.point != new Vector2(0, 0)) { // Point found!
                grapplePoint = hit.point;
                grappleDirection = direction;

                // When grappling, all gravity removed
                isGrappling = true;
                rb2d.gravityScale = 0;

                rb2d.position += grappleDirection * grappleSpeed / 1000 * Time.deltaTime;
            }
        }

        // Go towards grapple point
        if (Input.GetKey(KeyCode.LeftShift)) {
            if (isGrappling && !isColliding()) {
                rb2d.velocity = grappleDirection * grappleSpeed * Time.deltaTime;
            }

            if (isGrappling && isColliding()) {
                rb2d.velocity = new Vector2(0, 0);
            }
        }

        if (Input.GetKeyUp(KeyCode.LeftShift)) {
            isGrappling = false;
            rb2d.gravityScale = 5;
        }
    }

    private void UseGravityBoots() {
        if (Input.GetKeyDown(KeyCode.LeftShift) && IsGrounded()) {
            rb2d.gravityScale *= -1;
            transform.localScale = new Vector2(transform.localScale.x, transform.localScale.y * -1);
        }
    }

    private void GatherGems() {
        if (myfeetcollider.IsTouchingLayers(LayerMask.GetMask("Crystal")) ||
            mybodycollider.IsTouchingLayers(LayerMask.GetMask("Crystal"))) {
            Debug.Log("Touched gem");
            SetEnergy(energy + crystalEnergyBoost);
        }
    }

    bool IsGrounded() {
        return myfeetcollider.IsTouchingLayers(LayerMask.GetMask("Ground"));
    }

    bool isColliding() {
        return myfeetcollider.IsTouchingLayers(LayerMask.GetMask("Ground")) ||
        mybodycollider.IsTouchingLayers(LayerMask.GetMask("Ground"));
    }

    private void die() {

        if (mybodycollider.IsTouchingLayers(LayerMask.GetMask("Enemy"))) {
            myanimator.SetTrigger("Hurt");
            rb2d.velocity = deathKick;
            hitdead = true;
        }

    }
    
    private void nextLevel() {
        
        
        if (mybodycollider.IsTouchingLayers(LayerMask.GetMask("ExitTrigger"))) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    public void SetEnergy(float e) {
        energy = e;
        energy = Mathf.Clamp(energy, 0, maxEnergy);
        energySlider.value = energy;
        if (energy >= 80) {
            minEnergy = true;
        }
    }
}