using UnityEngine;
using UnityEngine.UI;

// échelles
// TODO script pas encore utilisé
public class Ladder : MonoBehaviour //video 12
{
	private bool isInRange;					// vrai si joueur suffisamment proche pour emprunter l'échelle
	private PlayerMovement playerMovement;	// script playerMovement
	public BoxCollider2D topCollider;		// collider du haut de l'échelle, évite au joueur de retomber
	public Text interactUI;					// texte s'affichant pour indiquer avec quelle touche interagir

	/*	récupère le script 'PlayerMovement' et le texte d'interaction
	*/
	void Awake() {
		playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
		interactUI = GameObject.FindGameObjectWithTag("InteractUI").GetComponent<Text>();
	}


	/* débute ou met fin à l'interaction lors de l'utilisation de la touche dédiée
	*/
	void Update() {
		if(isInRange && playerMovement.isClimbing && Input.GetKeyDown(KeyCode.E)) {
			playerMovement.isClimbing = false;
			topCollider.isTrigger = false;
			interactUI.enabled = true;
			return;
		}

		if(isInRange  && !playerMovement.isClimbing && Input.GetKeyDown(KeyCode.E)) {
			playerMovement.isClimbing = true;
			topCollider.isTrigger = true;
			interactUI.enabled = false;
		}
	}


	/* collider détecte l'arrivée du joueur près de l'échelle
	* affiche la touche d'interaction et modif les variables
	*/
	private void OnTriggerEnter2D(Collider2D collision) {
		if(collision.CompareTag("Player")) {
			interactUI.enabled = true;
			isInRange = true;
		}
	}


	/* collider détecte l'éloignement du joueur de l'échelle
	* cache la touche d'interaction et modif les variables
	*/
	private void OnTriggerExit2D(Collider2D collision) {
		if(collision.CompareTag("Player")) {
			interactUI.enabled = false;
			isInRange = false;
			playerMovement.isClimbing = false;
			topCollider.isTrigger = false;
		}
	}
}
