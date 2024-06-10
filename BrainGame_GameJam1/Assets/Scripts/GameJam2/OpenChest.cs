using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class OpenChest : MonoBehaviour
{
    [SerializeField] private TilemapRenderer closedChest;
    [SerializeField] private TilemapRenderer openChest;

    void Start()
    {
        //Set the displayed chest to start as closed
        closedChest.enabled = true;
        openChest.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        closedChest.enabled = false;
        openChest.enabled = true;

        //Add other behaviour here later for UI collectibles
    }
}
