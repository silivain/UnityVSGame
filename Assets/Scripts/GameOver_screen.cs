using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver_screen : MonoBehaviour
{
    public Text pointsText;
    public void Setup(int score){
        Time.timeScale = 0f; //si le temps a été arreté , il est maintenant en vitesse normale.
        gameObject.SetActive(true);
        pointsText.text = "Tué en "+score.ToString() + " secondes !";  
        
    } 

    public void Restart(){
        Time.timeScale = 1f; //si le temps a été arreté , il est maintenant en vitesse normale.
        SceneManager.LoadScene("SimpleArena");
    }

    public void MainMenu(){
        Time.timeScale = 1f; //si le temps a été arreté , il est maintenant en vitesse normale.
        SceneManager.LoadScene("MainMenu");
        
    }
}
