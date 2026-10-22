using System.Collections.Generic;
using UnityEngine;

namespace Lanternwake
{
    public class RoutePlanner : MonoBehaviour
    {
        [SerializeField] Transform[] markers;
        [SerializeField] Transform player;
        [SerializeField] PickupTrigger pickup;
        [SerializeField] ShrineTrigger shrine;
        [SerializeField] LayerMask solidMask;
        [SerializeField] float pairSeparation = 8;
        [SerializeField] float playerSeparation = 3;
        [SerializeField] float clearanceRadius = 1;
        int previousPickup = -1, previousShrine = -1;
        public PickupTrigger Pickup => pickup;
        public ShrineTrigger Shrine => shrine;
        public Transform[] Markers => markers;
        public int PickupIndex => previousPickup;
        public int ShrineIndex => previousShrine;
        public int Generation { get; private set; }
        public void ResetRoutes() { previousPickup = previousShrine = -1; Generation = 0; Relocate(); }
        public void Relocate()
        {
            // Enumerate valid ordered pairs; never relax safety when a random attempt fails.
            var valid = new List<Vector2Int>();
            for (int a = 0; a < markers.Length; a++)
            {
                if (!Safe(a)) continue;
                for (int b = 0; b < markers.Length; b++)
                    if (a != b && Safe(b) && Vector2.Distance(markers[a].position, markers[b].position) >= pairSeparation)
                        valid.Add(new Vector2Int(a, b));
            }
            if (valid.Count == 0) throw new System.InvalidOperationException("No safe route pairs. Check Inspector markers and clearance.");
            Vector2Int pair = valid[Random.Range(0, valid.Count)];
            previousPickup = pair.x; previousShrine = pair.y; Generation++;
            pickup.transform.position = markers[pair.x].position;
            shrine.transform.position = markers[pair.y].position;
            pickup.gameObject.SetActive(true); shrine.gameObject.SetActive(true);
        }
        bool Safe(int index)
        {
            if (index == previousPickup || index == previousShrine) return false;
            Vector2 p = markers[index].position;
            return Vector2.Distance(p, player.position) >= playerSeparation && !Physics2D.OverlapCircle(p, clearanceRadius, solidMask);
        }
    }
}
