using UnityEngine;

public class HealPowerUp : MonoBehaviour  //video 14
{
    public int healthPoints;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
          PlayerHealth.instance.HealPlayer(healthPoints);
          Destroy(gameObject);
        }
    }
}
