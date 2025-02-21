using UnityEngine;
using TMPro; // Nécessaire pour utiliser TextMeshPro

public class AffichageChronometre3D : MonoBehaviour
{
    private TextMeshPro textMeshProChronometre; // Référence au composant TextMeshPro

    private void Start()
    {
        // Récupérer le composant TextMeshPro attaché à l'objet
        textMeshProChronometre = GetComponent<TextMeshPro>(); 
        if (textMeshProChronometre == null)
        {
            Debug.LogError("TextMeshPro non trouvé sur cet objet !");
        }
    }

    private void Update()
    {
        // Vérifiez que le chronomètre existe avant de mettre à jour le texte
        if (Chronometre.Instance != null && textMeshProChronometre != null)
        {
            // Mise à jour du texte basé sur le temps formaté
            textMeshProChronometre.text = Chronometre.Instance.ObtenirTempsFormate();
        }
    }
}
