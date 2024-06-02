using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangerCamera : MonoBehaviour
{
    // Variables contenant les références aux caméras. À définir dans l'inspecteur
    public GameObject Camera1;
    public GameObject Camera2;

    // Au départ, on active la caméra 1 et on désactive la caméra 2
    void Start()
    {
    Camera1.SetActive(true);
    Camera2.SetActive(false);
        
    }

    // Détection de touches du clavier pour changer les caméras
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1)) // Touche 1 (en haut des lettres)
        {
            Camera1.SetActive(true);
            Camera2.SetActive(false);
        }
        else if(Input.GetKeyDown(KeyCode.Alpha2))// Touche 2 (en haut des lettres)
        {
            Camera2.SetActive(true);
            Camera1.SetActive(false);
        }

        
    }
}
