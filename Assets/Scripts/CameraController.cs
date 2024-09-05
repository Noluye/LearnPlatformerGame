using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform target;
    private Vector3 offset;
    public float moveSpeed = 15f;  // this is for camera following the character.
    // Start is called before the first frame update
    void Start()
    {
        target = FindObjectOfType<PlayerController>().transform;
        offset = transform.position;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, target.position + offset, moveSpeed * Time.deltaTime);
        if (transform.position.y < offset.y)
        {
            transform.position = new Vector3(transform.position.x, offset.y, transform.position.z);
        }
    }
}
