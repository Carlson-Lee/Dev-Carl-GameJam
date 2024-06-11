/*
 *  File: EndLevel.cs
 *  Author: Devon
 *  Purpose: Controls triggering of end game panel + raising flag tiles at end of level
 */

using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class EndLevel : MonoBehaviour
{
    [SerializeField] private TilemapRenderer flag;
    [SerializeField] private GameObject endGameCanvas;
    [SerializeField] private PlayerInput player;
    [SerializeField] private EndGameManager endGameManager;
    private bool playerInTrigger = false;
    private bool flagRaised = false;

    /// <summary>
    /// Hide the 'flag' tiles the player needs at the end
    /// </summary>
    void Start()
    {
        flag.enabled = false; //Disable flag from showing on pole
    }

    /// <summary>
    /// If the player is in the correct trigger and pressing F, raise the flag
    /// </summary>
    private void Update()
    {
        if (playerInTrigger && !flagRaised && Input.GetKey(KeyCode.F))
        {
            RaiseFlag();
            flagRaised = true;
        }
    }

    /// <summary>
    /// Set bool for raising flag
    /// </summary>
    /// <param name="collision">Player collision</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInTrigger = true;
        }
    }

    /// <summary>
    /// Set bool so player cannot raise flag outside trigger
    /// </summary>
    /// <param name="other">Player collision leaving trigger</param>
    private void OnTriggerExit(Collider other)
    {
        playerInTrigger = false;
    }

    /// <summary>
    /// Show the tilemap with the flag tiles on it
    /// Start coroutine to countdown the show end summary panel
    /// </summary>
    private void RaiseFlag()
    {
        flag.enabled = true;
        StartCoroutine(EndGame());
    }

    /// <summary>
    /// Starts a countdown the shows an end game panel
    /// Stops any player movement after game end
    /// </summary>
    /// <returns></returns>
    private IEnumerator EndGame()
    {
        endGameManager.CheckItemsAndShowGameOver();
        yield return new WaitForSeconds(3f);
        endGameCanvas.SetActive(true);
        player.enabled = false;
    }
}
