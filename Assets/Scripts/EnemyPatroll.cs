using UnityEngine;

// create a path for mobs to follow, mobs will loop on this path
// TODO this script is not used yet
// TODO create trapped items like bombs that run on defined zones before exploding for ex ?
public class EnemyPatroll : MonoBehaviour
{
    public float speed;                 // mob speed
    public Transform[] waypoints;       // points the mob will pass through

    public int damageOnCollision = 20;  // mob dmgs on player collision
    public SpriteRenderer graphics;     // mob's graphics
    private Transform target;           // mob current destination (a 'waypoints' transform)
    private int destPoint = 0;          // current index in 'waypoints' list

    void Start()
    {
      target = waypoints[0];            // set current destination to 'waypoints' first element
    }

    void Update()
    {
      // calculate new direction using the current destination point
      Vector3 dir = target.position - transform.position;

      // moves the mob according to 'direction' and 'speed'
      transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);

      /* when mob reaches current destination
      * loads next destination (loop on 'waypoints')
      * flips mob graphics (cause new destination => change of direction)
      */
      if(Vector3.Distance(transform.position, target.position) < 0.3f) { // security value
        destPoint = (destPoint + 1) % waypoints.Length;
        target = waypoints[destPoint];
        graphics.flipX = !graphics.flipX;
      }
    }

    // deals damage to the collided player calling 'TakeDamage' fct
    private void OnCollisionEnter2D(Collision2D collision) {
      if(collision.transform.CompareTag("Player")) {
        PlayerHealth playerHealth = collision.transform.GetComponent<PlayerHealth>();
        playerHealth.TakeDamage(damageOnCollision);
      }
    }
}
