using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
	public int splashDamage = 15;
	public float splashRange = 2f;
	public GameObject explosionVisual;


	// Start is called before the first frame update
	void Start()
    {
		Instantiate(explosionVisual, gameObject.transform.position, transform.rotation = Quaternion.identity);
		Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, splashRange);
		foreach (Collider2D hitCollider in hitColliders)
		{
			if (hitCollider is CapsuleCollider2D && hitCollider.transform.tag.Substring(0, 4) == "Play")
			{
				PlayerHealth playerHealth = hitCollider.transform.GetComponent<PlayerHealth>();
				playerHealth.TakeDamage(splashDamage);
				hitCollider.attachedRigidbody.AddForce((hitCollider.transform.position - gameObject.transform.position).normalized * 50f, ForceMode2D.Impulse);
			}
		}
		Destroy(gameObject);
	}
}
