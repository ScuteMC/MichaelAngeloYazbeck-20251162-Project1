using UnityEngine;
namespace Lanternwake
{
    public class ShrineTrigger : MonoBehaviour
    {
        [SerializeField] DeliveryRules delivery;
        void OnTriggerEnter2D(Collider2D other) { if (other.CompareTag("Player")) delivery.Deliver(); }
    }
}
