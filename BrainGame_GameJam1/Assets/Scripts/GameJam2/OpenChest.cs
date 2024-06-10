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

    void Start()
    {
        //Set the displayed chest to start as closed
        closedChest.enabled = true;
        openChest.enabled = false;
    }

    private void Update()
    {
        if (playerInTrigger && Input.GetKey(KeyCode.X))
        {
            PickupItem();
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
        text.gameObject.SetActive(true);
        collectible.enabled = true;

        //Add other behaviour here later for UI collectibles
    }

    private void OnTriggerExit(Collider other)
    {
        playerInTrigger = false;
    }
    private void PickupItem()
    {
        this.collectible.enabled = false;
        this.UI_display.enabled = true;
    }
}
