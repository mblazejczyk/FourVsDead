using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTracker : MonoBehaviour
{
    GameObject trackingZombie;
    private void Update()
    {
        if(trackingZombie == null)
        {
            if(GameObject.FindGameObjectsWithTag("Enemy").Length == 0) 
            {
                gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
                return; 
            }
            trackingZombie = GameObject.FindGameObjectsWithTag("Enemy")[Random.Range(0, GameObject.FindGameObjectsWithTag("Enemy").Length - 1)];
        }
        gameObject.transform.LookAt(trackingZombie.transform);
    }
}
