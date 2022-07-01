using System;
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//Si UnityEngine.InputSystem introuvable, aller dans préférnces, external tools tout cocher et regenerate project files
using UnityEngine.InputSystem;

public class PlayerWeapon : MonoBehaviour
{

  public PlayerControls controls;

  public GameObject weapon;						// arme utilisée par le player lorsque 'fire1' pressed
  public GameObject[] weaponsGO;				// armes du jeu
  public Sprite[] weaponsItems;					// images des items des armes (les collectables)
  public int weaponID;							// ID de l'arme du player
  public Transform throwPoint;				    // point depuis lequel les projectiles sont instanciés
  public Transform highThrowPoint;				// throwPoint haut (souba etc)
  public KeyCode changeWeapon; 					// changement d'arme (debug)
  public GameObject grenadeGO;					// TODO
  public static PlayerWeapon instance;     		// instance de la classe
  public PlayerWeapon enemyPW;                  // script PlayerWeapon du joueur adverse

  public Transform tromboneRangePoint;          // transform délimitant la portée de la coulisse
  public LayerMask playerLayers;                // layers définissant la catégorie d'objets ciblés par la coulisse
  public int tromboneDamage = 8;                // dégats d'un coup de trombone
  public GameObject AnimTrb0;                   // GameObject animation du coup de coulisse
  public GameObject AnimTrb1;

  public bool mySolo = false;                   // vrai si mon joueur est équipé d'une flute et fait un solo
  public bool enemySolo = false;                // vrai si le joueur adverse est équipé d'une flute et fait un solo
  public float timeBeforeSolo;                  // durée avant le solo de flute
  public float soloDuration;                    // durée du solo de flute

  private Boolean isWeaponReady = true;         // vrai si le cooldown de l'arme est à 0
  public int ammunition = 1000;                 // nombre de munitions restatntes pour l'arme équipée
  public Image ammunitionBar;			        // barre d'affichage des munitions
  public Text ammunitionCountText;		        // texte d'affichage du nb de mun

  public Transform playerShield;               // shield du joueur

  public int splashDamage = 10;                 // dégats de zone de l'explosion du tuba
  public float splashRange = 3f;                // taille de la zone de dégats
  public GameObject explosionVisual;            // visuel de l'explosion

  public  static string[] weapons         = {"Bullet", "Clarinet", "Grenade", "Trombone", "Sousa", "Tuba", "Flute"};	// armes du jeu, l'ordre des armes doit match leur weaponID
  public         int[]    maxAmmunition   = {1000, 13, 7, 25, 21, 15, 1000};                                                  // nombre de munitions max/de départ pour chaque arme
  public         int[]    bonusAmmunition = {1000, 6, 3, 12, 10, 7, 1000};                                                    // nombre du munitions apportés par l'item 'Ammunition'
  private static float[]  cooldownTime    = {.5f, 1f, 2f, .5f, 1f, 1f, 0.5f};                                                 // Cooldown de chaque arme

  public float currentDamageBonus = 0;        // bonus de dégats actuel

  private float startTime = 0f;
    //private float endTime=0f; TODO
    public int deviceNumber;            //Numero de device du gamepad


    private void Awake() {
        controls = new PlayerControls();
        controls.devices = new[] { InputSystem.devices[deviceNumber] };

        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        controls.Gameplay.Shoot.performed += ctx => Shoot();
    }

