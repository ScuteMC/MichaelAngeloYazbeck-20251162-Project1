using UnityEngine;
namespace Lanternwake
{
    public class BoostFlower : MonoBehaviour
    {
        [SerializeField] RoundController round;
        [SerializeField] MothPilot player;
        bool consumed;
        void OnTriggerEnter2D(Collider2D other) { if (other.CompareTag("Player")) Take(); }
        public bool Take()
        {
            if (consumed || !round.IsActive) return false;
            consumed = true; player.ActivateBoost(); round.Spark(transform.position);
            round.Announce("MOONBLOOM  /  SWIFT WINGS FOR 5 SECONDS");
            gameObject.SetActive(false); return true;
        }
        public void ResetPickup() { consumed = false; gameObject.SetActive(true); }
    }
}
