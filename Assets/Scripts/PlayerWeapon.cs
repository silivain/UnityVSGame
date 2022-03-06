using System;
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
  public KeyCode fire1;						    // touche de tir

  public GameObject weapon;						// arme utilisée par le player lorsque 'fire1' pressed
  public GameObject[] weaponsGO;				// armes du jeu
  public int weaponID;							// ID de l'arme du player
  public Transform throwPoint;				    // point depuis lequel les projectiles sont instanciés
  public KeyCode changeWeapon; 					// changement d'arme (debug)
  public GameObject grenadeGO;					// TODO
  public static PlayerWeapon instance;     		// instance de la classe
  public float force;							// TODO

  public static string[] weapons= {"Bullet", "Clarinet", "Grenade"};	// armes du jeu, l'ordre des armes doit match leur weaponID

  private float startTime =0f;
  //private float endTime=0f; TODO


  private void Awake() {
    instance = this;
  }

    // Update is called once per frame
    void Update()
    {
      // tir
      if (Input.GetKeyDown(changeWeapon)){
        weaponID++;
        if(weaponID>2)
        {
          weaponID = 0;
        }
      }


      if (Input.GetKeyDown(fire1)) {
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
          default:
            bullet();
            break;
        }
      }
    }

    void bullet() {
      GameObject bulletClone = (GameObject) Instantiate(weapon, throwPoint.position, throwPoint.rotation);
      // TODO indiqué au projectile son parent pour pas se le manger lors d'un dash par ex
      //Debug.Log("player pos : " + transform.position + "\ndebug depuis PlayerMovement, l84"); debug
      //anim.SetTrigger("fire anim"); animation
    }

    void clarinet() {
      Vector3 vectorClarinet = throwPoint.position;
      GameObject clarinetClone1 = (GameObject)Instantiate(weapon, vectorClarinet, throwPoint.rotation);
      vectorClarinet.y += 0.5f;
      GameObject clarinetClone2 = (GameObject)Instantiate(weapon, vectorClarinet, throwPoint.rotation);
      vectorClarinet.y += 0.5f;
      GameObject clarinetClone3 = (GameObject)Instantiate(weapon, vectorClarinet, throwPoint.rotation);

      //// TODO indiqué au projectile son parent pour pas se le manger lors d'un dash par ex
      //Debug.Log("player pos : " + transform.position + "\ndebug depuis PlayerMovement, l84"); debug
      //anim.SetTrigger("fire anim"); animation
    }

    void grenadeLaunch(){
      //lengthTime = endTime-startTime;
      float lengthTime = 3;
      Vector3 vectorClarinet = throwPoint.position;
      GameObject grenade = Instantiate(grenadeGO, vectorClarinet, throwPoint.rotation);
      Rigidbody2D projRb = grenade.GetComponent<Rigidbody2D>();
      projRb.AddForce(new Vector2(1*force*lengthTime,2*force*lengthTime));
      projRb.angularVelocity = -180;
      //Destroy(grenade,Random.Range(1,10));
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
	  //Debug.Log("ID de l'arme équipée : " + wName);

	  Predicate<string> checkWeapon = arrayEl => arrayEl == wName;
      weaponID = Array.FindIndex(weapons, checkWeapon);
	  weapon = weaponsGO[weaponID];

      // TODO (lancer une anim) + changer l'apparence du player en fonction de l'item
    }


	/* Fonction déclenchée par la récupération d'une arme dans la scène
    * le paramètre 'other' correspond au collider de l'arme collectée
    */
    void OnTriggerEnter2D(Collider2D other) {
        // appel au CurrentSceneManager pour tenir le compte du nombre de PowerUp dans la scène
        CurrentSceneManager.instance.CollectedItem();

        // appel au buffer pour équiper le buff/l'objet sur le player
        if (other.transform.CompareTag("Weapon")) {
          setWeapon(other.gameObject);
        }

        Destroy(other.gameObject);
    }
}