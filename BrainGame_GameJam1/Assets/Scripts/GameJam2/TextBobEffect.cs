/*
 *  File: TextBobEffect.cs
 *  Author: Devon
 *  Purpose: Gives specified text objects a 'bobbing' animation up and down
 *  
 *  AI assistance: Some help from ChatGPT in getting the Sin wave for the text bob working correctly
 */

using TMPro;
using UnityEngine;

public class TextBobEffect : MonoBehaviour
{
    [Header("Bobbing Settings")]
    [SerializeField] private float amplitude = 0.02f; // The height of the bobbing
    [SerializeField] private float speed = 1f; // The speed of the bobbing

    [Header("TMP object")]
    private TextMeshProUGUI text; 
    private Vector3 originalPosition; //Centre location of the bobbing movement

    /// <summary>
    /// Set the initial components and values
    /// </summary>
    private void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        originalPosition = text.transform.localPosition;
    }

    /// <summary>
    /// Move the text up and down (Sin) over time
    /// </summary>
    private void Update()
    {
        if (text != null)
        {
            float newY = originalPosition.y + Mathf.Sin(Time.time * speed) * amplitude;
            text.transform.localPosition = new Vector3(originalPosition.x, newY, originalPosition.z);
        }
    }
}
