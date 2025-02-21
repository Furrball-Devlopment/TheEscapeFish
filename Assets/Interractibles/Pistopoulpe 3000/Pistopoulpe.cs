using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem; // Nécessaire pour utiliser le système d'entrée Input System

public class Pistopoulpe : MonoBehaviour
{
    [Header("Projectile Settings")]
    public GameObject projectilePrefab; // Le prefab du projectile
    public Transform firePoint;         // Point de tir des projectiles
    public float fireForce = 500f;      // Force appliquée au projectile

    [Header("Shooting Cooldown")]
    public float fireCooldown = 0.1f;   // Temps d'attente entre les tirs en secondes
    private float lastFireTime = 0f;    // Chronomètre pour gérer le délai

    [Header("Input System Settings")]
    public InputActionReference fireAction; // Référence vers l'action "Fire" définie dans Mes Actions
    public GameObject[] mainsjoueurs;

    private void Start()
    {
        // Activer l'action "Fire" pour capturer les inputs
        if (fireAction != null)
        {
            fireAction.action.Enable(); // Active la gestion de l'action dans le système Input System
        }
    }

    private void Update()
    {
        // Vérifier si l'action Fire (gâchette) est déclenchée
        if (fireAction != null && Vector3.Distance(mainsjoueurs[0].transform.position, transform.position) < 2f || Vector3.Distance(mainsjoueurs[1].transform.position, transform.position) < 2f)
        {
            float fireValue = fireAction.action.ReadValue<float>(); // Lire la valeur associée à Fire
            Debug.Log("Action Fire Value: " + fireValue); // Déboguer la valeur détectée

            // Détecter si la gâchette est pressée (valeur > 0.1)
            if (fireValue > 0.1f && Time.time >= lastFireTime + fireCooldown)
            {
                Debug.Log("Fire action déclenchée !");
                Fire(); // Appeler la méthode pour tirer le projectile
                lastFireTime = Time.time; // Mettre à jour le chronomètre après le tir
            }
        }
    }

    private void Fire()
    {
        // Vérifier que le projectilePrefab et le firePoint sont assignés
        if (projectilePrefab != null && firePoint != null)
        {
            Debug.Log("Création d'un projectile au FirePoint.");
            
            // Instancier le projectile à la position et orientation du FirePoint
            GameObject spawnedProjectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

            // Récupérer le Rigidbody du projectile et y appliquer une force
            Rigidbody rb = spawnedProjectile.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddForce(firePoint.forward * fireForce);
                Debug.Log("Force appliquée au projectile.");
            }
            else
            {
                Debug.LogWarning("Le prefab du projectile n'a pas de Rigidbody !");
            }
        }
        else
        {
            Debug.LogWarning("ProjectilePrefab ou FirePoint n'est pas assigné dans l'inspecteur !");
        }
    }
}
