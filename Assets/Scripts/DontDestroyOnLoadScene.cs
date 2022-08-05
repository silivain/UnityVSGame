using UnityEngine;
using UnityEngine.SceneManagement;

// list of gameobjects that must not be detroyed between scences
// TODO this script is not used yet
/* TODO not sure we should use it, since players will reselect their
                                          characters between games */
public class DontDestroyOnLoadScene : MonoBehaviour
{
    public GameObject[] objects;                    // list of gameobjects to preserve

    public static DontDestroyOnLoadScene instance;  // class instance (only 1 in the entire game)


    private void Awake()
    {
      if (instance != null)
      {
        Debug.LogWarning("Il y a plus d'une instance de DontDestroyOnLoadScene dans la scène.");
        return;
      }
      instance = this;

      foreach(var element in objects)
      {
        DontDestroyOnLoad(element);   // sets the gameobjects to 'DontDestroyOnLoad'
      }
    }


    /* Remove gameobjects from the 'DontDestroyOnLoad' list
    */
    public void RemoveFromDontDestroyOnLoad() //video 16
    {
      foreach(var element in objects)
      {
        SceneManager.MoveGameObjectToScene(element, SceneManager.GetActiveScene());
      }
    }
}
