using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDownTimer : MonoBehaviour
{
    
    public int countDownTime;
    public Text countDownDisplay;
    public GameObject p1;
    public GameObject p2;

    private void Start() {
        
        p1.GetComponent<PlayerMovement>().enabled = false;
        p2.GetComponent<PlayerMovement>().enabled = false;
        p1.GetComponent<PlayerWeapon>().enabled = false;
        p2.GetComponent<PlayerWeapon>().enabled = false;
        StartCoroutine(CountDownToStart());
    }

    IEnumerator CountDownToStart()
    {
        while(countDownTime>0)
        {
            countDownDisplay.text =  countDownTime .ToString();
            yield return new  WaitForSeconds(1f);

            countDownTime--;

        }

        countDownDisplay.text = "Pouet!";
   
        
        p1.GetComponent<PlayerMovement>().enabled = true;
        p2.GetComponent<PlayerMovement>().enabled = true;
        p1.GetComponent<PlayerWeapon>().enabled = true;
        p2.GetComponent<PlayerWeapon>().enabled = true;

        yield return new WaitForSeconds(1f); 

        countDownDisplay.gameObject.SetActive(false);
       

    }
}
