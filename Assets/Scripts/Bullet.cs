using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

   public float bulletSpeed;
   //public GameObject bulletEffect;
   private Rigidbody2D rb;


   // Start is called before the first frame update
   void Start()
   {
     rb = GetComponent<Rigidbody2D>();
   }

   // Update is called once per frame
   void Update()
   {
     rb.velocity = new Vector2(bulletSpeed * transform.localScale.x, 0f);
   }

   void OnTriggerEnter2D(Collider2D other) {
       //Instantiate(bulletEffect, transform.position, transform.position);
       Destroy(gameObject);
   }
}
