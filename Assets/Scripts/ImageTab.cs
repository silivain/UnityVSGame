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
    
    public void Update()
    {
        GetComponent<Image>().sprite=allImages[level];
        txt.GetComponent<TextMeshProUGUI>().text = imageTitles[level];
    }

    public void nextImage(){
        level++;
        if(level>allImages.Length-1){
            level=0;
        }
    }
    
    public void previousImage(){
        level--;
        if(level<0){
            level=allImages.Length-1;
        }
    }
}
