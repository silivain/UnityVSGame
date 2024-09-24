using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// Handles players health
public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;	// player max hp
    public int currentHealth;	// player current hp

    /* TODO : invincibility system after hit
    public float invincibilityTimeAfterHit = 3f;
    public float invincibilityFlashDelay = 0.15f;
    public bool isInvincible = false;
    */

    //public SpriteRenderer graphics;
    public HealthBar healthBar;	// player health bar
    public Transform player;	// player transform

    public static PlayerHealth instance;    	// class instance
	public static string[] heals = {"Bandage"};	// names of different heal types
	private static int[] healValues = {5};		// power of different heal types


    public GameObject shield;                   // player shield GO
    public KeyCode shieldKey;                   // shield key code
    private bool shieldReady = true;            // true if shield not on cooldown
    public float shieldCooldown = 5f;           // shield cooldown
    public bool trapResistance = false;         // true if player not vulnerable to traps

    public GameObject GameOver;                 // GameOver window
    private GameOver GameOverScript;            // GameOver.cs

    public AudioClip shieldAudio;               // audio list
    public AudioSource audioSource;             // audio source

    public PlayerControls controls;             // player inputs
    private bool devMode = false;               // can call KillP2 if true


    /* instantiate inputs
    */
    private void Awake() {
        controls = new PlayerControls();    // link with script handling inputs
        if (transform.tag == "Player 1") {  // listen to appropriate InputActionMap
            controls.Player1.Enable();
            controls.Player1.Shield.performed += ctx => Shield();
        }else if (transform.tag == "Player 2"){
            controls.Player2.Enable();
            controls.Player2.Shield.performed += ctx => Shield();
        }
        controls.DevMode.Enable();
        controls.DevMode.KillP2.performed += ctx => KillP2();

        // link proper device to the player (mouse + keyboard/controler)
        controls.devices = InputTools.inputSelect(transform.tag);
    }


    /* set player health and health bar to full capacity
	*/
    void Start() {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

        // link to GameOver.cs
        if (!GameOver.TryGetComponent<GameOver>(out GameOverScript)) {
            Debug.Log("failed to pull GameOver.cs in PlayerHealth.cs");
        }
    }


    /* active shield on button pressed, check if cooldown finished
    * initiate shield cooldown
    * check if game is playing (and not paused)
    */
    private void Shield()
    {   
        if (!shield.activeSelf && shieldReady && this.enabled && Time.timeScale != 0f) {
            shield.SetActive(true);
            shieldReady = false;
            StartCoroutine(cooldownShield());
        }
    }


    /* heal player 'amount' hp, update health bar
    */
    public void HealPlayer(int amount) {
        currentHealth = Mathf.Min(maxHealth, currentHealth + amount);
        healthBar.SetHealth(currentHealth);
    }


    /* heal player depending on collectable
    */
    public void HealPlayerGO(GameObject heal) {

        // collected heal name
        string hName = heal.name;
        if (hName.Substring(0, Math.Min(4, hName.Length)) == "Heal") {
            hName = hName.Substring(4, hName.Length - 4);
        }

        if (hName.Substring(Math.Max(0, hName.Length - 5), Math.Min(5, hName.Length)) == "(Clone)") {
            hName = hName.Substring(0, hName.Length - 5);
        }

        // shortest heal name
        int shortestHealName = heals[0].Length;
        for(int i = 1; i < heals.Length; ++i) {
            if (heals[i].Length < shortestHealName) {
                shortestHealName = heals[i].Length;
            }
        }

        /* find healID corresponding to collected heal
        * heal player, update health bar
        */
        Predicate<string> checkHeal = arrayEl => arrayEl.Substring(0, shortestHealName) == hName.Substring(0, shortestHealName);
        int healID = Array.FindIndex(heals, checkHeal);
        int healingValue = healValues[healID];

        currentHealth = Mathf.Min(maxHealth, currentHealth + healingValue);
        healthBar.SetHealth(currentHealth);
    }


    /* Call GameOver screen
    */
    public void CallGameOver(string _tag) {
        GameOverScript.Setup(_tag);
    }


    /* deals 'damage' dmgs to the player, update health bar
    */
    public void TakeDamage(int damage) {

        if (!shield.activeSelf) {
            currentHealth = Mathf.Max(0, currentHealth - damage);
            healthBar.SetHealth(currentHealth);
        }

        if (shield.activeSelf) {
            AudioManager.instance.PlayClipAt(shieldAudio, transform.position);
        }

        if(currentHealth<=0) {
            CallGameOver(transform.tag);
        }

        /*
        if(!isInvincible) {
            currentHealth = Mathf.Max(0, currentHealth - damage);
            healthBar.SetHealth(currentHealth);

            if(currentHealth <= 0) {
                PlayerLives.instance.Die(player);
                return;
            }

            isInvincible = true;
            StartCoroutine(InvincibilityFlash()); //mecanisme pour gerer des durees
            StartCoroutine(HandleInvincibilityDelay());
        }
        */
    }


	/* respawn player at respawn location
	* reset his stats
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
	* flashy display during invincibility timelapse
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
	* handle invincibility duration
    public IEnumerator HandleInvincibilityDelay()
    {
      yield return new WaitForSeconds(invincibilityTimeAfterHit);
      isInvincible = false;
    }
    */


    /* handle shield cooldown
    */
    IEnumerator cooldownShield() {
        if (!shieldReady) {
            yield return new WaitForSeconds(1f);
            shield.SetActive(false);
            yield return new WaitForSeconds(shieldCooldown - 1f);
            shieldReady = true;
        }

    }


	/* when collecting a heal
    * call proper heal function with GO name
    * destroy collectable
    */
	private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.transform.CompareTag("Heal")) {
			HealPlayerGO(collision.gameObject);
			CurrentSceneManager.instance.CollectedHeal(collision.transform.position);
    		Destroy(collision.gameObject);
        }
    }


    /* call "CallGameOver" with tag "Player2"
    * direct ingame access to GameOver screen
    */
    private void KillP2() {
        if (devMode) {
            CallGameOver("Player 2");
        }
    }
}
