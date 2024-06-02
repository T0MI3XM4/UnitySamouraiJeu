using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneFinale : MonoBehaviour
{


    void Update()
    {
    Invoke("RecommencerJeu", 10f);
                
    }
    void RecommencerJeu()
    {
        SceneManager.LoadScene("Intro");  
    }
  
}
