using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerColider : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            Physics2D.IgnoreCollision(collision.gameObject.GetComponent<CircleCollider2D>(), gameObject.GetComponent<BoxCollider2D>());
        }
    }
}
