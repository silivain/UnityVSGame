using System;
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerWeapon : MonoBehaviour
{
    public KeyCode fire1;						    // touche de tir

  public GameObject weapon;						// arme utilisée par le player lorsque 'fire1' pressed
  public GameObject[] weaponsGO;				// armes du jeu
  public Sprite[] weaponsItems;					// images des items des armes (les collectables)
  public int weaponID;							// ID de l'arme du player
  public Transform throwPoint;				    // point depuis lequel les projectiles sont instanciés
  public Transform highThrowPoint;				// throwPoint haut (souba etc)
  public KeyCode changeWeapon; 					// changement d'arme (debug)
  public GameObject grenadeGO;					// TODO
  public static PlayerWeapon instance;     		// instance de la classe
  public float force;                           // TODO
  public Transform tromboneRangePoint;
  public LayerMask playerLayers;
  public int tromboneDamage = 10;
  public SpriteRenderer tromboneSprite;

  private Boolean isWeaponReady = true;
  public int ammunition = 1000;
  public Image ammunitionBar;			// barre d'affichage des munitions
  public Text ammunitionCountText;		// texte d'affichage du nb de mun

  private Transform playerShield;   // shield du joueur

  public int splashDamage = 10;
  public float splashRange = 3f;
  public GameObject explosionVisual;

  public  static string[] weapons         = {"Bullet", "Clarinet", "Grenade", "tromboneRangePoint", "Sousa", "Tuba"};	// armes du jeu, l'ordre des armes doit match leur weaponID
  private static int[]    maxAmmunition   = {1000, 13, 7, 25, 21, 15};                                                  // nombre de munitions max/de départ pour chaque arme
  public         int[]    bonusAmmunition = {1000, 6, 3, 12, 10, 7};                                                    // nombre du munitions apportés par l'item 'Ammunition'
  private static float[]  cooldownTime    = {.5f, 1f, 2f, .5f, 1f, 1f};                                                 // Cooldown de chaque arme

  public float currentDamageBonus = 0;        // bonus de dégats actuel

  private float startTime = 0f;
  //private float endTime=0f; TODO


  private void Awake() {
    instance = this;
    playerShield = transform.Find("Shield");
  }

    // Update is called once per frame
    void Update()
    {
      // Debug Key pour le changement d'arme
      if (Input.GetKeyDown(changeWeapon)){
            weaponID = 3;
			setWeapon(weaponsGO[3]);
      }



        if (Input.GetKeyDown(fire1) && !playerShield.gameObject.activeSelf && isWeaponReady) {
            //cooldown du tir
            isWeaponReady = false;
            StartCoroutine(cooldownWeapon());
            //tir en fonction de l'arme équipée
            switch (weaponID) {
          case 0:
            bullet();
            break;
          case 1:
            clarinet();
            break;
          case 2:
            startTime = Time.time;
            grenadeLaunch();
            break;
          case 3:
            trombone();
            break;
		  case 4:
		  	StartCoroutine(sousa());
			break;
		  case 5:
  		  	StartCoroutine(tuba());
  			break;
          default:
            bullet();
            break;
            }
        }
    }


    /* TODO
    */
    void bullet() {
      GameObject bulletClone = (GameObject) Instantiate(weapon, throwPoint.position, throwPoint.rotation);
	  bulletClone.tag = "Proj" + transform.tag;

	  UseAmmo();

      // TODO indiqué au projectile son parent pour pas se le manger lors d'un dash par ex
      //Debug.Log("player pos : " + transform.position + "\ndebug depuis PlayerMovement, l84"); debug
      //anim.SetTrigger("fire anim"); animation
    }


    /* Tire 3 projs de clarinette
    * les projs suivent une traj courbée
    * ils sont instantiés les uns sous les autres ingame
    */
    void clarinet() {
      Vector3 vectorClarinet = throwPoint.position;
      GameObject clarinetClone1 = (GameObject)Instantiate(weapon, vectorClarinet, throwPoint.rotation);
	  clarinetClone1.tag = "Proj" + transform.tag;
      vectorClarinet.y += 0.5f;
      GameObject clarinetClone2 = (GameObject)Instantiate(weapon, vectorClarinet, throwPoint.rotation);
	  clarinetClone2.tag = "Proj" + transform.tag;
      vectorClarinet.y += 0.5f;
      GameObject clarinetClone3 = (GameObject)Instantiate(weapon, vectorClarinet, throwPoint.rotation);
	  clarinetClone3.tag = "Proj" + transform.tag;

	  UseAmmo();

      //// TODO indiqué au projectile son parent pour pas se le manger lors d'un dash par ex
      //Debug.Log("player pos : " + transform.position + "\ndebug depuis PlayerMovement, l84"); debug
      //anim.SetTrigger("fire anim"); animation
    }


    /* Tire une grenade
    * TODO : commenter pls
    */
    void grenadeLaunch(){
      //lengthTime = endTime-startTime;
      float lengthTime = 3;
      Vector3 vectorClarinet = throwPoint.position;
      GameObject grenade = Instantiate(grenadeGO, vectorClarinet, throwPoint.rotation);
	  grenade.tag = "Proj" + transform.tag;
      Rigidbody2D projRb = grenade.GetComponent<Rigidbody2D>();
      projRb.AddForce(new Vector2(1*force*lengthTime,2*force*lengthTime));
      projRb.angularVelocity = -180;

	  UseAmmo();

      //Destroy(grenade,Random.Range(1,10));
    }


    /* Coup de trombone au cac
    * on récupère tous les collider entre le joueur et 'tromboneRangePoint'
    * on applique les dégats du trombone à tous les joueurs récupérés
    */
    void trombone() {
        StartCoroutine(tromboneAppear());
        Collider2D[] tromboneHitbox = Physics2D.OverlapAreaAll(throwPoint.position, tromboneRangePoint.position, playerLayers);
        foreach(Collider2D enemy in tromboneHitbox)
        {
            // CapsuleCollider2D pour éviter d'appliquer les dégats à plusieurs colliders du même perso
            if (enemy is CapsuleCollider2D) {
                //Debug.Log("we hit " + enemy.name);
                PlayerHealth playerHealth = enemy.transform.GetComponent<PlayerHealth>();
                playerHealth.TakeDamage(tromboneDamage + (int) currentDamageBonus);
                // TODO appel à la fonction de recul en passant les arguments nécessaires
                // le collider 'other', le rigidbody du go bullet (pour pouvoir recup sa velocity)
                PlayerMovement.instance.RecoilCac(enemy, transform);
            }
        }
	}


    /* Animation du coup de trombone
    */
    IEnumerator tromboneAppear()
    {
        tromboneSprite.enabled = true;
        yield return new WaitForSeconds(0.2f);
        tromboneSprite.enabled = false;

		UseAmmo();
    }


    /* Tire 3 projs de sousa
    * les projs disparaissent au bout de 1 sec
    * TODO : faire une var pour déterminer la range = durée avant destroy
    */
	IEnumerator sousa() {
		Vector3 vectorSousa = highThrowPoint.position;
		GameObject sousa1 = (GameObject) Instantiate(weapon, vectorSousa, highThrowPoint.rotation);
  	  	sousa1.tag = "Proj" + transform.tag;
		vectorSousa.y -= 0.3f;
		GameObject sousa2 = (GameObject) Instantiate(weapon, vectorSousa, highThrowPoint.rotation);
  	  	sousa2.tag = "Proj" + transform.tag;
		Rigidbody2D rbSousa2 = sousa2.GetComponent<Rigidbody2D>();
		rbSousa2.SetRotation(transform.rotation.y >= 0 ? (rbSousa2.rotation - 3f) : (rbSousa2.rotation + 3f));
		vectorSousa.y -= 0.3f;
		GameObject sousa3 = (GameObject) Instantiate(weapon, vectorSousa, highThrowPoint.rotation);
  	  	sousa3.tag = "Proj" + transform.tag;
		Rigidbody2D rbSousa3 = sousa3.GetComponent<Rigidbody2D>();
		rbSousa3.SetRotation(transform.rotation.y >= 0 ? (rbSousa3.rotation - 6f) : (rbSousa3.rotation + 6f));

		UseAmmo();

		yield return new WaitForSeconds(1f);
		Destroy(sousa1);
		Destroy(sousa2);
		Destroy(sousa3);
	}


    /* Gère le cooldown de l'arme actuelle en fonction des valeurs de 'cooldownTime'
    */
    IEnumerator cooldownWeapon() {
        if (!isWeaponReady) {
            yield return new WaitForSeconds(cooldownTime[weaponID]);
            isWeaponReady = true;
        }
    }


    /* Crée un projectile 'tuba'
    * le projectile explose :
    * - au contact d'un collider
    * - au bout de 0.75 sec sinon
    * TODO : faire une var pour la durée avant explo
    */
	IEnumerator tuba() {
		GameObject tuba = (GameObject) Instantiate(weapon, throwPoint.position, throwPoint.rotation);
  	    tuba.tag = "Proj" + transform.tag;
	    UseAmmo();

		yield return new WaitForSeconds(0.75f);
        Explosion(tuba);
    }


	/* Utilise une munition de l'arme équipée
	* Si le joueur n'a plus de munitions, rééquipe l'arme de base
	*/
	void UseAmmo() {
		if (ammunition-- == 1) {
	      weaponID = 0;
	      weapon = weaponsGO[weaponID];
	      ammunition = maxAmmunition[0];
	      ammunitionCountText.text = "∞";
		  ammunitionBar.sprite = weaponsItems[weaponID];
	    }
	    if (weaponID != 0) {
	      ammunitionCountText.text = ammunition.ToString();
	    }
	}


    /* Équipe l'arme sur le player
    * si le player était déjà équipé d'une arme, la remplace
    */
    public void setWeapon(GameObject w) {

	  // on récupère le nom de l'arme ramassée
	  string wName = w.name;
	  if (wName.Substring(0, Math.Min(6, wName.Length)) == "weapon") {
		  wName = wName.Substring(6, wName.Length - 6);
	  }
	  if (wName.Substring(Math.Max(0, wName.Length - 7), Math.Min(7, wName.Length)) == "(Clone)") {
		  wName = wName.Substring(0, wName.Length - 7);
	  }

	  /* on récupère le weaponID correspondant au nom de l'arme
	  * on équipe la bonne arme
	  * on met à jour les munitions et l'affichage
	  */
	  Predicate<string> checkWeapon = arrayEl => arrayEl.Substring(0, 3) == wName.Substring(0, 3) ;
	  weaponID = Array.FindIndex(weapons, checkWeapon);
	  weapon = weaponsGO[weaponID];
      ammunition = maxAmmunition[weaponID];
      AmmoDisplay();


      // TODO (lancer une anim) + changer l'apparence du player en fonction de l'item
    }


    // met à jour l'affichage des munitions
    public void AmmoDisplay() {
        ammunitionBar.sprite = weaponsItems[weaponID];
        if (weaponID == 0) {
            ammunitionCountText.text = "∞";
        }else{
            ammunitionCountText.text = ammunition.ToString();
        }
    }


    /* Explosion du tuba
    * TODO : commenter pls
    */
    public void Explosion(GameObject tuba)
    {
        Instantiate(explosionVisual, tuba.transform.position, tuba.transform.rotation = Quaternion.identity);
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(tuba.transform.position, splashRange);
        foreach (Collider2D hitCollider in hitColliders)
        {
            PlayerHealth playerHealth = hitCollider.transform.GetComponent<PlayerHealth>();

            // CapsuleCollider2D pour éviter d'appliquer les dégats à plusieurs colliders du même perso
            if (hitCollider is CapsuleCollider2D && hitCollider.transform.tag.Substring(0, 4) == "Play")
            {

                playerHealth.TakeDamage(splashDamage + (int) currentDamageBonus);
                hitCollider.attachedRigidbody.AddForce((hitCollider.transform.position - tuba.transform.position).normalized * 20f, ForceMode2D.Impulse);
            }
        }
        Destroy(tuba);
    }


    /* Fonction déclenchée par la récupération d'une arme dans la scène
    * le paramètre 'other' correspond au collider de l'arme collectée
    */
    void OnTriggerEnter2D(Collider2D other) {
		// appel au buffer pour équiper le buff/l'objet sur le player
        if (other.transform.CompareTag("Weapon")) {
          setWeapon(other.gameObject);
	      // appel au CurrentSceneManager pour tenir le compte du nombre de PowerUp dans la scène
	      CurrentSceneManager.instance.CollectedWeapon(other.transform.position);
          Destroy(other.gameObject);
        }
    }
}
