using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// écran de gameover qd le joueur n'a plus de vies
// TODO script pas encore utilisé
/* TODO changer le script pour que le menu se déclenche qd
  un de deux joueurs perd le combat */
public class GameOverManager : MonoBehaviour  //video 16
{
    public GameObject gameOverUI;           // interface de game over
    private Animator gameOverFadeSystem;    // système de fondu

    public static GameOverManager instance; // instance de la classe


    private void Awake()
    {
      if (instance != null)
      {
        Debug.LogWarning("Il y a plus d'une instance de GameOverManager dans la scène.");
        return;
      }
      instance = this;

      // récupère l'animation de game over à partir de son tag
      gameOverFadeSystem = GameObject.FindGameObjectWithTag("GameOverFadeSystem").GetComponent<Animator>();
    }


    /* affiche le menu de game over
    * évite les duplications du gameObject 'player'
    * en vérifiant si celui ci est présent par défaut dans la scène
    */
    public void OnPlayerDeath()
    {
      if (CurrentSceneManager.instance.isPlayerPresentByDefault)
      {
        DontDestroyOnLoadScene.instance.RemoveFromDontDestroyOnLoad();
      }
      StartCoroutine(DisplayMenu());
    }


    /* boutton retry
    * réinitialise le nb de pièces collectés et le nb de vies du joueur
    * respawn le player, désactive le menu de game over et recharge la scène
    */
    public void RetryButton()
    {
      Inventory.instance.RemoveCoins(CurrentSceneManager.instance.coinsPickedUpInThisSceneCount);
      PlayerLives.instance.livesCount = PlayerLives.instance.maxLivesCount;
      PlayerHealth.instance.Respawn();
      gameOverUI.SetActive(false);
      SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


    /* boutton main menu
    * supprime tous les gameobjects de la scène et charge la scène 'MainMenu'
    */
    public void MainMenuButton()
    {
      DontDestroyOnLoadScene.instance.RemoveFromDontDestroyOnLoad();
      SceneManager.LoadScene("MainMenu");
    }

    /* boutton quit
    * ferme l'appli
    */
    public void QuitButton()
    {
      Application.Quit();
    }


    /* affiche le menu game over avec un fondu
    * cache le joueur et empêche la cam de bouger après sa mort
    */
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
