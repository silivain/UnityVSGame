using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour  //video 16
{
    public GameObject gameOverUI;
    private Animator gameOverFadeSystem;

    public static GameOverManager instance;

    private void Awake()
    {
      if (instance != null)
      {
        Debug.LogWarning("Il y a plus d'une instance de GameOverManager dans la scène.");
        return;
      }
      instance = this;
      gameOverFadeSystem = GameObject.FindGameObjectWithTag("GameOverFadeSystem").GetComponent<Animator>();
    }

    public void OnPlayerDeath()
    {
      if (CurrentSceneManager.instance.isPlayerPresentByDefault)
      {
        DontDestroyOnLoadScene.instance.RemoveFromDontDestroyOnLoad();
      }
      StartCoroutine(DisplayMenu());
    }

    public void RetryButton()
    {
      Inventory.instance.RemoveCoins(CurrentSceneManager.instance.coinsPickedUpInThisSceneCount);
      PlayerLives.instance.livesCount = PlayerLives.instance.maxLivesCount;
      PlayerHealth.instance.Respawn();
      gameOverUI.SetActive(false);
      SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MainMenuButton()
    {
      DontDestroyOnLoadScene.instance.RemoveFromDontDestroyOnLoad();
      SceneManager.LoadScene("MainMenu");
    }

    public void QuitButton()
    {
      Application.Quit(); //quitte l'appli
    }

    public IEnumerator DisplayMenu()
    {
      gameOverFadeSystem.SetTrigger("FadeIn");
      yield return new WaitForSeconds(1.5f);
      PlayerMovement.instance.enabled = false;
      PlayerMovement.instance.rb.bodyType = RigidbodyType2D.Kinematic;
      PlayerMovement.instance.rb.velocity = Vector3.zero;
      PlayerMovement.instance.playerCollider.enabled = false;
    }
}
