using UnityEngine;

public class DestroyOnPoulpeCollision : MonoBehaviour
{
    // Cette méthode est appelée quand une collision se produit
    private void OnCollisionEnter(Collision collision)
    {
        // Vérifier si l'objet qui entre en collision possède le tag "poulpe"
        Debug.Log("Collision avec " + collision.gameObject.tag);
        if (collision.gameObject.CompareTag("Poulpe"))
        {
            // Détruire l'objet auquel ce script est attaché
            Destroy(gameObject);
        }
    }
}
