using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;



public class LivreVR : MonoBehaviour
{
    public PoissonDatabase database; // Référence à la base de données

    public TextMeshProUGUI texteGauche;
    public TextMeshProUGUI texteDroite;

    public Transform poissonGaucheParent;
    public Transform poissonDroiteParent;

    private GameObject poissonGaucheActuel;
    private GameObject poissonDroiteActuel;
    private int indexPage = 0;

    [Header("Input System Settings")]
    public InputActionReference pageGauche;
    public InputActionReference pageDroite;
    private float cooldown = 0.5f;
    private float cooldownTimer = 0f;
    public GameObject[] mainsjoueurs;


    void Start()
    {
        AfficherPages();
    }

    void Update()
    {
        // Mettre à jour le cooldown timer
        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
        }

        // Vérifier si une main du joueur est assez proche
        if (Vector3.Distance(mainsjoueurs[0].transform.position, transform.position) < 2f || Vector3.Distance(mainsjoueurs[1].transform.position, transform.position) < 2f)
        {
            // Vérifier si le cooldown est terminé
            if (cooldownTimer <= 0)
            {
                // Vérifier si l'action de la page gauche est déclenchée
                if (pageGauche != null)
                {
                    float pageGaucheValue = pageGauche.action.ReadValue<float>();
                    if (pageGaucheValue > 0.1f)
                    {
                        PagePrecedente();
                        cooldownTimer = cooldown; // Réinitialiser le cooldown
                    }
                }
                // Vérifier si l'action de la page droite est déclenchée
                if (pageDroite != null)
                {
                    float pageDroiteValue = pageDroite.action.ReadValue<float>();
                    if (pageDroiteValue > 0.1f)
                    {
                        PageSuivante();
                        cooldownTimer = cooldown; // Réinitialiser le cooldown
                    }
                }
            }
        }
    }



    public void PageSuivante()
    {
        if (indexPage < database.poissons.Count - 2)
        {
            indexPage += 2;
            AfficherPages();
        }
    }

    public void PagePrecedente()
    {
        if (indexPage > 0)
        {
            indexPage -= 2;
            AfficherPages();
        }
    }

    void AfficherPages()
    {
        if (indexPage < database.poissons.Count)
        {
            texteGauche.text = database.poissons[indexPage].nom;
        }

        if (indexPage + 1 < database.poissons.Count)
        {
            texteDroite.text = database.poissons[indexPage + 1].nom;
        }

        // Supprimer les anciens poissons
        if (poissonGaucheActuel) Destroy(poissonGaucheActuel);
        if (poissonDroiteActuel) Destroy(poissonDroiteActuel);

        // Instancier les nouveaux poissons
        if (indexPage < database.poissons.Count)
        {
            poissonGaucheActuel = Instantiate(database.poissons[indexPage].prefab, poissonGaucheParent);
            poissonGaucheActuel.transform.localPosition = Vector3.zero;
            poissonGaucheActuel.transform.localRotation = Quaternion.Euler(0, 180, 0);
            poissonGaucheActuel.transform.localScale = Vector3.one * 0.2f;

            //Enlever le capsule collider
            Destroy(poissonGaucheActuel.GetComponent<CapsuleCollider>());

            //Enlever le rigidbody
            Destroy(poissonGaucheActuel.GetComponent<Rigidbody>());

            //Enlever le xr grab interactor
            Destroy(poissonGaucheActuel.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>());

        }

        if (indexPage + 1 < database.poissons.Count)
        {
            poissonDroiteActuel = Instantiate(database.poissons[indexPage + 1].prefab, poissonDroiteParent);
            poissonDroiteActuel.transform.localPosition = Vector3.zero;
            poissonDroiteActuel.transform.localRotation = Quaternion.Euler(0, 180, 0);
            poissonDroiteActuel.transform.localScale = Vector3.one * 0.2f; 

            //Enlever le capsule collider
            Destroy(poissonDroiteActuel.GetComponent<CapsuleCollider>());

            //Enlever le rigidbody
            Destroy(poissonDroiteActuel.GetComponent<Rigidbody>());

            //Enlever le xr grab interactor
            Destroy(poissonDroiteActuel.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>());
        }
    }
}
