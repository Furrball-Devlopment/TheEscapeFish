using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoissonSpawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    public PoissonDatabase poissonDatabase; // La base de données de poissons
    public Transform spawnZone; // Une zone définie avec un Collider où les poissons spawneront
    public int maxPoissons = 10; // Limite maximale de poissons en même temps
    public float spawnDelay = 3f; // Temps entre chaque spawn
    public int fishNumberAtSpawn = 100;

    private List<GameObject> poissonsActuels = new List<GameObject>(); // Liste des poissons actuellement dans la zone
    private IEnumerator spawnRoutine; // Routine de spawn

    private Collider spawnZoneCollider; // Référence au collider de la zone

    private void Start()
    {
        // Vérifiez si la base de données et la zone sont correctement configurées
        if (poissonDatabase == null || poissonDatabase.poissons.Count == 0)
        {
            Debug.LogError("La base de données de poissons est vide ou non assignée.");
            return;
        }

        if (spawnZone == null)
        {
            Debug.LogError("La zone de spawn n'est pas assignée.");
            return;
        }

        // Récupérer le Collider de la zone de spawn
        spawnZoneCollider = spawnZone.GetComponent<Collider>();
        if (spawnZoneCollider == null)
        {
            Debug.LogError("La zone de spawn doit avoir un Collider !");
            return;
        }

        // Générer au démarrage une quantité fixe de poissons (100 poissons ici)
        GenererPoissonsInitiaux(fishNumberAtSpawn);

        // Lancer la coroutine pour le spawn des poissons
        spawnRoutine = SpawnPoissons();
        StartCoroutine(spawnRoutine);
    }

    private void Update()
    {
        // Nettoyer la liste des objets pour supprimer les poissons détruits
        poissonsActuels.RemoveAll(poisson => poisson == null);
    }

    private IEnumerator SpawnPoissons()
    {
        while (true)
        {
            // Vérifiez si on a atteint la limite maximale de poissons
            if (poissonsActuels.Count < maxPoissons)
            {
                // Spawn d'un poisson aléatoire
                SpawnPoissonAleatoire();
            }

            // Attendre avant le prochain spawn
            yield return new WaitForSeconds(spawnDelay);
        }
    }

    private void GenererPoissonsInitiaux(int nombreDePoissons)
    {
        for (int i = 0; i < nombreDePoissons; i++)
        {
            // Vérifier que la limite des poissons n'est pas dépassée
            if (poissonsActuels.Count >= maxPoissons)
            {
                Debug.Log("Max poissons atteint lors de l'initialisation.");
                break;
            }

            // Générer un poisson aléatoire
            SpawnPoissonAleatoire();
        }
    }

    private void SpawnPoissonAleatoire()
    {
        // Sélectionner un poisson aléatoire depuis la base de données
        int index = Random.Range(0, poissonDatabase.poissons.Count);
        Poisson poissonChoisi = poissonDatabase.poissons[index];

        // Générer une position aléatoire dans la zone de spawn
        Vector3 spawnPosition = ObtenirPositionAleatoireDansZone();

        // Générer une rotation aléatoire
        Quaternion spawnRotation = Quaternion.Euler(
            0f, // Garder l'axe X fixe (par exemple, pour des poissons horizontaux)
            Random.Range(0f, 360f), // Rotation aléatoire sur l'axe Y
            0f  // Garder l'axe Z fixe
        );

        // Instancier le prefab du poisson
        if (poissonChoisi.prefab != null)
        {
            GameObject poissonInstance = Instantiate(poissonChoisi.prefab, spawnPosition, spawnRotation);
            poissonsActuels.Add(poissonInstance); // Ajouter à la liste locale
        }
        else
        {
            Debug.LogWarning($"Le prefab pour le poisson {poissonChoisi.nom} est manquant.");
        }
    }

    private Vector3 ObtenirPositionAleatoireDansZone()
    {
        // Calculer des coordonnées aléatoires dans la zone de spawn
        Bounds bounds = spawnZoneCollider.bounds;
        float posX = Random.Range(bounds.min.x, bounds.max.x);
        float posY = Random.Range(bounds.min.y, bounds.max.y);
        float posZ = Random.Range(bounds.min.z, bounds.max.z);

        return new Vector3(posX, posY, posZ);
    }

    public void PoissonSupprime(GameObject poisson)
    {
        // Permet de retirer un poisson manuellement de la liste (utile si vous supprimez le poisson via un autre script)
        if (poissonsActuels.Contains(poisson))
        {
            poissonsActuels.Remove(poisson);
        }
    }
}
