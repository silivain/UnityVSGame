using UnityEngine;
using UnityEngine.UI;

public class Ladder : MonoBehaviour //video 12
{
    private bool isInRange;
    private PlayerMovement playerMovement;
    public BoxCollider2D topCollider;
    public Text interactUI;

    void Awake()
    {
      playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
      interactUI = GameObject.FindGameObjectWithTag("InteractUI").GetComponent<Text>();
    }

    void Update()
    {
      if(isInRange && playerMovement.isClimbing && Input.GetKeyDown(KeyCode.E))
      {
        playerMovement.isClimbing = false;
        topCollider.isTrigger = false;
        interactUI.enabled = true;
        return;
      }

      if(isInRange  && !playerMovement.isClimbing && Input.GetKeyDown(KeyCode.E))
      {
        playerMovement.isClimbing = true;
        topCollider.isTrigger = true;
        interactUI.enabled = false;
      }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
      if(collision.CompareTag("Player")) {
        interactUI.enabled = true;
        isInRange = true;
      }
    }

    private void OnTriggerExit2D(Collider2D collision) {
      if(collision.CompareTag("Player")) {
        interactUI.enabled = false;
        isInRange = false;
        playerMovement.isClimbing = false;
        topCollider.isTrigger = false;
      }
    }
}
