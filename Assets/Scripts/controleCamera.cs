using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControleCamera : MonoBehaviour
{
    // Script à mettre sur la caméra
    public GameObject cibleASuivre;

    // Les valeurs limites que ne peut dépasser la caméra (gauche, droite, haut, bas).
    // À initialiser dans l'inspecteur
    public float limiteGauche;     
    public float limiteDroite;
    public float limiteHaut;
    public float limiteBas;

    void Update()
    {
    // On met en mémoire la position de la cible
    Vector3 laPosition = cibleASuivre.transform.position;
            
    // Si la position x est plus petite que la limite de gauche, on redéfinit la position X à  
    // la limite de gauche. De cette façon, on s'assure que la limite de gauche ne sera 
    // jamais dépassée. On répète la même chose pour les autres limites.
        if(laPosition.x < limiteGauche) laPosition.x  = limiteGauche;
        if(laPosition.x > limiteDroite) laPosition.x  = limiteDroite;
        if(laPosition.y < limiteBas)    laPosition.y  = limiteBas;
        if(laPosition.y > limiteHaut)   laPosition.y  = limiteHaut;
        
    // Pour éviter une erreur, on définit le Z à -10 (valeur par défaut). Si on ne fait
    // pas ça, le Z sera mis à 0 et on ne verra plus les objets de notre scène.
        laPosition.z = -10;
    // On attribue la nouvelle position à la caméra
        transform.position = laPosition;
            
        }
}
