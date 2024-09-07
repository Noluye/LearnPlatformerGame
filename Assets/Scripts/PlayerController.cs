using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
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
        if (Time.timeScale <= 0.00001f) return;

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
            jumpParticle.SetActive(false);

            if (!lastGrounded)
            {
                landingParticle.SetActive(true);
            }
            
            if (Input.GetButtonDown("Jump"))
            {
                moveAmount.y = jumpForce;
                jumpParticle.SetActive(true);
            }
        }

        lastGrounded = character.isGrounded;  // before move

        character.Move(new Vector3(moveAmount.x * moveSpeed, moveAmount.y, moveAmount.z * moveSpeed) * Time.deltaTime);

        float moveVel = new Vector3(moveAmount.x, 0f, moveAmount.z).magnitude * moveSpeed;
        anim.SetFloat("speed", moveVel);
        anim.SetBool("isGrounded", character.isGrounded);
        anim.SetFloat("yVel", moveAmount.y);
    }

    public void Bounce()
    {
        moveAmount.y = bounceForce;
        character.Move(Vector3.up * bounceForce * Time.deltaTime);
    }
}
