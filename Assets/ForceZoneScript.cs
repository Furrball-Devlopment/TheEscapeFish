using UnityEngine;

public class ForceZoneScript : MonoBehaviour
{
    [Header("Paramètres de la force")]
    [Tooltip("Force à appliquer dans la direction Z négative")]
    public float forceMagnitude = 10f;

    [Tooltip("Appliquer la force en continu tant que l'objet est dans la zone")]
    public bool forceContinue = true;

    [Tooltip("Appliquer la force uniquement à l'entrée dans la zone")]
    public bool forceImpulsion = false;

    private void OnTriggerStay(Collider other)
    {
        // Vérifier si l'objet a un Rigidbody et si la force continue est activée
        if (forceContinue && other.attachedRigidbody != null)
        {
            // Appliquer une force continue
            other.attachedRigidbody.AddForce(Vector3.back * forceMagnitude, ForceMode.Force);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Vérifier si l'objet a un Rigidbody et si la force par impulsion est activée
        if (forceImpulsion && other.attachedRigidbody != null)
        {
            // Appliquer une force d'impulsion
            other.attachedRigidbody.AddForce(Vector3.back * forceMagnitude, ForceMode.Impulse);
        }
    }

    // Visualisation de la zone dans l'éditeur
    private void OnDrawGizmos()
    {
        // Obtenir le Collider attaché
        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            // Dessiner les Gizmos
            Gizmos.color = new Color(0, 0, 1, 0.3f); // Bleu semi-transparent
            Gizmos.matrix = transform.localToWorldMatrix;
            
            if (col is BoxCollider)
            {
                BoxCollider boxCol = col as BoxCollider;
                Gizmos.DrawCube(boxCol.center, boxCol.size);
            }
            else if (col is SphereCollider)
            {
                SphereCollider sphereCol = col as SphereCollider;
                Gizmos.DrawSphere(sphereCol.center, sphereCol.radius);
            }

            // Dessiner une flèche indiquant la direction de la force
            Gizmos.color = Color.red;
            Vector3 center = col.bounds.center;
            Vector3 direction = Vector3.back * forceMagnitude * 0.1f; // Échelle de la flèche
            Gizmos.DrawRay(center, direction);
        }
    }
}
