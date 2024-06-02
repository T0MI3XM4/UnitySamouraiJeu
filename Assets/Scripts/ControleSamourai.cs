using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting.ReorderableList.Element_Adder_Menu;


public class ControleSamourai : MonoBehaviour
{
    public float vitesseX;                 //vitesse horizontale actuelle
    public float vitesseMax;               //vitesse horizontale Maximale désirée
    public float vitesseY;                 //vitesse verticale 
    public float vitesseSaut;              //vitesse de saut désirée

    public float vitesseMaximale;          //vitesse maximal pour attaque

    public GameObject parcheminLettre;      //Parchemin avec instructions

    int pointagePiece = 0;                  //nombre de pièces d'or accumulés
    int pointageVie = 5;                    //nombre de vies restantes

    public AudioClip sonOr;                 //Son des pièces d'or
    public AudioClip sonFlute;              //Son de vies
    public AudioClip sonMort;               //Son quand le personnage meurt   
    public AudioClip sonPiegeImpact;        //Son de collision avec un piège
    public AudioClip sonAttaque;            //Son lors de l'attaque

    public AudioClip sonParchemin;           //Son lors de L'ouverture du parchemin

    public TextMeshPro textePointageOr;      //Texte de pointage des pièces d'or

    public TextMeshPro textePointageVie;     //Texte de pointage des vies restrantes


    public bool peutAttaquer = false;          //Éviter la double attaque



    /*Détection des touches et modification de la vitesse de déplacement;
      flèche droite et gauche pour avancer et reculer, flèche du haut pour sauter tant que la partie n'est pas terminée
    */
    void Update()
    {
        if(pointageVie > 0){
        if (Input.GetKey(KeyCode.LeftArrow))                  //déplacement vers la gauche
        {
            vitesseX = -vitesseMax;
            GetComponent<SpriteRenderer>().flipX = true;
            
        }

        else if (Input.GetKey(KeyCode.RightArrow))             //déplacement vers la droite
        {
            vitesseX = vitesseMax;
            GetComponent<SpriteRenderer>().flipX = false;
        }
        
        else
        {
            vitesseX = GetComponent<Rigidbody2D>().velocity.x;   //mémorise vitesse actuelle en X
        }
        
        if (Input.GetKeyDown(KeyCode.UpArrow) && Physics2D.OverlapCircle(transform.position,0.25f))
        {
            vitesseY = vitesseSaut;
            GetComponent<Animator>().SetBool("saut", true);
        }

        else
        {
            vitesseY = GetComponent<Rigidbody2D>().velocity.y;  //vitesse actuelle verticale
        }

            
        if (Input.GetKeyDown(KeyCode.Space) && peutAttaquer == false)                  // Déclenche l'attaque
        {
            peutAttaquer = true;
            GetComponent<Animator>().SetBool("attaque", true);
            GetComponent<Animator>().SetBool("saut", false);
            Invoke("AttaquerDeNouveau", 0.4f);

            if(GetComponent<Animator>().GetBool("attaque") == true){
            vitesseX *= vitesseMaximale;
            }
            

        }
        
                //Applique les vitesses en X et Y
                GetComponent<Rigidbody2D>().velocity = new Vector2(vitesseX, vitesseY);
        }

         //Active l'animation de course si la vitesse de déplacement est plus grand que 0.01f, sinon le repos sera jouer par Animator
        if (Mathf.Abs(vitesseX) > 0.9f)
        {
            GetComponent<Animator>().SetBool("course", true);
        }

        else
        {
            GetComponent<Animator>().SetBool("course", false);
        }


         //Contenu texte du texte de pointage de pièces d'ors
         textePointageOr.text = "Pièces D'Or x " + pointagePiece;

         //Contenu texte du texte de pointage de vies
         textePointageVie.text = "Vies x " + pointageVie;

         if(pointageVie >= 3)
         {
            pointageVie = 3;
         }

         if(pointageVie <= 0 )
         {
            pointageVie = 0;
         }
    }
        
        void OnCollisionEnter2D(Collision2D InfoCollision){
        
        // Le saut est possible que quand le Samourai est sur un objet (Pas de double saut)
        if (Physics2D.OverlapCircle(transform.position,0.25f))
        {
                GetComponent<Animator>().SetBool("saut", false);
        }

        //  Ouvrir et fermer les instructions sur le parchemin
        if (InfoCollision.gameObject.name == "parchemin")
        {
            GetComponent<AudioSource>().PlayOneShot(sonParchemin);
            parcheminLettre.SetActive(true);
        }

        else{
           parcheminLettre.SetActive(false);   
        }
        // Collision avec pièce d'or
        if (InfoCollision.gameObject.name == "piece")
        {
            GetComponent<AudioSource>().PlayOneShot(sonOr);
            pointagePiece += 1;
            InfoCollision.gameObject.SetActive(false);
        }

        // Collision avec une vie
        if (InfoCollision.gameObject.name == "coeur")
        {
            GetComponent<AudioSource>().PlayOneShot(sonFlute);
            pointageVie += 1;
            InfoCollision.gameObject.SetActive(false);
        }

        // Collision avec un piège
        if (InfoCollision.gameObject.tag == "piege")
        {
            GetComponent<AudioSource>().PlayOneShot(sonPiegeImpact);
            pointageVie -= 1;

            if(pointageVie <= 0)
            {
                GetComponent<Animator>().SetBool("mort", true);
                GetComponent<AudioSource>().PlayOneShot(sonMort);
                Invoke("FinMort", 2f);
            }
        }

        //Collision avec un ennemi
        if (InfoCollision.gameObject.tag == "demon")
        {

                if(GetComponent<Animator>().GetBool("attaque") == true) 
                {
                    GetComponent<AudioSource>().PlayOneShot(sonParchemin);
                    Destroy(InfoCollision.gameObject);
                }

                else if(GetComponent<Animator>().GetBool("attaque") == false && pointageVie > 0)
                {
                pointageVie -= 1;
                }

                if(GetComponent<Animator>().GetBool("attaque") == false &&  pointageVie <= 0)
                {
                    GetComponent<Animator>().SetBool("mort", true);
                    GetComponent<AudioSource>().PlayOneShot(sonMort);
                    Invoke("FinMort", 2f);
                }
            }


        // Fin de la partie Gagné ou Perdu (Si arriver au bout du jeu)
        if (InfoCollision.gameObject.name == "FinDeScene"){
 

        if (pointagePiece >= 8)
        {
            Invoke("FinGagne", 0.1f);

        }

        else
        {
            Invoke("FinMort", 2f);

        }

        }


    }
    
    // Fin perdu
    void FinMort(){
        SceneManager.LoadScene("Perdu");
    }

    // Fin gagné
    void FinGagne(){
         SceneManager.LoadScene("Gagné");
    }

    // Attaquer de nouveau
    void AttaquerDeNouveau()
    {
        peutAttaquer = false;  
        GetComponent<Animator>().SetBool("attaque", false);

    }


}

