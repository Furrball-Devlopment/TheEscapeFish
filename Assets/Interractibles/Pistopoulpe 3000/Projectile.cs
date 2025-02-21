using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Settings")]
    public float speed = 20f;          // Vitesse du projectile
    public float lifespan = 5f;       // Durée de vie du projectile
    public float growthDuration = 1f; // Temps pour que le projectile atteigne sa taille maximale

    private Vector3 initialScale = Vector3.zero;    // Échelle initiale (0)
    private Vector3 finalScale = Vector3.one;       // Échelle finale (1)
    private float spawnTime;                        // Temps où le projectile a été créé

    void Start()
    {
        // Assurez-vous que le projectile commence avec une échelle de 0
        transform.localScale = initialScale;

        // Enregistrer l'heure à laquelle le projectile a été créé
        spawnTime = Time.time;

        // Détruire le projectile après sa durée de vie
        Destroy(gameObject, lifespan);
    }

    void Update()
    {
        // Calculer le temps écoulé depuis la création du projectile
        float elapsedTime = Time.time - spawnTime;

        // Croissance progressive (Lerp entre l'échelle initiale et finale)
        if (elapsedTime < growthDuration)
        {
            float progress = elapsedTime / growthDuration; // Valeur entre 0 et 1
            transform.localScale = Vector3.Lerp(initialScale, finalScale, progress);
        }
        else
        {
            transform.localScale = finalScale; // S'assurer que l'échelle reste à 1 après la croissance
        }

        // Déplacement du projectile (avance en ligne droite selon sa vitesse)
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
}
