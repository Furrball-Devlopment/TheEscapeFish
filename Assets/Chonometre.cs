using UnityEngine;

public class Chronometre : MonoBehaviour
{
    public static Chronometre Instance { get; private set; } // Singleton pour accès global

    private float tempsEcoule = 0f; // Temps écoulé en secondes
    private bool estActif = true;  // Le chronomètre démarre immédiatement

    private void Awake()
    {
        // Si une autre instance existe déjà, ne créez pas un nouveau chronomètre
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // Configure cette instance comme "unique" et la rend persistante entre les scènes
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        // Le chronomètre commence dès le début du jeu
        ResetChronometre();
        DemarrerChronometre();
    }

    private void Update()
    {
        // Si le chronomètre est actif, mettre à jour le temps écoulé
        if (estActif)
        {
            tempsEcoule += Time.deltaTime; // Ajoute le deltaTime (temps écoulé entre chaque frame)
        }
    }

    /// <summary>
    /// Démarre le chronomètre.
    /// </summary>
    public void DemarrerChronometre()
    {
        estActif = true;
    }

    /// <summary>
    /// Stoppe le chronomètre.
    /// </summary>
    public void StopperChronometre()
    {
        estActif = false;
    }

    /// <summary>
    /// Réinitialise le chronomètre à 0 sans le démarrer.
    /// </summary>
    public void ResetChronometre()
    {
        tempsEcoule = 0f;
        estActif = false;
    }

    /// <summary>
    /// Retourne le temps écoulé en secondes.
    /// </summary>
    public float ObtenirTempsEcoule()
    {
        return tempsEcoule;
    }

    /// <summary>
    /// Retourne le temps écoulé formaté en minutes:secondes (MM:SS).
    /// </summary>
    public string ObtenirTempsFormate()
    {
        int minutes = Mathf.FloorToInt(tempsEcoule / 60);
        int secondes = Mathf.FloorToInt(tempsEcoule % 60);
        return string.Format("{0:00}:{1:00}", minutes, secondes);
    }
}
