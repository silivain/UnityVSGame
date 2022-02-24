using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
  public KeyCode fire1;						        // touche de tir

  public GameObject playerBullet;					// gameobject tiré lorsque 'fire1' pressed
  public int bulletID = 1;                // ID du projectile
  public Transform throwPoint;				    // point depuis lequel les projectiles sont instanciés
  public KeyCode changeWeapon; // changement d'arme (debug)
  public GameObject grenadeGO;
  public static PlayerShoot instance;     // instance de la classe
  public float force;

  private float startTime =0f;
  private float endTime=0f;
  

  private void Awake() {
    instance = this;
  }

    // Update is called once per frame
    void Update()
    {
      // tir
      if (Input.GetKeyDown(changeWeapon)){
        bulletID++;
        if(bulletID>2)
        {
          bulletID = 0;
        }
      }


      if (Input.GetKeyDown(fire1)) {
        switch(bulletID) {
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
      GameObject bulletClone = (GameObject) Instantiate(playerBullet, throwPoint.position, throwPoint.rotation);
      // TODO indiqué au projectile son parent pour pas se le manger lors d'un dash par ex
      //Debug.Log("player pos : " + transform.position + "\ndebug depuis PlayerMovement, l84"); debug
      //anim.SetTrigger("fire anim"); animation
    }

    void clarinet() {
      Vector3 vectorClarinet = throwPoint.position;
      GameObject clarinetClone1 = (GameObject)Instantiate(playerBullet, vectorClarinet, throwPoint.rotation);
      vectorClarinet.y += 0.5f;
      GameObject clarinetClone2 = (GameObject)Instantiate(playerBullet, vectorClarinet, throwPoint.rotation);
      vectorClarinet.y += 0.5f;
      GameObject clarinetClone3 = (GameObject)Instantiate(playerBullet, vectorClarinet, throwPoint.rotation);

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
      Destroy(grenade,Random.Range(1,10)); 
    }
}