    private void Shoot()
    {
        if (!playerShield.gameObject.activeSelf && isWeaponReady)
        {
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
                    StartCoroutine(trombone());
                    break;
                case 4:
                	StartCoroutine(sousa());
                    break;
                case 5:
                  	StartCoroutine(tuba());
                	break;
                case 6:
                	StartCoroutine(flute());
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
        if (!enemySolo) {
            GameObject bulletClone = (GameObject) Instantiate(weapon, throwPoint.position, throwPoint.rotation);
            bulletClone.tag = "Proj" + transform.tag;

            UseAmmo();

            //anim.SetTrigger("fire anim"); animation
        }
    }


    /* Tire 3 projs de clarinette
    * les projs suivent une traj courbée
    * ils sont instantiés les uns sous les autres ingame
    */
    void clarinet() {
        if (!enemySolo) {
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
    }


    /* Tire une grenade
    * TODO : commenter pls
    */
    void grenadeLaunch(){
        if (!enemySolo) {
            //lengthTime = endTime-startTime;
            float lengthTime = 3;
            Vector3 vectorClarinet = throwPoint.position;
            GameObject grenade = Instantiate(grenadeGO, vectorClarinet, throwPoint.rotation);
            grenade.tag = "Proj" + transform.tag;
            Rigidbody2D projRb = grenade.GetComponent<Rigidbody2D>();
            projRb.AddForce(new Vector2(1*lengthTime,2*lengthTime));
            projRb.angularVelocity = -180;

            UseAmmo();

            //Destroy(grenade,Random.Range(1,10));
        }
    }


    /* Coup de trombone au cac
    * on récupère tous les collider entre le joueur et 'tromboneRangePoint'
    * on applique les dégats du trombone à tous les joueurs récupérés
    */
    IEnumerator trombone() {
        if (!enemySolo) {
            AnimTrb0.SetActive(false);
            AnimTrb1.SetActive(true);
            yield return new WaitForSeconds(0.133f);

        AnimTrb0.SetActive(false);
        AnimTrb1.SetActive(true);
        yield return new WaitForSeconds(0.133f);

        Collider2D[] tromboneHitbox = Physics2D.OverlapAreaAll(throwPoint.position,
            tromboneRangePoint.position, playerLayers);
        foreach(Collider2D enemy in tromboneHitbox) {
            if(enemy.transform.tag[enemy.transform.tag.Length - 1] != transform.tag[transform.tag.Length - 1]
    		&& enemy.transform.tag.Substring(0, 4) == "Play"
    		&& enemy is CapsuleCollider2D) {
                PlayerHealth playerHealth = enemy.transform.GetComponent<PlayerHealth>();
                PlayerMovement playerMovement = enemy.transform.GetComponent<PlayerMovement>();

                playerHealth.TakeDamage(tromboneDamage);
                playerMovement.RecoilCac(enemy, transform);
            }
        }

            yield return new WaitForSeconds(0.36f);
            AnimTrb1.SetActive(false);
            AnimTrb0.SetActive(true);

            UseAmmo();
        }
	}

    /* Tire 3 projs de sousa
    * les projs disparaissent au bout de 1 sec
    * TODO : faire une var pour déterminer la range = durée avant destroy
    */
	IEnumerator sousa() {
        if (!enemySolo) {
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
            if (sousa1 != null) {
    		          Destroy(sousa1);
            }
            if (sousa2 != null) {
    		          Destroy(sousa2);
            }
            if (sousa3 != null) {
    		          Destroy(sousa3);
            }
        }
	}


    /* Crée un projectile 'tuba'
    * le projectile explose :
    * - au contact d'un collider
    * - au bout de 0.75 sec sinon
    * TODO : faire une var pour la durée avant explo
    */
	IEnumerator tuba() {
        if (!enemySolo) {
    		GameObject tuba = (GameObject) Instantiate(weapon, throwPoint.position, throwPoint.rotation);
      	    tuba.tag = "Proj" + transform.tag;
    	    UseAmmo();

            yield return new WaitForSeconds(0.75f);
            if(tuba != null) {
                Explosion(tuba);
            }
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


    /* TODO
    */
    IEnumerator flute() {
        if (mySolo) {
            GameObject flute1 = (GameObject) Instantiate(weapon, throwPoint.position, throwPoint.rotation);
      	  	flute1.tag = "Proj" + transform.tag;

    		GameObject flute2 = (GameObject) Instantiate(weapon, throwPoint.position, throwPoint.rotation);
      	  	flute2.tag = "Proj" + transform.tag;
    		Rigidbody2D rbFlute2 = flute2.GetComponent<Rigidbody2D>();
    		rbFlute2.SetRotation(transform.rotation.y >= 0 ? (rbFlute2.rotation - 3f) : (rbFlute2.rotation + 3f));

    		GameObject flute3 = (GameObject) Instantiate(weapon, throwPoint.position, throwPoint.rotation);
      	  	flute3.tag = "Proj" + transform.tag;
    		Rigidbody2D rbFlute3 = flute3.GetComponent<Rigidbody2D>();
    		rbFlute3.SetRotation(transform.rotation.y >= 0 ? (rbFlute3.rotation + 3f) : (rbFlute3.rotation - 3f));

    		yield return new WaitForSeconds(1f);
            if (flute1 != null) {
    		          Destroy(flute1);
            }
            if (flute2 != null) {
    		          Destroy(flute2);
            }
            if (flute3 != null) {
    		          Destroy(flute3);
            }
        }
    }


    /* Gère le cooldown de la flute
    * attends 'timeBeforeSolo' secondes avant de pouvoir tirer
    * le solo dure 'soloDuration' secondes
    */
    IEnumerator cooldownFlute() {
        while(weaponID == 6) {
            Debug.Log("debut boucle cd flute");
            yield return new WaitForSeconds(timeBeforeSolo);
            Debug.Log("debut solo flute");
            mySolo = true;
            enemyPW.enemySolo = true;
            yield return new WaitForSeconds(soloDuration);
            mySolo = false;
            enemyPW.enemySolo = false;
            Debug.Log("fin solo flute");
        }
    }


	/* Utilise une munition de l'arme équipée
	* Si le joueur n'a plus de munitions, rééquipe l'arme de base
	*/
	void UseAmmo() {
        if (ammunition-- == 1) {
            weaponID = 0;
            weapon = weaponsGO[weaponID];
            ammunition = maxAmmunition[0];
            AnimTrb0.SetActive(false);
            ammunitionCountText.text = "∞";
            ammunitionBar.sprite = weaponsItems[weaponID];
        }
        if (weaponID != 0) {
            ammunitionCountText.text = ammunition.ToString();
        }
	}


    /* Gère le cooldown de l'arme actuelle en fonction des valeurs de 'cooldownTime'
    */
    IEnumerator cooldownWeapon() {
        if (!isWeaponReady) {
            yield return new WaitForSeconds(cooldownTime[weaponID]);
            isWeaponReady = true;
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


      /* Lorsqu'on récup le trombone :
      * on récup les GO d'animation
      */
      if (weaponID == 3) {
          AnimTrb0.SetActive(true);
      }else {
          AnimTrb0.SetActive(false);
      }


      /* Lorsqu'on récupère la flûte :
      * démarre le cooldown avant le solo
      */
      if (weaponID == 6) {
          StartCoroutine(cooldownFlute());
      }

      // TODO (lancer une anim) + changer l'apparence du player en fonction de l'item
    }


    // met à jour l'affichage des munitions
    public void AmmoDisplay() {
        ammunitionBar.sprite = weaponsItems[weaponID];
        if (weaponID == 0 || weaponID == 6) {
            ammunitionCountText.text = "∞";
        }else{
            ammunitionCountText.text = ammunition.ToString();
        }
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

    private void OnEnable()
    {
        controls.Gameplay.Enable();
    }

    private void OnDisable()
    {
        controls.Gameplay.Disable();
    }
}
