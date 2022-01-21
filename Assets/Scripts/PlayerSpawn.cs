using UnityEngine;

// spawn des joueurs sur la scène
public class PlayerSpawn : MonoBehaviour
{
    public string ID;	// tag du joueur concerné par le spawn

	/* place le joueur de tag 'ID' sur le spawn
	*/
    private void Awake() {
      GameObject.FindGameObjectWithTag(ID).transform.position = transform.position;
    }
}
