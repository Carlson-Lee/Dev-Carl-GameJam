using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class OpenChest : MonoBehaviour
{
    [SerializeField] private TilemapRenderer closedChest;
    [SerializeField] private TilemapRenderer openChest;

    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private SpriteRenderer collectible;
    [SerializeField] private RawImage UI_display;
    private bool playerInTrigger = false;
    private bool itemPickedUp = false;

    void Start()
    {
        //Set the displayed chest to start as closed
        closedChest.enabled = true;
        openChest.enabled = false;
    }

    private void Update()
    {
        if (playerInTrigger && !itemPickedUp && Input.GetKey(KeyCode.X))
        {
            PickupItem();
            itemPickedUp = true;
        }
    }

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
            text.gameObject.SetActive(true);
            collectible.enabled = true;
        }

        //Add other behaviour here later for UI collectibles
    }

    private void OnTriggerExit(Collider other)
    {
        playerInTrigger = false;
    }
    private void PickupItem()
    {
        collectible.enabled = false;
        UI_display.enabled = true;
        text.gameObject.SetActive(false);
    }
}
