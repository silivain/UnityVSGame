using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    public string ID;

    private void Awake()
    {
      GameObject.FindGameObjectWithTag(ID).transform.position = transform.position;
    }
}
