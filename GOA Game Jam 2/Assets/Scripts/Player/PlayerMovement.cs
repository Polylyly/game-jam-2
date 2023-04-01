using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement instance;
    public SpriteRenderer sprite;
    public float walkSpeed, jumpForce, slideSpeed, runSpeed, currentSpeed;
    public Rigidbody2D rb;
    Animator animator;

    [Header("Inputs")]
    public KeyCode jumpKey;
    public KeyCode dashKey, runKey, crouchKey;

    [Header("Physics")]
    public Transform groundCheck;
    public LayerMask groundLayer;
    float originalGravity;

    bool isRunning = false;

    [Header("Dash")]
    public float longDashTime;
    public float dashForce, dashTime, dashCooldown;
    public bool longDashStarted = false;
    float dashStartTime;
    bool readyToDash, longDashFirstCheck;
    public TrailRenderer dashTrail;
    public Transform dashPoint;
    private Camera mainCam;
    private Vector3 mousePos, mouseRotation;
    float rotZ, originalFixedDeltaTime;

    [Header("Crouch")]
    public float slideForce;
    public bool isCrouching = false, isSliding = false;
    public Collider2D normalCollider, crouchCollider, slideCollider;

    private bool isGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        animator = GetComponentInChildren<Animator>();

        normalCollider.enabled = true;
        crouchCollider.enabled = false;
        slideCollider.enabled = false;

        originalGravity = rb.gravityScale;
        currentSpeed = walkSpeed;
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        readyToDash = true;
        originalFixedDeltaTime = Time.fixedDeltaTime;
    }

    // Update is called once per frame
    void Update()
    {
        MyInput();

        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        mouseRotation = mousePos - transform.position;
        rotZ = Mathf.Atan2(mouseRotation.y, mouseRotation.x) * Mathf.Rad2Deg;
        dashPoint.rotation = Quaternion.Euler(0, 0, rotZ);

        if (isRunning) currentSpeed = runSpeed;
        else if (!isCrouching) currentSpeed = walkSpeed;

        float horizontalMovement = Input.GetAxis("Horizontal");

        if (horizontalMovement < 0) sprite.flipX = true;
        else if (horizontalMovement > 0) sprite.flipX = false;

        rb.velocity = new Vector2(horizontalMovement * currentSpeed, rb.velocity.y);
        if (isGrounded())
        {
            animator.SetBool("Jumping", false);

            if (isRunning && !isCrouching && !isSliding) animator.SetBool("Running", true);
            else if (Mathf.Abs(horizontalMovement) > 0.1f && !isCrouching) animator.SetBool("Walking", true);
            else
            {
                animator.SetBool("Running", false);
                animator.SetBool("Walking", false);
            }
        }
        else if (!isGrounded())
        {
            animator.SetBool("Jumping", true);
        }
    }

    void MyInput()
    {
        if (Input.GetKeyDown(runKey) && !isCrouching)
        {
            isRunning = true;
        }
        if (Input.GetKeyUp(runKey))
        {
            isRunning = false;
        }

        if (Input.GetKeyDown(dashKey))
        {
            dashStartTime = Time.time;
            longDashFirstCheck = true;
        }
        if (longDashFirstCheck && Time.time - dashStartTime >= longDashTime && !longDashStarted && Input.GetKey(dashKey) && readyToDash) LongDashStart();
        else if (Input.GetKeyUp(dashKey) && !longDashStarted) Dash();

        if (Input.GetKeyDown(jumpKey) && isGrounded()) Jump();

        if (longDashStarted)
        {
            if (Input.GetKeyUp(dashKey)) LongDash();
        }

        if (Input.GetKeyDown(crouchKey) && !isSliding) StartCrouch();
        if (Input.GetKeyUp(crouchKey)) EndCrouch();
    }

    void StartCrouch()
    {
        if (!isCrouching)
        {
            isCrouching = true;
            animator.SetBool("Walking", false);
            animator.SetBool("Running", false);
            animator.SetBool("Crouching", true);
            if (!isRunning)
            {
                currentSpeed = 6;
                normalCollider.enabled = false;
                crouchCollider.enabled = true;
            }
            else if (Mathf.Abs(Input.GetAxis("Horizontal")) > 0)
            {
                isSliding = true;
                normalCollider.enabled = false;
                slideCollider.enabled = true;
                rb.AddForce(new Vector2(Input.GetAxis("Horizontal") * slideForce, 0));
            }
        }
    }

    void EndCrouch()
    {
        animator.SetBool("Crouching", false);
        currentSpeed = walkSpeed;
        isCrouching = false;
        isSliding = false;
        crouchCollider.enabled = false;
        slideCollider.enabled = false;
        normalCollider.enabled = true;
    }

    void Dash()
    {
        if (readyToDash)
        {
            longDashFirstCheck = false;
            dashForce = mousePos.x - transform.position.x;
            dashForce = Mathf.Clamp(dashForce, -4, 4);
            rb.MovePosition(transform.position + transform.right * dashForce);
            readyToDash = false;
            StartCoroutine(DashGetReady());
        }
    }

    void LongDashStart()
    {
        animator.SetBool("Dashing", true);
        Time.timeScale = 0.05f;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
        longDashStarted = true;
        longDashFirstCheck = false;
    }

    void LongDash()
    {
        animator.Play("Player-Dash");
        Vector2 dashVector = mousePos - transform.position;
        dashForce = Mathf.Abs(dashVector.magnitude);
        dashForce = Mathf.Clamp(dashForce, 0, 7);
        rb.MovePosition(transform.position + dashPoint.right * dashForce);

        if (dashVector.x < 0) sprite.flipX = true;
        else sprite.flipX = false;

        readyToDash = false;
        longDashStarted = false;
        StartCoroutine(DashNormalTime());
        StartCoroutine(DashGetReady());
    }

    IEnumerator DashNormalTime()
    {
        yield return new WaitForSeconds(0.01f);
        animator.SetBool("Dashing", false);
        Time.timeScale = 1;
        Time.fixedDeltaTime = originalFixedDeltaTime;
    }

    IEnumerator DashGetReady()
    {
        yield return new WaitForSeconds(dashCooldown);
        readyToDash = true;
    }

    void Jump()
    {
        //Jump animation
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }
}
