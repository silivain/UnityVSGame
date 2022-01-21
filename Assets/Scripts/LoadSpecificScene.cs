using UnityEngine;
﻿using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

// chargeur de scènes
public class LoadSpecificScene : MonoBehaviour
{
    public string sceneName;	// nom de la scène courante
    public Animator fadeSystem;	// système de fondu
    public Text interactUI;		// texte affichant la touche d'interaction
    private bool isInRange;		// vrai si le joueur est à portée

    private PlayerMovement playerMovement;	// script 'PlayerMovement'


	/*	récupère le script 'PlayerMovement' et le système de fondu
	*/
    private void Awake()
    {
      playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
      fadeSystem = GameObject.FindGameObjectWithTag("FadeSystem").GetComponent<Animator>();
    }


	/* si le jouer est à portée et interagit
	* bloque les mouvements du joueur et appelle le mécanisme de chargement de scène
	*/
    private void Update()
    {
      if(isInRange && Input.GetKeyDown(KeyCode.E))
      {
        interactUI.enabled = false;
        isInRange = false;
        PlayerMovement.instance.enabled = false;
        PlayerMovement.instance.rb.velocity = Vector3.zero;
        PlayerMovement.instance.animator.SetTrigger("Respawn");
        StartCoroutine(loadNextScene());
      }
    }


	/* collider détecte l'arrivée du joueur près de la fin du niveau
	* affiche la touche d'interaction et modif les variables
	*/
    private void OnTriggerEnter2D(Collider2D collision)
    {
      if(collision.CompareTag("Player"))
      {
        interactUI.enabled = true;
        isInRange = true;
      }
    }

	/* collider ne détecte plus le joueur près de la fin du niveau
	* cache la touche d'interaction et modif les variables
	*/
    private void OnTriggerExit2D(Collider2D collision)
    {
      interactUI.enabled = false;
      isInRange = false;
    }


	/* charge la scène suivante avec un fondu
	*/
    public IEnumerator loadNextScene()
    {
      fadeSystem.SetTrigger("FadeIn");
      yield return new WaitForSeconds(1f);
      PlayerMovement.instance.enabled = true;
      SceneManager.LoadScene(sceneName);
    }
}
