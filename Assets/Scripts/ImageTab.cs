using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ImageTab : MonoBehaviour
{
    // TODO : script plus utilisé, remplacé par 'SelectSceneMenu' => à supprimer

    public Sprite[] allImages;
    public string[] imageTitles;
    public GameObject txt;
    public int level;
    private bool _lockPrevious = false;
    private bool _lockNext = false;


    public void Start()
    {
        GetComponent<Image>().sprite = allImages[level];
        txt.GetComponent<TextMeshProUGUI>().text = imageTitles[level];
    }


    public void nextImage(){
        if (!_lockNext) {
            _lockNext = true;
            StartCoroutine(lockNext());
            level = (level + 1) % (allImages.Length - 1);
            GetComponent<Image>().sprite = allImages[level];
            txt.GetComponent<TextMeshProUGUI>().text = imageTitles[level];
        }
    }


    public void previousImage(){
        if (!_lockPrevious) {
            _lockPrevious = true;
            StartCoroutine(lockPrevious());
            if(--level < 0){
                level = allImages.Length - 1;
            }
            GetComponent<Image>().sprite = allImages[level];
            txt.GetComponent<TextMeshProUGUI>().text = imageTitles[level];
        }
    }


    IEnumerator lockPrevious() {
        yield return new WaitForSeconds(0.25f);
        _lockPrevious = false;
    }


    IEnumerator lockNext() {
        yield return new WaitForSeconds(0.25f);
        _lockNext = false;
    }
}
