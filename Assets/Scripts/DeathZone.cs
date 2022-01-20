using UnityEngine;

public class DeathZone : MonoBehaviour
{
    // kills the player if he enters a defined area
    // TODO this script is not used yet
    // TODO should we even use it ?? gameplay choice

    /* When player enters deathzone :
    * if he only had 1 life left, call the gameover function
    * else, lifes -= 1 and player is moved back to player spawn
    */
    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.CompareTag("Player")) {
          PlayerLives.instance.Fall(collision.transform);
        }
    }
}
