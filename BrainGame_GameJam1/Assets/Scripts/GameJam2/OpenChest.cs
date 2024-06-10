/*
 *  File: OpenChest.cs
 *  Author: Devon
 *  Purpose: Swaps between 2 chest tilemaps and displays a tutorial message + collectible
 */

using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class OpenChest : MonoBehaviour
{
    [Header("Tilemaps")]
    [SerializeField] private TilemapRenderer closedChest;
    [SerializeField] private TilemapRenderer openChest;

    [Header("Canvas Objects")]
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private SpriteRenderer collectible;
    [SerializeField] private RawImage UI_display;

    [Header("Trigger flags")]
    private bool playerInTrigger = false;
    private bool itemPickedUp = false;

    /// <summary>
    /// Only shows the closed chest at game start
    /// </summary>
    void Start()
    {
        closedChest.enabled = true;
        openChest.enabled = false;
    }

    /// <summary>
    /// Allows the player to pickup the collectible while in trigger area
    /// </summary>
    private void Update()
    {
        if (playerInTrigger && !itemPickedUp && Input.GetKey(KeyCode.X))
        {
            PickupItem();
            itemPickedUp = true;
        }
    }

    /// <summary>
    /// Checks for player collision and swaps chest to open
    /// Shows the collectible and tutorial if it hasnt been collected yet
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Used to only pickup one item at a time
        if (collision.CompareTag("Player"))
        {
            playerInTrigger = true;
        }
        
        closedChest.enabled = false;
        openChest.enabled = true;

        if (!itemPickedUp)
        {
            text.gameObject.SetActive(true); //Show tutorial
            collectible.enabled = true; //Show collectible
        }
    }

    /// <summary>
    /// Resets bool so player cant pickup items outside trigger area
    /// </summary>
    /// <param name="other">Player collider leaving trigger</param>
    private void OnTriggerExit(Collider other)
    {
        playerInTrigger = false;
    }

    /// <summary>
    /// Hide the tutorial/collectible and add to UI
    /// </summary>
    private void PickupItem()
    {
        collectible.enabled = false;
        UI_display.enabled = true;
        text.gameObject.SetActive(false);
    }
}
