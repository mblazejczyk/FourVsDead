using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardSaver : MonoBehaviour
{
    public int xpGranted;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
