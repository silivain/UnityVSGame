using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// système de vie des joueurs
public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;	// vie max du joueur
    public int currentHealth;	// vie courante du joueur

    /* système d'invincibilité après prise de dégats
    public float invincibilityTimeAfterHit = 3f;
    public float invincibilityFlashDelay = 0.15f;
    public bool isInvincible = false;
    */

    //public SpriteRenderer graphics;
    public HealthBar healthBar;	// barre de vie du joueur
    public Transform player;	// transform du joueur

    public static PlayerHealth instance;    	// instance de la classe
	public static string[] heals = {"Bandage"};	// noms des différents heals
	private static int[] healValues = {20};		// puissance des différents heals


    public GameObject shield;                   // shield du joueur
    public KeyCode shieldKey;                   // touche du shield
    private bool shielded;                      // booléen rendant vrai si le shield du joueur est activé
    public GameOver_screen GameOver_Screen;     // Gestion de l'affichage de GameOver

    /* remplit la vie et la barre de vie du joueur au démarrage
	*/
    void Start() {
      currentHealth = maxHealth;
      healthBar.SetMaxHealth(maxHealth);
    }


	/* système de debug : inflige des dégats au joueur lorsque 'H' est utilisée
	* TODO à retirer lorsque implem finie
	*/
    void Update()
    {
      if(Input.GetKeyDown(KeyCode.H))
      {
        TakeDamage(5);
      }

      // shield
      if (Input.GetKeyDown(shieldKey) && !shielded)
      {
          shielded = true;
          shield.SetActive(true);
      }

      // désactivation shield
      if (Input.GetKeyUp(shieldKey) && shielded)
      {
          shielded = false;
          shield.SetActive(false);
      }

    }


    /* heal le joueur de 'amount' pv, et met à jour la barre de vie
	*/
    public void HealPlayer(int amount)
    {
      currentHealth = Mathf.Min(maxHealth, currentHealth + amount);
      healthBar.SetHealth(currentHealth);
    }

	/* heal le joueur en fonction du GO ramassé
	*/
    public void HealPlayerGO(GameObject heal)
    {
	  // on récupère le nom du heal ramassé
  	  string hName = heal.name;
  	  if (hName.Substring(0, Math.Min(4, hName.Length)) == "Heal") {
  		  hName = hName.Substring(4, hName.Length - 4);
  	  }
  	  if (hName.Substring(Math.Max(0, hName.Length - 5), Math.Min(5, hName.Length)) == "(Clone)") {
  		  hName = hName.Substring(0, hName.Length - 5);
  	  }

	  // on récupère la taille du plus petit nom de heal
	  int shortestHealName = heals[0].Length;
	  for(int i = 1; i < heals.Length; ++i) {
		  if (heals[i].Length < shortestHealName) {
			  shortestHealName = heals[i].Length;
		  }
	  }

	  /* on récupère le healID correspondant au nom du heal
	  * on applique le soin correspondant
	  */
	  Predicate<string> checkHeal = arrayEl => arrayEl.Substring(0, shortestHealName) == hName.Substring(0, shortestHealName);
	  int healID = Array.FindIndex(heals, checkHeal);
	  int healingValue = healValues[healID];

      currentHealth = Mathf.Min(maxHealth, currentHealth + healingValue);
      healthBar.SetHealth(currentHealth);
    }

    public void GameOver(){

      GameOver_Screen.Setup(120);
    }


	/* inflige 'damage' dégats au joueur, et met à jour la barre de vie
	*/
    public void TakeDamage(int damage) {

      if (!shielded) {
            currentHealth = Mathf.Max(0, currentHealth - damage);
            healthBar.SetHealth(currentHealth);
        }

      if(currentHealth<=0) {
        GameOver();
      }

      /*
      if(!isInvincible)
      {
        currentHealth = Mathf.Max(0, currentHealth - damage);
        healthBar.SetHealth(currentHealth);

        if(currentHealth <= 0)
        {
          PlayerLives.instance.Die(player);
          return;
        }

        isInvincible = true;
        StartCoroutine(InvincibilityFlash()); //mecanisme pour gerer des durees
        StartCoroutine(HandleInvincibilityDelay());
      }
      */
    }

	/* respawn le joueur au respawn point courant à la mort du joueur
	* reset ses stats
	*/
    public void Respawn() { //video16
      PlayerMovement.instance.enabled = true;
      PlayerMovement.instance.animator.SetTrigger("Respawn");
      PlayerMovement.instance.rb.bodyType = RigidbodyType2D.Dynamic;
      PlayerMovement.instance.playerCollider.enabled = true;
      currentHealth = maxHealth;
      healthBar.SetHealth(currentHealth);
    }

    /*
	* affichage d'un clignotement pdnt la période d'invincibilité
    public IEnumerator InvincibilityFlash()
    {
      while(isInvincible)
      {
        graphics.color = new Color(1f, 1f, 1f, 0f);
        yield return new WaitForSeconds(invincibilityFlashDelay); //mecanisme d'attente
        graphics.color = new Color(1f, 1f, 1f, 1f);
        yield return new WaitForSeconds(invincibilityFlashDelay);
      }
    }
    */

    /*
	* mécanisme de durée de l'invincibilité
    public IEnumerator HandleInvincibilityDelay()
    {
      yield return new WaitForSeconds(invincibilityTimeAfterHit);
      isInvincible = false;
    }
    */


	/* appelle la méthode de heal avec le nom du GO
    * lorsque le joueur entre en contact avec un heal
    * détruit le heal après l'appel
    */
	private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.transform.CompareTag("Heal")) {
			HealPlayerGO(collision.gameObject);
			CurrentSceneManager.instance.CollectedHeal();
    		Destroy(collision.gameObject);
        }
    }

}
