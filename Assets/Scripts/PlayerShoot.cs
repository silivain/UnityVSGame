using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
  public KeyCode fire1;						        // touche de tir

  public GameObject playerBullet;					// gameobject tiré lorsque 'fire1' pressed
  public int bulletID = 1;                // ID du projectile
  public Transform throwPoint;				    // point depuis lequel les projectiles sont instanciés

  public static PlayerShoot instance;     // instance de la classe

  private void Awake() {
    instance = this;
  }

    // Update is called once per frame
    void Update()
    {
      // tir
      if (Input.GetKeyDown(fire1)) {
        switch(bulletID) {
          case 0:
            bullet();
            break;
          case 1:
            clarinet();
            break;
          case 2:
            // GRANADA
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
}
