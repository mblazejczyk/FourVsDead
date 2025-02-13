using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SmoothCamMovement : MonoBehaviour
{
    public Transform target;
    public Vector3 velocity;
    public float dampTime = 0.2f;
    void Update()
    {
        float maxScreenPoint = 0.2f;
        Vector3 mousePos = Input.mousePosition * maxScreenPoint + new Vector3(Screen.width, Screen.height, 0f) * ((1f - maxScreenPoint) * 0.5f);
        //Vector3 position = (target.position + GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition)) / 2f;
        Vector3 position = (target.position + GetComponent<Camera>().ScreenToWorldPoint(mousePos)) / 2f;
        Vector3 destination = new Vector3(position.x, position.y, -10);
        transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
    }
}