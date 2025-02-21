using UnityEngine;

public class DureeViePoisson : MonoBehaviour
{
    [Header("Paramètres de durée de vie")]
    public float tempsDeVie = 360f;         // Durée de vie en secondes
    private bool estDansContainer = false; // Pour suivre si le poisson est dans le container
    private float compteurTemps = 0f;     // Compteur de temps

    private void Update()
    {
        // Si le poisson n'est pas dans le container, on compte le temps
        if (!estDansContainer)
        {
            compteurTemps += Time.deltaTime;

            // Si le temps est écoulé, on détruit le poisson
            if (compteurTemps >= tempsDeVie)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Vérifie si l'objet entré en collision a le tag "container_epreuve"
        if (other.CompareTag("container_epreuve"))
        {
            estDansContainer = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Si le poisson sort du container, on réactive le compteur
        if (other.CompareTag("container_epreuve"))
        {
            estDansContainer = false;
        }
    }
}
