using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class SelectSceneMenu : MonoBehaviour
{
    public Sprite[] allImages;              // images des niveaux
    public string[] imageTitles;            // nom des niveaux
    public GameObject txt;                  // go du texte du nom des niveaux
    public GameObject image;                // go de l'image du niveau
    public int level;                       // indice du niveau actuellement sélectionné
    private bool _lockPrevious = false;     // lock sur previous
    private bool _lockNext = false;         // lock sur next

    // TODO : SimpleSceneSwing
    private string[] levelToLoad = {"SimpleSceneLalaland","SimpleSceneOmen","SimpleSceneOmen"};
    public PlayerControls controls;         // InputSystem


    /* recup les inputs via l'InputActionMap 'UI'
    */
    private void Awake() {
        controls = new PlayerControls();    // on recup les inputs
        controls.UI.Enable();
        controls.UI.GoLeft.performed += ctx => previousImage();
        controls.UI.GoRight.performed += ctx => nextImage();
        controls.UI.Start.performed += ctx => Launch();
    }


    /* init image et nom du niveau
    */
    public void Start() {
        image.GetComponent<Image>().sprite = allImages[level];
        txt.GetComponent<TextMeshProUGUI>().text = imageTitles[level];
    }


    /* affiche l'image et le nom du niveau suivant
    * lock pour éviter le spam
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


    /* affiche l'image et le nom du niveau précédent
    * lock pour éviter le spam
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


    /* charge le niveau sélectionné
    */
    private void Launch() {
        if (gameObject.activeSelf) {
            SceneManager.LoadScene(levelToLoad[level]);
        }
    }


    /* lock sur previous
    */
    IEnumerator lockPrevious() {
        yield return new WaitForSeconds(0.25f);
        _lockPrevious = false;
    }


    /* lock sur next
    */
    IEnumerator lockNext() {
        yield return new WaitForSeconds(0.25f);
        _lockNext = false;
    }
}
