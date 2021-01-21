using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerLives : MonoBehaviour
{
    private Transform playerSpawn;
    private Animator fadeSystem;

    public int maxLivesCount;
    public int livesCount;
    public Text livesCountText;

    public static PlayerLives instance;

    private void Awake() {  //mécanisme de singleton : garantit qu'il n'y ait qu'une seule instance de PlayerLives
                            //permet aussi d'appeler ce script de puis n'importe quel autre script sans utiliser de ref
      if(instance != null) {
        Debug.LogWarning("Il y a plus d'une instance de PlayerLives dans la scène.");
        return;
      }
      instance = this;

      playerSpawn = GameObject.FindGameObjectWithTag("PlayerSpawn").transform;
      fadeSystem = GameObject.FindGameObjectWithTag("FadeSystem").GetComponent<Animator>();
    }

    public void AddLives(int count)
    {
      livesCount += count;
      livesCountText.text = livesCount.ToString();
    }

    public void RemoveLives(int count)
    {
      livesCount -= count;
      livesCountText.text = livesCount.ToString();
    }

    public void Fall(Transform player)
    {
      StartCoroutine(ReplacePlayer(player));
    }

    public void Die(Transform player)
    {
      PlayerMovement.instance.enabled = false;                //bloquer les mouvements du perso
      PlayerMovement.instance.rb.bodyType = RigidbodyType2D.Kinematic;  //desactiver la physique du perso pour ne plus interagir
      PlayerMovement.instance.rb.velocity = Vector3.zero;
      PlayerMovement.instance.playerCollider.enabled = false; //desactiver le collider du perso pour ne plus tuer les ennemis si ceux ci nous traversent
      StartCoroutine(DeathAnimation(player));
    }

    public IEnumerator DeathAnimation(Transform player)
    {
      PlayerMovement.instance.animator.SetTrigger("Death");
      yield return new WaitForSeconds(0.5f);

      if(livesCount == 1)
      {
        GameOverManager.instance.OnPlayerDeath();
      }
      else
      {
        fadeSystem.SetTrigger("FadeIn");
        yield return new WaitForSeconds(1f);
        PlayerLives.instance.RemoveLives(1);
        PlayerHealth.instance.Respawn();
        player.position = playerSpawn.position;
      }
    }

    public IEnumerator ReplacePlayer(Transform player)
    {
      if(livesCount == 1)
      {
        GameOverManager.instance.OnPlayerDeath();
      }
      else
      {
        fadeSystem.SetTrigger("FadeIn");
        yield return new WaitForSeconds(1f);
        PlayerLives.instance.RemoveLives(1);
        player.position = playerSpawn.position;
      }
    }
}
