using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class WindZone3D : MonoBehaviour
{
    [Header("Parametry wiatru")]
    [Tooltip("Sila pionowa unoszaca kule")]
    [SerializeField] private float upwardForce = 25f;

    [Tooltip("Maksymalna predkosc pionowa")]
    [SerializeField] private float maxUpwardVelocity = 7f;

    [Header("Filtr kulki")]
    [Tooltip("Maksymalna masa do uniesienia (papier = 0.3, drewno = 1.0, kamien = 3.0)")]
    [SerializeField] private float maxLiftableMass = 0.5f;

    private void Awake()
    {
        var col = GetComponent<BoxCollider>();
        col.isTrigger = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        Rigidbody rb = other.attachedRigidbody;
        if (rb != null)
        {
            if (rb.mass > maxLiftableMass) return;

            if (rb.linearVelocity.y < maxUpwardVelocity)
            {
                rb.AddForce(Vector3.up * upwardForce, ForceMode.Acceleration);
            }
        }
    }
}