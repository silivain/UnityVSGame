using UnityEngine;

// set a smooth camera follow when the player moves on screen
// TODO this script is not used yet
// TODO this script is made for one player
public class CameraFollow : MonoBehaviour	//fin video 3
{
    public GameObject player; // player the camera is focused on
    private Vector3 velocity; // camera speed

    // timeOffset and posOffset make the camera movements look more comfortable
    public float timeOffset;  // time between player and camera movement
                              // camera doesnt follow player at the exact same frame

    public Vector3 posOffset; // position offset between player and camera
                              // player is not centered on screen


    void Update()
    {
      // moves camera from current position to (player position + 'posOffset') at 'velocity' speed, after waiting 'timeOffset'
      transform.position = Vector3.SmoothDamp(transform.position, player.transform.position + posOffset, ref velocity, timeOffset);
    }
}
