using UnityEngine;
using System.Collections;

public class SpawnPoisson : MonoBehaviour
{
    [Header("Paramètres de spawn")]
    public PoissonDatabase poissonDatabase;    // La base de données de poissons
    public float delaiEntreSpawns = 2f;        // Délai entre chaque spawn

    private void Start()
    {
        // Vérifier si la base de données est assignée et non vide
        if (poissonDatabase == null || poissonDatabase.poissons.Count == 0)
        {
            Debug.LogError("La base de données de poissons est vide ou non assignée.");
            return;
        }

        // Démarrer la coroutine de spawn
        StartCoroutine(SpawnerPoissons());
    }

    private IEnumerator SpawnerPoissons()
    {
        while (true) // Boucle infinie
        {
        
            yield return new WaitForSeconds(delaiEntreSpawns);

            CreerPoisson();
        }
    }

    private void CreerPoisson()
    {
        // Sélectionner un poisson aléatoire depuis la base de données
        int index = Random.Range(0, poissonDatabase.poissons.Count);
        Poisson poissonChoisi = poissonDatabase.poissons[index];

        // Vérifier si le prefab existe
        if (poissonChoisi.prefab != null)
        {
            // Spawner le poisson à la position du spawner
            GameObject nouveauPoisson = Instantiate(poissonChoisi.prefab, transform.position, Quaternion.identity);
            
        }
        else
        {
            Debug.LogWarning($"Le prefab pour le poisson {poissonChoisi.nom} est manquant.");
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 0.3f);
    }
}

