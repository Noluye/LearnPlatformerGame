using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 8;
    public CharacterController character;
    private CameraController cam;
    private Vector3 moveAmount;
    // Start is called before the first frame update
    void Start()
    {
        cam = FindObjectOfType<CameraController>();
    }

    // Update is called once per frame
    void Update()
    {
        moveAmount = cam.transform.forward * Input.GetAxisRaw("Vertical") + (cam.transform.right * Input.GetAxisRaw("Horizontal"));
        moveAmount.y = 0;
        moveAmount = moveAmount.normalized;
        // Input.GetAxisRaw("Horizontal") -> [-1, 1]
        character.Move(moveAmount * moveSpeed * Time.deltaTime);
    }
}
