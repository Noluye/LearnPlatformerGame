using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 2;
    public float jumpForce = 4;
    public float gravityScale = 5f;
    public float rotateSpeed = 10f;
    public CharacterController character;
    private float yStore;
    private CameraController cam;
    private Vector3 moveAmount;
    
    
    // Start is called before the first frame update
    void Start()
    {
        cam = FindObjectOfType<CameraController>();
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
        yStore = moveAmount.y;

        moveAmount = cam.transform.forward * Input.GetAxisRaw("Vertical") + (cam.transform.right * Input.GetAxisRaw("Horizontal"));
        moveAmount.y = 0;
        moveAmount = moveAmount.normalized;

        if (moveAmount.magnitude > .1f)
        {
            if (moveAmount != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(moveAmount);
            }
        }

        moveAmount.y = yStore;

        if (character.isGrounded)
        {
            if (Input.GetButtonDown("Jump"))
            {
                moveAmount.y = jumpForce;
            }
        }

        character.Move(new Vector3(moveAmount.x * moveSpeed, moveAmount.y, moveAmount.z * moveSpeed * moveSpeed) * Time.deltaTime);
    }
}
