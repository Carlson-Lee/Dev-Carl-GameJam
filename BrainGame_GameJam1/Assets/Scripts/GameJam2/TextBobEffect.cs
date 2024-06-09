using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextBobEffect : MonoBehaviour
{
    [Header("Bobbing Settings")]
    [SerializeField] private float amplitude = 0.02f; // The height of the bobbing
    [SerializeField] private float speed = 1f; // The speed of the bobbing

    [Header("TMP object")]
    private TextMeshProUGUI text;
    private Vector3 originalPosition;

    private void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        originalPosition = text.transform.localPosition;
    }

    private void Update()
    {
        if (text != null)
        {
            float newY = originalPosition.y + Mathf.Sin(Time.time * speed) * amplitude;
            text.transform.localPosition = new Vector3(originalPosition.x, newY, originalPosition.z);
        }
    }
}