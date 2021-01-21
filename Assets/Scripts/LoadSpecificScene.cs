using UnityEngine;
﻿using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class LoadSpecificScene : MonoBehaviour
{
    public string sceneName;
    public Animator fadeSystem;
    public Text interactUI;
    private bool isInRange;

    private PlayerMovement playerMovement;

    private void Awake()
    {
      playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
      fadeSystem = GameObject.FindGameObjectWithTag("FadeSystem").GetComponent<Animator>();
    }

    private void Update()
    {
      if(isInRange && Input.GetKeyDown(KeyCode.E))
      {
        interactUI.enabled = false;
        isInRange = false;
        PlayerMovement.instance.enabled = false;                //bloquer les mouvements du perso
        PlayerMovement.instance.rb.velocity = Vector3.zero;
        PlayerMovement.instance.animator.SetTrigger("Respawn");
        StartCoroutine(loadNextScene());
      }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
      if(collision.CompareTag("Player"))
      {
        interactUI.enabled = true;
        isInRange = true;
      }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
      interactUI.enabled = false;
      isInRange = false;
    }

    public IEnumerator loadNextScene()
    {
      fadeSystem.SetTrigger("FadeIn");
      yield return new WaitForSeconds(1f);
      PlayerMovement.instance.enabled = true;
      SceneManager.LoadScene(sceneName);
    }
}
