using UnityEngine;

public class PoissonDeplacement : MonoBehaviour
{
    [Header("Paramètres de déplacement")]
    public float vitesse = 2f;
    public float rotationVitesse = 50f;
    public float distanceMinimaleObjectif = 0.5f;

    [Header("Paramètres de délai")]
    public float delaiMinimum = 0.5f; // Délai minimum avant de changer de point
    public float delaiMaximum = 2f;   // Délai maximum avant de changer de point

    [Header("Limites de l'espace de déplacement")]
    public Vector3 dimensionsMin;
    public Vector3 dimensionsMax;

    private Vector3 pointCible;
    private bool besoinNouvelObjectif = true;
    private float delaiAvantChangement;
    private float compteurDelai;
    private bool enPause = false;

    private void Start()
    {
        GenererNouvelObjectif();
    }

    private void Update()
    {
        if (enPause)
        {
            // Mise à jour du compteur de délai
            compteurDelai += Time.deltaTime;
            if (compteurDelai >= delaiAvantChangement)
            {
                enPause = false;
                GenererNouvelObjectif();
            }
            return;
        }

        if (Vector3.Distance(transform.position, pointCible) < distanceMinimaleObjectif)
        {
            // Initialiser la pause
            enPause = true;
            compteurDelai = 0f;
            delaiAvantChangement = Random.Range(delaiMinimum, delaiMaximum);
        }
        else
        {
            DeplacerVersObjectif();
        }
    }

    private void DeplacerVersObjectif()
    {
        // Calculer la direction vers le point cible
        Vector3 direction = (pointCible - transform.position).normalized;

        // Déplacement progressif
        transform.position = Vector3.MoveTowards(
            transform.position,
            pointCible,
            vitesse * Time.deltaTime
        );

        // Rotation progressive
        if (direction != Vector3.zero)
        {
            Quaternion rotationCible = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                rotationCible,
                rotationVitesse * Time.deltaTime
            );
        }
    }

    private void GenererNouvelObjectif()
    {
        // Générer un nouveau point cible aléatoire dans les limites définies
        pointCible = new Vector3(
            Random.Range(dimensionsMin.x, dimensionsMax.x),
            Random.Range(dimensionsMin.y, dimensionsMax.y),
            Random.Range(dimensionsMin.z, dimensionsMax.z)
        );

        besoinNouvelObjectif = false;
    }

    private void OnDrawGizmos()
    {
        // Dessiner les limites
        Gizmos.color = Color.green;
        Vector3 centre = (dimensionsMin + dimensionsMax) / 2f;
        Vector3 dimensions = dimensionsMax - dimensionsMin;
        Gizmos.DrawWireCube(centre, dimensions);

        // Dessiner le point cible en mode jeu
        if (Application.isPlaying)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(pointCible, 0.2f);
        }
    }

    // Méthode publique pour forcer un changement de direction
    public void ChangerDirection()
    {
        enPause = false;
        GenererNouvelObjectif();
    }
}
