using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Panier : MonoBehaviour
{
    [Header("Database")]
    public PoissonDatabase poissonDatabase;

    [Header("References")]
    public TextMeshPro textMesh;
    public GameObject objectToDestroy; // L'objet à détruire quand tous les paniers sont validés

    // Variables statiques partagées entre tous les paniers
    private static int paniersValides = 0;
    private static int nombreTotalPaniers = 4;

    private string nomPoissonCible;
    private GameObject prefabPoissonCible;
    private bool estValide = false; // Pour suivre si ce panier spécifique est validé

    private void Start()
    {
        // Réinitialiser le compteur au démarrage du jeu
        if (gameObject.scene.buildIndex == 0) // Si c'est le premier panier chargé
        {
            paniersValides = 0;
        }
        ChoisirPoissonAleatoire();
    }

    private void ChoisirPoissonAleatoire()
    {
        if (poissonDatabase.poissons == null || poissonDatabase.poissons.Count == 0)
        {
            Debug.LogError("La base de données de poissons est vide ou non assignée !");
            return;
        }

        int index = Random.Range(0, poissonDatabase.poissons.Count);
        Poisson poissonChoisi = poissonDatabase.poissons[index];

        nomPoissonCible = poissonChoisi.nom;
        prefabPoissonCible = poissonChoisi.prefab;

        if (textMesh != null)
        {
            textMesh.text = $"{nomPoissonCible}";
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Vérifier que ce panier n'est pas déjà validé
        if (!estValide)
        {
            if (other.gameObject.name.Contains(prefabPoissonCible.name))
            {
                estValide = true;
                paniersValides++;
                Debug.Log($"Panier validé ! ({paniersValides}/{nombreTotalPaniers})");

                // Changer la couleur du texte pour indiquer la validation
                if (textMesh != null)
                {
                    textMesh.color = Color.green;
                }

                // Vérifier si tous les paniers sont validés
                VerifierTousPaniers();
            }
            else
            {
                Debug.Log("Mauvais poisson !");
            }
        }
    }

    private void VerifierTousPaniers()
    {
        if (paniersValides >= nombreTotalPaniers)
        {
            Debug.Log("Tous les paniers sont validés !");
            
            // Détruire l'objet spécifié
            if (objectToDestroy != null)
            {
                Destroy(objectToDestroy);
            }
            else
            {
                Debug.LogWarning("Aucun objet à détruire n'a été spécifié !");
            }
        }
    }

    // Méthode pour réinitialiser le système (utile pour recommencer le jeu)
    public static void ReinitialiserPaniers()
    {
        paniersValides = 0;
    }
}
