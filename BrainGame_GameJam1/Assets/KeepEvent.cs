using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepEvent : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
}
