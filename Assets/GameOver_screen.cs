using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver_screen : MonoBehaviour
{
    public Text pointsText;
    public void Setup(int score){
        gameObject.SetActive(true);
        pointsText.text = score.ToString() + " Points";  
        
    } 

    public void Restart(){
        SceneManager.LoadScene("SimpleArena");
    }

    public void MainMenu(){
        SceneManager.LoadScene("MainMenu");
        
    }
}
