using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevel : MonoBehaviour
{
    [SerializeField] private TMP_Text instruction;
    private bool playerInTrigger;
    [SerializeField] private ActiveSceneManager activeSceneManager;

    private void Start()
    {
        GameObject sceneManagerObject = GameObject.FindWithTag("SceneManager");
        activeSceneManager = sceneManagerObject.GetComponent<ActiveSceneManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            instruction.gameObject.SetActive(true);
            playerInTrigger = true;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && playerInTrigger)
        {
            activeSceneManager.LoadAndSetActiveScene("SideScrolling_World1");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        instruction.gameObject.SetActive(false);
        playerInTrigger = false;
    }
}
