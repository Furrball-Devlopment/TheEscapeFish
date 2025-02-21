using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class ExplodingUrchin : MonoBehaviour
{
    public float explosionRadius = 5f;
    public float explosionForce = 700f;
    public GameObject explosionEffect;
    public float activationDelay = 1f; // Temps avant que l'oursin puisse exploser

    private XRGrabInteractable grabInteractable;
    private Rigidbody rb;
    public bool hasExploded = false;
    public bool canExplode = false; // Empêche l'explosion immédiate

    void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        rb = GetComponent<Rigidbody>();

        if (grabInteractable != null)
        {
            grabInteractable.selectExited.AddListener(OnRelease);
        }

        StartCoroutine(EnableExplosionAfterDelay()); // Lance la coroutine pour activer l'explosion après un délai
    }

    void OnRelease(SelectExitEventArgs args)
    {
        rb.isKinematic = false; // Active la physique au lâcher
    }

    private IEnumerator EnableExplosionAfterDelay()
    {
        yield return new WaitForSeconds(activationDelay);
        canExplode = true;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!hasExploded && canExplode && rb.linearVelocity.magnitude > 1f) // Vérifie si l'oursin est bien lancé
        {
            Explode();
        }
    }

    void Explode()
    {
        hasExploded = true;

        if (explosionEffect != null)
        {
            GameObject effect = Instantiate(explosionEffect, transform.position, Quaternion.identity);

            ParticleSystem ps = effect.GetComponent<ParticleSystem>();
            if (ps != null)
            {
                Destroy(effect, ps.main.duration - 0.1f);
            }
        }

        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider nearbyObject in colliders)
        {
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
            }
        }

        Destroy(gameObject); // Détruit l'oursin après explosion
    }
}
