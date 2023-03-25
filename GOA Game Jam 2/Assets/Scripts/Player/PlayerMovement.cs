using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float walkSpeed, jumpForce, slideSpeed, runSpeed, currentSpeed;
    public Rigidbody2D rb;

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
    bool readyToDash, isDashing, longDashFirstCheck;
    public TrailRenderer dashTrail;
    public Transform dashPoint;
    private Camera mainCam;
    private Vector3 mousePos, mouseRotation;
    float rotZ, originalFixedDeltaTime;

    [Header("Crouch")]
    public float slideForce;
    bool isCrouching = false, isSliding = false;
    public Collider2D normalCollider, crouchCollider, slideCollider;

    private bool isGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    // Start is called before the first frame update
    void Start()
    {
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
        rb.velocity = new Vector2(horizontalMovement * currentSpeed, rb.velocity.y);
        if (isGrounded())
        {
            //if (isRunning) //Running animation
            //else //Walking animation
        }
        else
        {
            //Jumping moving animation
        }
    }

    void MyInput()
    {
        if (Input.GetKeyDown(runKey) && !isCrouching)
        {
            //Run animation
            isRunning = true;
        }
        if (Input.GetKeyUp(runKey))
        {
            //Walk animation
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
            if (!isRunning)
            {
                //Crouch animation
                currentSpeed = 6;
                normalCollider.enabled = false;
                crouchCollider.enabled = true;
            }
            else if (Mathf.Abs(Input.GetAxis("Horizontal")) > 0)
            {
                //Slide animation
                isSliding = true;
                normalCollider.enabled = false;
                slideCollider.enabled = true;
                rb.AddForce(new Vector2(Input.GetAxis("Horizontal") * slideForce, 0));
            }
        }
    }

    void EndCrouch()
    {
        currentSpeed = walkSpeed;
        isCrouching = false;
        isSliding = false;
        //Standing animation
        crouchCollider.enabled = false;
        slideCollider.enabled = false;
        normalCollider.enabled = true;
    }

    void Dash()
    {
        if (readyToDash)
        {
            //Dash animation
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
        //Long dash animation
        Time.timeScale = 0.05f;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
        longDashStarted = true;
        longDashFirstCheck = false;
    }

    void LongDash()
    {
        Vector2 dashVector = mousePos - transform.position;
        dashForce = Mathf.Abs(dashVector.magnitude);
        dashForce = Mathf.Clamp(dashForce, 0, 7);
        rb.MovePosition(transform.position + dashPoint.right * dashForce);

        readyToDash = false;
        longDashStarted = false;
        StartCoroutine(DashNormalTime());
        StartCoroutine(DashGetReady());
    }

    IEnumerator DashNormalTime()
    {
        yield return new WaitForSeconds(0.01f);
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
