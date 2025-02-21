using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Receptacle : MonoBehaviour
{
    [Header("Database")]
    public PoissonDatabase poissonDatabase; // La base de données fournie avec vos poissons

    [Header("References")]
    public TextMeshPro textMesh; // TextMesh Pro classique pour afficher le nom
    public GameObject porte; // La porte ou objet à supprimer lorsque le bon poisson est déposé

    private string nomPoissonCible; // Le nom du poisson cible
    private GameObject prefabPoissonCible; // Le prefab correspondant au poisson cible

    private void Start()
    {
        // Sélectionner aléatoirement un poisson
        ChoisirPoissonAleatoire();
    }

    private void ChoisirPoissonAleatoire()
    {
        // Vérifiez que la base de données contient des poissons
        if (poissonDatabase.poissons == null || poissonDatabase.poissons.Count == 0)
        {
            Debug.LogError("La base de données de poissons est vide ou non assignée !");
            return;
        }

        // Choisir un poisson aléatoire
        int index = Random.Range(0, poissonDatabase.poissons.Count);
        Poisson poissonChoisi = poissonDatabase.poissons[index];

        // Définir le poisson cible
        nomPoissonCible = poissonChoisi.nom;
        prefabPoissonCible = poissonChoisi.prefab;

        // Affichage du nom du poisson dans TextMesh Pro
        if (textMesh != null)
        {
            textMesh.text = $"{nomPoissonCible}";
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Vérifie si l'objet déposé est celui attendu
        if (other.gameObject.name.Contains(prefabPoissonCible.name))
        {
            Debug.Log($"Le bon objet ({nomPoissonCible}) a été déposé !");
            
            // Supprime la porte
            if (porte != null)
            {
                Destroy(porte);
            }
        }
        else
        {
            Debug.Log("Mauvais objet déposé !");
        }
    }

    private void OnCollisionEnter(Collision collision)
{
    Debug.Log("Collision détectée avec : " + collision.gameObject.name);
}

}
