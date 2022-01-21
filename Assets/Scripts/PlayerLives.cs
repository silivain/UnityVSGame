using UnityEngine;
using UnityEngine.UI;
using System.Collections;

// système de nombre de vies des joueurs
// TODO script pas encore utilisé
public class PlayerLives : MonoBehaviour
{
    private Transform playerSpawn;	// spawn du joueur
    //private Animator fadeSystem;	// système de fondu

    public int maxLivesCount;		// nb de vies max du joueur
    public int livesCount;			// nb de vies courant du joueur
    public Text livesCountText;		// texte d'affichage du nb de vies

    public static PlayerLives instance;	// instance de la classe


	/* récupère le spawn du joueur et le système de fondu
	*/
    private void Awake() {
    	playerSpawn = GameObject.FindGameObjectWithTag("PlayerSpawn").transform;
		//fadeSystem = GameObject.FindGameObjectWithTag("FadeSystem").GetComponent<Animator>();
    }

	/* ajoute 'count' vies au joueur et met à jour l'affichage
	*/
    public void AddLives(int count)
    {
      livesCount += count;
      livesCountText.text = livesCount.ToString();
    }


	/* retire 'count' vies au joueur et met à jour l'affichage
	*/
    public void RemoveLives(int count)
    {
      livesCount -= count;
      livesCountText.text = livesCount.ToString();
    }

	/* déclenche le mécanisme de perte de vie à la mort du joueur
	*/
    public void Fall(Transform player)
    {
      StartCoroutine(ReplacePlayer(player));
    }


	/* mort du joueur
	* bloque le mouvement et les interactions du joueur
	* déclenche l'animation de mort
	*/
    public void Die(Transform player)
    {
      PlayerMovement.instance.enabled = false;
      PlayerMovement.instance.rb.bodyType = RigidbodyType2D.Kinematic;
      PlayerMovement.instance.rb.velocity = Vector3.zero;
      PlayerMovement.instance.playerCollider.enabled = false;
      StartCoroutine(DeathAnimation(player));
    }


	/* animation de mort
	* diffère selon game over ou simple respawn
	*/
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
        //fadeSystem.SetTrigger("FadeIn");
        yield return new WaitForSeconds(1f);
        PlayerLives.instance.RemoveLives(1);
        PlayerHealth.instance.Respawn();
        player.position = playerSpawn.position;
      }
    }

	/* animation de mort
	* diffère selon game over ou simple respawn
	* TODO à priori doublon de la méthode 'DeathAnimation'
	* peut être lié à un correctif dans les tutos
	*/
    public IEnumerator ReplacePlayer(Transform player)
    {
      if(livesCount == 1)
      {
        GameOverManager.instance.OnPlayerDeath();
      }
      else
      {
        //fadeSystem.SetTrigger("FadeIn");
        yield return new WaitForSeconds(1f);
        PlayerLives.instance.RemoveLives(1);
        player.position = playerSpawn.position;
      }
    }
}
