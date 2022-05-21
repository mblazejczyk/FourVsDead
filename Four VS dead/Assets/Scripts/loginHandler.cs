using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class loginHandler : MonoBehaviour
{
    public string loginId = "";
    public string login = "";
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
