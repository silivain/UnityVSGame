using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    public float invincibilityTimeAfterHit = 3f;
    public float invincibilityFlashDelay = 0.15f;
    public bool isInvincible = false;

    public SpriteRenderer graphics;
    public HealthBar healthBar;
    public Transform player;

    public static PlayerHealth instance;

    private void Awake()
    {                       //mécanisme de singleton : garantit qu'il n'y ait qu'une seule instance de PlayerHealth
                            //permet aussi d'appeler ce script de puis n'importe quel autre script sans utiliser de ref
      if(instance != null)
      {
        Debug.LogWarning("Il y a plus d'une instance de PlayerHealth dans la scène.");
        return;
      }

      instance = this;
    }

    void Start()
    {
      currentHealth = maxHealth;
      healthBar.SetMaxHealth(maxHealth);
    }

    void Update()
    {
      if(Input.GetKeyDown(KeyCode.H))
      {
        TakeDamage(100);
      }
    }

    public void HealPlayer(int amount)
    {
      currentHealth = Mathf.Min(maxHealth, currentHealth + amount);
      healthBar.SetHealth(currentHealth);
    }

    public void TakeDamage(int damage)
    {
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
    }

    public void Respawn() //video16
    {
      PlayerMovement.instance.enabled = true;
      PlayerMovement.instance.animator.SetTrigger("Respawn");
      PlayerMovement.instance.rb.bodyType = RigidbodyType2D.Dynamic;
      PlayerMovement.instance.playerCollider.enabled = true;
      currentHealth = maxHealth;
      healthBar.SetHealth(currentHealth);
    }

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

    public IEnumerator HandleInvincibilityDelay()
    {
      yield return new WaitForSeconds(invincibilityTimeAfterHit);
      isInvincible = false;
    }
}
