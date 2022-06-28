 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CountDownTimer : MonoBehaviour
{
    
    public int countDownTime;
    public Text countDownDisplay;
    public GameObject p1;
    public GameObject p2;
    public bool go=false;
    public bool start = false;

    private void Start() {
        countDownDisplay.text ="Reeeeeeady ?";
        p1.GetComponent<PlayerMovement>().enabled = false;
        p2.GetComponent<PlayerMovement>().enabled = false;
        p1.GetComponent<PlayerWeapon>().enabled = false;
        p2.GetComponent<PlayerWeapon>().enabled = false;
        
        StartCoroutine(CountDownToStart());
    }

    IEnumerator CountDownToStart()
    {
        yield return new  WaitForSeconds(1.0f);
        
        while(countDownTime<=4)
        {
            Debug.Log(Time.realtimeSinceStartup);
            start=true;
            countDownDisplay.text = countDownTime .ToString();
            if(SceneManager.GetActiveScene().name == "SimpleSceneLalaland")
            {
                yield return new  WaitForSeconds(0.968f);
            }else if(SceneManager.GetActiveScene().name == "SimpleSceneOmen")
            {
                yield return new  WaitForSeconds(1.413f);
            }else if(SceneManager.GetActiveScene().name == "SimpleSceneSwing")
            {
                yield return new  WaitForSeconds(0.3f);
            }

            
            
            countDownTime++;
        }
        start = false;
                
        Debug.Log("pouet");
        Debug.Log(Time.realtimeSinceStartup);

        countDownDisplay.text = "Pouet!";
        go=true;
        yield return new WaitForSeconds(0.90f);
        
        p1.GetComponent<PlayerMovement>().enabled = true;
        p2.GetComponent<PlayerMovement>().enabled = true;
        p1.GetComponent<PlayerWeapon>().enabled = true;
        p2.GetComponent<PlayerWeapon>().enabled = true;
        countDownDisplay.gameObject.SetActive(false);
       

    }
}
