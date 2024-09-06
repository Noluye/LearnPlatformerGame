using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public GameObject effect;

    public Transform effectPoint;

    public Animator anim;

    public GameObject[] test;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (LevelManager.instance.respawnPoint != transform.position)  // TODO: maybe trouble
            {
                LevelManager.instance.respawnPoint = transform.position;

                if (effect != null)
                {
                    Instantiate(effect, effectPoint.position, Quaternion.identity);
                }

                Checkpoint[] checkpoints = FindObjectsOfType<Checkpoint>();
                foreach (Checkpoint checkpoint in checkpoints)
                {
                    checkpoint.anim.SetBool("Active", false);
                }
                anim.SetBool("Active", true);
            }
        }
    }
}
