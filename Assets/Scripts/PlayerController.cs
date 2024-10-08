using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float mediationSpeed = 7.0f;
    public float shieldSanityConsumed = 8.0f;
    public float jumpSanityConsumed = 2.0f;

    public float moveSpeed = 8;
    public float jumpForce = 17;
    public float gravityScale = 5f;
    public float rotateSpeed = 10f;
    public CharacterController character;
    public Animator anim;
    private float yStore;
    private CameraController cam;
    private Vector3 moveAmount;
    public GameObject jumpParticle, landingParticle;
    private bool lastGrounded;

    public float bounceForce;

    // attack
    public GameObject shieldPrefab; // Assign the transparent sphere prefab in the inspector
    private GameObject activeShield;
    public float shieldDuration = 2f; // 2 minutes

    // Multiple jump variables
    public int maxJumps = 2; // Maximum number of jumps allowed
    private int currentJumps = 0; // Track how many jumps the player has performed

    // Start is called before the first frame update
    void Start()
    {
        cam = FindObjectOfType<CameraController>();
        lastGrounded = true;
        character.Move(new Vector3(0f, Physics.gravity.y * gravityScale * Time.deltaTime, 0f));
    }

    private void FixedUpdate()
    {
        if (!character.isGrounded)
        {
            moveAmount.y += Physics.gravity.y * gravityScale * Time.fixedDeltaTime;
        } else
        {
            moveAmount.y = Physics.gravity.y * gravityScale * Time.fixedDeltaTime;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale <= 0.00001f || LevelManager.instance.levelComplete) return;
        
        EatLogic();

        yStore = moveAmount.y;

        moveAmount = cam.transform.forward * Input.GetAxisRaw("Vertical") + (cam.transform.right * Input.GetAxisRaw("Horizontal"));
        moveAmount.y = 0;
        moveAmount = moveAmount.normalized;

        if (moveAmount.magnitude > .1f)
        {
            if (moveAmount != Vector3.zero)
            {
                 Quaternion newRot = Quaternion.LookRotation(moveAmount);
				 transform.rotation = Quaternion.Slerp(transform.rotation, newRot, rotateSpeed * Time.deltaTime);
            }
        }

        moveAmount.y = yStore;

        if (character.isGrounded)
        {
            // Reset jump count when grounded
            currentJumps = 0;

            if (jumpParticle) jumpParticle.SetActive(false);

            if (!lastGrounded)
            {
                if (landingParticle) landingParticle.SetActive(true);
            }
            
            if (Input.GetButtonDown("Jump"))
            {
                PerformJump();
            }
        } else
        {
            // Allow additional jumps while not grounded
            if (Input.GetButtonDown("Jump") && currentJumps < maxJumps && (PlayerHealthController.instance.TryToUseSanity(1)))
            {
                PerformJump(2);
            }
        }

        lastGrounded = character.isGrounded;  // before move

        character.Move(new Vector3(moveAmount.x * moveSpeed, moveAmount.y, moveAmount.z * moveSpeed) * Time.deltaTime);

        float moveVel = new Vector3(moveAmount.x, 0f, moveAmount.z).magnitude * moveSpeed;
        if (anim == null) return;
        anim.SetFloat("speed", moveVel);
        anim.SetBool("isGrounded", character.isGrounded);
        anim.SetFloat("yVel", moveAmount.y);
    }

    private void PerformJump(float additional = 0)
    {
        moveAmount.y = jumpForce;
        currentJumps++;

        if (jumpParticle) jumpParticle.SetActive(true);
    }

    public void Bounce()
    {
        moveAmount.y = bounceForce;
        character.Move(Vector3.up * bounceForce * Time.deltaTime);
    }

    private void EatLogic()
    {
        if (Input.GetKeyUp(KeyCode.R))  // eat a coin
        {
            PlayerHealthController.instance.TryToEatCoin();
        }
        if (Input.GetKeyUp(KeyCode.T))  // eat a crystal
        {
            PlayerHealthController.instance.TryToEatCrystal();
        }
        if (Input.GetKey(KeyCode.G))  // meditation
        {
            PlayerHealthController.instance.AddSanity(mediationSpeed * Time.deltaTime);
        }
        if (Input.GetKeyUp(KeyCode.F) && activeShield == null)  // rotate fight
        {
            if (PlayerHealthController.instance.TryToUseSanity(shieldSanityConsumed))  // TODO
            {
                SpawnShield();
            }
        }
        
    }

    private void SpawnShield()
    {
        // Spawn the transparent sphere around the character
        activeShield = Instantiate(shieldPrefab, transform.position, Quaternion.identity);

        // Make the sphere follow the character
        activeShield.transform.SetParent(transform);

        // Start coroutine to destroy the shield after 2 minutes
        StartCoroutine(DestroyShieldAfterDuration(shieldDuration));
    }

    private IEnumerator DestroyShieldAfterDuration(float duration)
    {
        yield return new WaitForSeconds(duration);

        DestroyShield();
    }

    public void DestroyShield()
    {
        if (activeShield != null)
        {
            Destroy(activeShield); // Destroy the shield after the duration
        }
    }

}
