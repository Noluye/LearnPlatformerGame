using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeRotator2 : MonoBehaviour
{
    public float rotationSpeed = 50f; // Speed of rotation
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Rotate the cube around the Y-axis
        transform.Rotate(rotationSpeed * Time.deltaTime, 0, 0);
    }
}
