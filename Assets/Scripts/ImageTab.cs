using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ImageTab : MonoBehaviour
{
    public Sprite[] allImages;
    public string[] imageTitles;
    public GameObject txt;
    public int level;

    public void Start()
    {
        Debug.Log("in start");
        GetComponent<Image>().sprite = allImages[level];
        txt.GetComponent<TextMeshProUGUI>().text = imageTitles[level];
    }

    public void nextImage(){
        Debug.Log("in next image");
        level++;
        if(level>allImages.Length-1){
            level=0;
        }
        GetComponent<Image>().sprite = allImages[level];
        txt.GetComponent<TextMeshProUGUI>().text = imageTitles[level];
    }

    public void previousImage(){
        Debug.Log("in previous image");
        level--;
        if(level<0){
            level=allImages.Length-1;
        }
        GetComponent<Image>().sprite = allImages[level];
        txt.GetComponent<TextMeshProUGUI>().text = imageTitles[level];
    }
}
