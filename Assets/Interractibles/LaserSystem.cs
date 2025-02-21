using UnityEngine;

public class LaserSystemSpecial : MonoBehaviour
{
    [Header("Objets définissant le laser")]
    [Tooltip("Objet définissant le point de départ du laser.")]
    public GameObject pointObjectA;

    [Tooltip("Objet définissant le point d'arrivée du laser.")]
    public GameObject pointObjectB;

    [Header("Objet spécial")]
    [Tooltip("Objet spécial à détecter lors de la collision du laser.")]
    public GameObject specialObject;

    [Header("Paramètres du laser")]
    public float laserWidth = 0.1f;
    public Color laserColor = Color.red;
    public GameObject objetVisible;

    [Header("Détection")]
    [Tooltip("Layers à tester pour la détection de collision du laser.")]
    public LayerMask collisionMask;

    private LineRenderer lineRenderer;

    void Start()
    {
        // Création et configuration du LineRenderer pour afficher le laser
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.startWidth = laserWidth;
        lineRenderer.endWidth = laserWidth;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = laserColor;
        lineRenderer.endColor = laserColor;

        //masquer l'objet visible
        objetVisible.SetActive(false);
    }

    void Update()
    {
        // Vérifie que les deux objets définissant les positions du laser sont assignés
        if (pointObjectA == null || pointObjectB == null)
            return;

        // Position de départ et d'arrivée définies par les objets
        Vector3 startPosition = pointObjectA.transform.position;
        Vector3 endPosition = pointObjectB.transform.position;
        Vector3 direction = (endPosition - startPosition).normalized;
        float distance = Vector3.Distance(startPosition, endPosition);

        // Raycast entre les deux points
        if (Physics.Raycast(startPosition, direction, out RaycastHit hitInfo, distance, collisionMask))
        {
            // Si le laser est coupé par quelque chose, on vérifie si c'est l'objet spécial
            if (specialObject != null && hitInfo.collider.gameObject == specialObject)
            {
                Debug.Log("Laser a collisionné avec l'objet spécial : " + hitInfo.collider.name);
                Chronometre.Instance.StopperChronometre();
                objetVisible.SetActive(true);
                
            }
            else
            {
                Debug.Log("Laser coupé par : " + hitInfo.collider.name);
            }

            // Le laser s'arrête au point de collision
            lineRenderer.SetPosition(0, startPosition);
            lineRenderer.SetPosition(1, hitInfo.point);
        }
        else
        {
            // Aucun obstacle détecté, le laser est dessiné de A à B
            lineRenderer.SetPosition(0, startPosition);
            lineRenderer.SetPosition(1, endPosition);
        }
    }
}
