using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Ground Check")]
    public LayerMask whatIsGround;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public bool isGrounded;

    [Header("On Speed Boost")]
    public LayerMask speedBoostLayer;
    public float boostedSpeed = 15f;
    public  bool onSpeedBoostSurface = false;

    [Header("On Small Speed Boost")]
    public LayerMask smallSpeedBoostLayer;
    public float smallBoostedSpeed = 5f;
    public bool onSmallSpeedBoostSurface = false;

    [Header("On Medium Speed Boost")]
    public LayerMask MediumSpeedBoostLayer;
    public float MediumBoostedSpeed = 10f;
    public bool onMediumSpeedBoostSurface = false;

    [Header("On Big Speed Boost")]
    public LayerMask bigSpeedBoostLayer;
    public float bigBoostedSpeed = 20f;
    public bool onBigSpeedBoostSurface = false;

    [Header("On Jump Pad")]
    public LayerMask jumpPadLayer;
    public float bounceForce = 10f;
    public bool onJumpPadSurface;

    [Header("On Small Jump Pad")]
    public LayerMask smallJumpPadLayer;
    public float smallBounceForce = 5f;
    public bool onSmallJumpPadSurface;

    [Header("On medium Jump Pad")]
    public LayerMask mediumJumpPadLayer;
    public float mediumBounceForce = 10f;
    public bool onMediumJumpPadSurface;

    [Header("On Big Jump Pad")]
    public LayerMask bigJumpPadLayer;
    public float bigBounceForce = 20f;
    public bool onBigJumpPadSurface;



    [Header("Jumping")]
    public bool readyToJump;
    public float jumpForce = 5f;

    public CharacterController controller;

    public Transform cam;

    public float speed = 6f;
    private float defaultSpeed;
    public float airControlFactor = 0.5f; // How much control you have in the air
    public float fallSpeed = 2.5f;
    public float gravityForce = -9.81f;

    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    private Vector3 velocity;
    private Vector3 move;
    private Vector3 airMomentum; // Stores movement direction while in air

    private void Start()
    {
        defaultSpeed = speed;
    }

    void Update()
    {

        // checks the ground
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, whatIsGround);


        // checks the speed pad
        onSpeedBoostSurface = Physics.CheckSphere(groundCheck.position, groundDistance, speedBoostLayer);

        onSmallSpeedBoostSurface = Physics.CheckSphere(groundCheck.position, groundDistance, smallSpeedBoostLayer);

        onMediumSpeedBoostSurface = Physics.CheckSphere(groundCheck.position, groundDistance, MediumSpeedBoostLayer);

        onBigSpeedBoostSurface = Physics.CheckSphere(groundCheck.position, groundDistance, bigSpeedBoostLayer);



        // checks the jump pad
        onJumpPadSurface = Physics.CheckSphere(groundCheck.position, groundDistance, jumpPadLayer);

        onSmallJumpPadSurface = Physics.CheckSphere(groundCheck.position, groundDistance, smallJumpPadLayer);

        onMediumJumpPadSurface = Physics.CheckSphere(groundCheck.position, groundDistance, mediumJumpPadLayer);

        onBigJumpPadSurface = Physics.CheckSphere(groundCheck.position, groundDistance, bigJumpPadLayer);



        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
            airMomentum = Vector3.zero; // Reset air momentum when landing
        }
        // apply speedboost if on a speed surface
        if (onBigSpeedBoostSurface)
        {
            speed = bigBoostedSpeed;
        }
        else if (onMediumSpeedBoostSurface)
        {
            speed = MediumBoostedSpeed;
        }
        else if (onSmallSpeedBoostSurface)
        {
            speed = smallBoostedSpeed;
        }
        else if (onSpeedBoostSurface)
        {
            speed = boostedSpeed;
        }
        else
        {
            speed = defaultSpeed;
        }


        // **Apply bounce if on a jump pad**
        if (onJumpPadSurface)
        {
            velocity.y = Mathf.Sqrt(bounceForce * -2f * gravityForce); // Apply bounce force
        }

        if (onSmallJumpPadSurface)
        {
            velocity.y = Mathf.Sqrt(smallBounceForce * -2f * gravityForce); // Apply bounce force
        }

        if (onMediumJumpPadSurface)
        {
            velocity.y = Mathf.Sqrt(mediumBounceForce * -2f * gravityForce); // Apply bounce force
        }

        if (onBigJumpPadSurface)
        {
            velocity.y = Mathf.Sqrt(bigBounceForce * -2f * gravityForce); // Apply bounce force
        }


        // **Handle movement input**
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(x, 0f, z).normalized;

        //float currentSpeed = onSpeedBoostSurface ? boostedSpeed : speed; // Adjust speed based on layer

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            move = moveDir.normalized * speed;
        }
        else if (isGrounded)
        {
            move = Vector3.zero; // Stop moving if no input on ground
        }

        // **Apply movement based on ground state**
        if (isGrounded)
        {
            controller.Move(move * Time.deltaTime);
        }
        else
        {
            // Apply air momentum if airborne
            airMomentum = Vector3.Lerp(airMomentum, move, airControlFactor * Time.deltaTime);
            controller.Move(airMomentum * Time.deltaTime);
        }

        // **Jump Logic**
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravityForce);
            airMomentum = move; // Preserve current movement direction when jumping
        }

        // **Gravity Application**
        if (velocity.y < 0)
        {
            velocity.y += gravityForce * fallSpeed * Time.deltaTime;
        }
        else
        {
            velocity.y += gravityForce * Time.deltaTime;
        }

        controller.Move(velocity * Time.deltaTime);
    }

    

}
