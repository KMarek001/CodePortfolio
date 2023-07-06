using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;

    public float groundDrag;

    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    public LayerMask whatIsTeleport;
    bool grounded;
    
    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    public Animator playerAnimator;

    private bool isRunning = false;

    [SerializeField]
    private AudioClip walkSound;
    [SerializeField]
    private AudioClip teleportSound;

    private AudioSource audioSource;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        readyToJump = true;
        audioSource = GetComponent<AudioSource>();
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f, whatIsGround);
        
        MyInput();
        SpeedControl();

        if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;

        if(Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f, whatIsTeleport))
        {
            if(!audioSource.isPlaying)
                audioSource.PlayOneShot(teleportSound, 0.25f);
            ChangeScene.instance.SwapScene();
        }
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if(Input.GetKeyDown(sprintKey) && grounded)
            isRunning = true;
        else if(Input.GetKeyUp(sprintKey) && grounded)
            isRunning = false;

        if (Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            isRunning = false;
            readyToJump = false;
 
            Jump();
            
            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if(grounded)
        {
            if (moveDirection != Vector3.zero)
            {
                playerAnimator.SetBool("isWalking", true);
                playerAnimator.SetBool("wasWalking", true);
                if (!audioSource.isPlaying)
                {
                    audioSource.PlayOneShot(walkSound, 0.1f);
                }
            }
            else
            {
                playerAnimator.SetBool("isWalking", false);
                playerAnimator.SetBool("wasWalking", false);
            }

            if(isRunning)
            {
                playerAnimator.SetBool("isRunning", true);
                rb.AddForce(moveDirection.normalized * moveSpeed * 20f, ForceMode.Force);
            }
            else
            {
                playerAnimator.SetBool("isRunning", false);
                rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
            }
        }
        else if(!grounded)
        {
            playerAnimator.SetBool("isWalking", false);
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
        }
           
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if(flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVal = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVal.x, rb.velocity.y, limitedVal.z);
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        playerAnimator.SetTrigger("Jump");
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;
    }
}
