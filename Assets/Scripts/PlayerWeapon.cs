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

  private int ammunition = 1000;
  public Image ammunitionBar;			// barre d'affichage des munitions
  public Text ammunitionCountText;		// texte d'affichage du nb de mun

  private Transform playerShield;   // shield du joueur

  public static string[] weapons = {"Bullet", "Clarinet", "Grenade", "tromboneRangePoint", "Sousa", "Tuba"};	// armes du jeu, l'ordre des armes doit match leur weaponID
  private static int[] maxAmmunition = {1000, 13, 7, 25, 21, 15};                                             // nombre de munitions max/de départ pour chaque arme

  private float startTime =0f;
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

	  /* Changement d'arme en fonction de son 'weaponID'
	  *	'weaponID' = position dans le tableau 'weapons'
	  */
      if (Input.GetKeyDown(fire1) && !playerShield.gameObject.activeSelf) {
        switch(weaponID) {
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

    void bullet() {
      GameObject bulletClone = (GameObject) Instantiate(weapon, throwPoint.position, throwPoint.rotation);
	  bulletClone.tag = "Proj" + transform.tag;

	  UseAmmo();

      // TODO indiqué au projectile son parent pour pas se le manger lors d'un dash par ex
      //Debug.Log("player pos : " + transform.position + "\ndebug depuis PlayerMovement, l84"); debug
      //anim.SetTrigger("fire anim"); animation
    }

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

    void trombone() {
        StartCoroutine(tromboneAppear());
        Collider2D[] tromboneHitbox = Physics2D.OverlapAreaAll(throwPoint.position, tromboneRangePoint.position, playerLayers);
        foreach(Collider2D enemy in tromboneHitbox)
        {
            Debug.Log("we hit " + enemy.name);
            PlayerHealth playerHealth = enemy.transform.GetComponent<PlayerHealth>();
            playerHealth.TakeDamage(tromboneDamage);
            // TODO appel à la fonction de recul en passant les arguments nécessaires
            // le collider 'other', le rigidbody du go bullet (pour pouvoir recup sa velocity)
            PlayerMovement.instance.RecoilCac(enemy, transform);
        }
	}

    IEnumerator tromboneAppear()
    {
        tromboneSprite.enabled = true;
        yield return new WaitForSeconds(0.2f);
        tromboneSprite.enabled = false;

		UseAmmo();
    }

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

		yield return new WaitForSeconds(0.15f);
		Destroy(sousa1);
		Destroy(sousa2);
		Destroy(sousa3);
	}

	IEnumerator tuba() {
		GameObject tuba = (GameObject) Instantiate(weapon, throwPoint.position, throwPoint.rotation);
  	tuba.tag = "Proj" + transform.tag;
	UseAmmo();

		yield return new WaitForSeconds(0.75f);
		Destroy(tuba);
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
	  	ammunitionBar.sprite = weaponsItems[weaponID];
	  if (weaponID == 0) {
	    ammunitionCountText.text = "∞";
	  }else{
	    ammunitionCountText.text = ammunition.ToString();
	  }


      // TODO (lancer une anim) + changer l'apparence du player en fonction de l'item
    }


	/* Fonction déclenchée par la récupération d'une arme dans la scène
    * le paramètre 'other' correspond au collider de l'arme collectée
    */
    void OnTriggerEnter2D(Collider2D other) {
		// appel au buffer pour équiper le buff/l'objet sur le player
        if (other.transform.CompareTag("Weapon")) {
          setWeapon(other.gameObject);
	      // appel au CurrentSceneManager pour tenir le compte du nombre de PowerUp dans la scène
	      CurrentSceneManager.instance.CollectedItem();
        }

        Destroy(other.gameObject);
    }
}
