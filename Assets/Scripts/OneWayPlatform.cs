using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneWayPlatform : MonoBehaviour
{
    public GameObject currentOneWayPlatform;
    [SerializeField] private CapsuleCollider2D playerCollider;
    [SerializeField] private BoxCollider2D boxCollider1;
    [SerializeField] private BoxCollider2D boxCollider2;
    public PlayerControls controls;

    private void Awake()
    {
        controls = new PlayerControls();
    }

    // Update is called once per frame
    void Update()
    {
        controls.Gameplay.GoDown.performed += ctx => GoDown();

    }

    private void GoDown()
    {
        if (currentOneWayPlatform != null)
        {
            StartCoroutine(DisableCollision());
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("OneWayPlatform")){
                currentOneWayPlatform = collision.gameObject;
        }

    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("OneWayPlatform")){
            currentOneWayPlatform =null;
        }
        
    }
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
    private void OnEnable()
    {
        controls.Gameplay.Enable();
    }

    private void OnDisable()
    {
        controls.Gameplay.Disable();
    }
}
