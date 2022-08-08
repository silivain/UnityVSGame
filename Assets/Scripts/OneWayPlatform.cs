using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class OneWayPlatform : MonoBehaviour
{
    public GameObject currentOneWayPlatform;                        // dernière plateforme traversée par le joueur
    [SerializeField] private CapsuleCollider2D playerCollider;      // CapsCollider du joueur
    [SerializeField] private BoxCollider2D boxCollider1;            // BoxCollider 1 du joueur
    [SerializeField] private BoxCollider2D boxCollider2;            // BoxCollider 2 du joueur
    private PlayerControls controls;                                // script gérant les inputs du joueur


    /* récupère les bons inputs
    */
    private void Awake() {
        controls = new PlayerControls();    // on recup le script qui gère les inputs
        controlPlayer();                    // on active les bons inputs
    }


    /* active les inputs correspondant au joueur
    */
    void controlPlayer() {
        if (transform.tag == "Player 1") {
            controls.Player1.Enable();
            controls.Player1.GoDown.performed += ctx => GoDown();
        }else{
            controls.Player2.Enable();
            controls.Player2.GoDown.performed += ctx => GoDown();
        }
    }


    /* si le joueur se situe sur une plateforme :
    * appelle la coroutine le faisant déscendre
    */
    private void GoDown() {
        if (currentOneWayPlatform != null) {
            StartCoroutine(DisableCollision());
        }
    }


    /* récupère la plateforme avec laquelle le joueur entre en contact
    */
    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("OneWayPlatform")){
            currentOneWayPlatform = collision.gameObject;
        }
    }


    /* reset la var 'currentOneWayPlatform' lorsque le joueur quitte la plateforme
    */
    private void OnCollisionExit2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("OneWayPlatform")){
            currentOneWayPlatform = null;
        }
    }


    /* désactive la collision du joueur avec la plateforme actuelle
    * fait descendre le joueur puis réactive les collisions après 1 sec
    */
    private IEnumerator DisableCollision(){
        BoxCollider2D plaformCollider = currentOneWayPlatform.GetComponent<BoxCollider2D>();
        Physics2D.IgnoreCollision(playerCollider, plaformCollider);
        Physics2D.IgnoreCollision(boxCollider1, plaformCollider);
        Physics2D.IgnoreCollision(boxCollider2, plaformCollider);


        yield return new WaitForSeconds(1f);
        Physics2D.IgnoreCollision(playerCollider,plaformCollider,false);
        Physics2D.IgnoreCollision(boxCollider1,plaformCollider,false);
        Physics2D.IgnoreCollision(boxCollider2, plaformCollider,false);
    }
}
