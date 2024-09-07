using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelExit : MonoBehaviour
{
    public string levelToLoad;

    private bool existing;

    public Transform camPos;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (!existing)
            {
                existing = true;
                // TODO: anim ...
                LevelManager.instance.EndLevel(levelToLoad);

                FindObjectOfType<CameraController>().endCamPos = camPos;
            }
        }
    }
}
