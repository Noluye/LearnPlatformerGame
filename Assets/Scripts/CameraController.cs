using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform target;
    private Vector3 offset;
    public float moveSpeed = 15f;  // this is for camera following the character.
    // Start is called before the first frame update

    [HideInInspector]
    public Transform endCamPos;

    void Start()
    {
        target = FindObjectOfType<PlayerController>().transform;
        offset = transform.position;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (endCamPos != null)
        {
            // at the end point
            transform.position = Vector3.Lerp(transform.position, endCamPos.position, moveSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Slerp(transform.rotation, endCamPos.rotation, moveSpeed * Time.deltaTime);
            return;
        }

        transform.position = Vector3.Lerp(transform.position, target.position + offset, moveSpeed * Time.deltaTime);
        if (transform.position.y < offset.y)
        {
            transform.position = new Vector3(transform.position.x, offset.y, transform.position.z);
        }
    }

    public void snapToTaget()
    {
        transform.position = target.position + offset;
    }
}
