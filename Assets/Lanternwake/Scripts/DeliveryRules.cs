using UnityEngine;

namespace Lanternwake
{
    public class DeliveryRules : MonoBehaviour
    {
        [SerializeField] RoundController round;
        [SerializeField] MothPilot player;
        [SerializeField] RoutePlanner routes;
        public bool Carrying { get; private set; }
        public bool Collect(GameObject pickup)
        {
            if (!round.IsActive || Carrying || pickup != routes.Pickup.gameObject || !pickup.activeSelf) return false;
            Carrying = true; player.ShowCargo(true); pickup.SetActive(false);
            round.Spark(player.transform.position); round.Announce("STARLIGHT GATHERED  /  FIND THE AMBER SHRINE");
            return true;
        }
        public bool Deliver()
        {
            if (!round.IsActive || !Carrying) return false;
            // Consume before counting. A second callback cannot deliver the same light.
            Carrying = false; player.ShowCargo(false);
            round.CountDelivery();
            if (round.IsActive) { round.Spark(player.transform.position); routes.Relocate(); }
            return true;
        }
        public void ResetCargo() { Carrying = false; player.ShowCargo(false); }
    }
}
