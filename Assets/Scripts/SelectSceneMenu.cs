using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class SelectSceneMenu : MonoBehaviour
{
    public Sprite[] allImages;              // levels pictures
    public string[] imageTitles;            // pictures names
    public GameObject txt;                  // level name gameobject
    public GameObject image;                // level picture gameobject
    public int level;                       // index of currently selected level
    private bool _lockPrevious = false;     // lock on previous level
    private bool _lockNext = false;         // lock on next level

    // TODO : SimpleSceneSwing
    private string[] levelToLoad = {"SimpleSceneLalaland","SimpleSceneOmen","SimpleSceneOmen"};
    public PlayerControls controls;         // InputSystem


    /* instantiate inputs
    */
    public void Awake() {
        controls = new PlayerControls();    // link with script handling inputs
        controls.UI.Enable();               // listen to 'UI' InputActionMap
        controls.UI.GoLeft.performed += ctx => previousImage();
        controls.UI.GoRight.performed += ctx => nextImage();
        controls.UI.Start.performed += ctx => Launch();
    }


    /* initialize name and picture of currently selected level
    */
    private void Start() {
        image.GetComponent<Image>().sprite = allImages[level];
        txt.GetComponent<TextMeshProUGUI>().text = imageTitles[level];
    }


    /* display name and picture of next level
    * lock to avoid spamming
    */
    public void nextImage(){
        if (!_lockNext && gameObject.activeSelf) {
            _lockNext = true;
            StartCoroutine(lockNext());
            level = (level + 1) % allImages.Length;
            image.GetComponent<Image>().sprite = allImages[level];
            txt.GetComponent<TextMeshProUGUI>().text = imageTitles[level];
        }
    }


    /* display name and picture of previous level
    * lock to avoid spamming
    */
    public void previousImage(){
        if (!_lockPrevious && gameObject.activeSelf) {
            _lockPrevious = true;
            StartCoroutine(lockPrevious());
            if(--level < 0){
                level = allImages.Length - 1;
            }
            image.GetComponent<Image>().sprite = allImages[level];
            txt.GetComponent<TextMeshProUGUI>().text = imageTitles[level];
        }
    }


    /* load selected level
    */
    private void Launch() {
        if (gameObject.activeSelf) {
            controls.UI.Disable();
            SceneManager.LoadScene(levelToLoad[level]);
        }
    }


    /* lock on previous
    */
    IEnumerator lockPrevious() {
        yield return new WaitForSeconds(0.25f);
        _lockPrevious = false;
    }


    /* lock on next
    */
    IEnumerator lockNext() {
        yield return new WaitForSeconds(0.25f);
        _lockNext = false;
    }
}
