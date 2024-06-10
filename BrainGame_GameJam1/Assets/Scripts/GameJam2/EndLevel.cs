using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class EndLevel : MonoBehaviour
{
    [SerializeField] private TilemapRenderer flag;
    [SerializeField] private GameObject endGameCanvas;
    [SerializeField] private PlayerInput player;
    [SerializeField] private EndGameManager endGameManager;
    private bool playerInTrigger = false;
    private bool flagRaised = false;

    void Start()
    {
        flag.enabled = false; //Disable flag from showing on pole
    }

    private void Update()
    {
        if (playerInTrigger && !flagRaised && Input.GetKey(KeyCode.F))
        {
            RaiseFlag();
            flagRaised = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Used to only pickup one item at a time
        if (collision.CompareTag("Player"))
        {
            playerInTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        playerInTrigger = false;
    }

    private void RaiseFlag()
    {
        flag.enabled = true;
        StartCoroutine(EndGame());
    }

    /// <summary>
    /// Starts a countdown the shows an end game panel
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
