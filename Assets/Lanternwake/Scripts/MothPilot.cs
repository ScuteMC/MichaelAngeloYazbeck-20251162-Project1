using UnityEngine;
using UnityEngine.InputSystem;

namespace Lanternwake
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class MothPilot : MonoBehaviour
    {
        [SerializeField] RoundController round;
        [SerializeField] InputActionReference move;
        [SerializeField] Rigidbody2D body;
        [SerializeField] Transform wings;
        [SerializeField] SpriteRenderer artwork;
        [SerializeField] GameObject carriedLight;
        [SerializeField, Min(0.1f)] float normalSpeed = 6;
        [SerializeField, Min(0.1f)] float boostedSpeed = 10;
        [SerializeField, Min(0.1f)] float boostDuration = 5;
        Vector2 direction;
        float boostRemaining;
        public float BoostRemaining => boostRemaining;
        public float CurrentSpeed => boostRemaining > 0 ? boostedSpeed : normalSpeed;
        public Rigidbody2D Body => body;
        void OnEnable() { if (move != null) move.action.Enable(); }
        void OnDisable() { if (move != null) move.action.Disable(); }
        void Update()
        {
            // Expiry belongs to time, not movement: waiting still spends a boost.
            boostRemaining = Mathf.Max(0, boostRemaining - Time.deltaTime);
            direction = round.IsActive ? Vector2.ClampMagnitude(move.action.ReadValue<Vector2>(), 1) : Vector2.zero;
            if (round.IsActive)
            {
                wings.localScale = new Vector3(1 + Mathf.Sin(Time.time * 15) * .055f, 1, 1);
                if (direction.sqrMagnitude > .01f)
                    wings.rotation = Quaternion.Slerp(wings.rotation, Quaternion.Euler(0, 0, Mathf.Atan2(-direction.x, direction.y) * Mathf.Rad2Deg), 14 * Time.deltaTime);
            }
            artwork.color = boostRemaining > 0 ? new Color(.65f, 1, .95f) : Color.white;
        }
        void FixedUpdate() { body.linearVelocity = round.IsActive ? direction * CurrentSpeed : Vector2.zero; }
        public void ActivateBoost() { if (round.IsActive) boostRemaining = boostDuration; }
        public void CancelBoost() { boostRemaining = 0; }
        public void ShowCargo(bool visible) { carriedLight.SetActive(visible); }
        public void Stop() { direction = Vector2.zero; body.linearVelocity = Vector2.zero; CancelBoost(); }
        public void ResetPilot(Vector2 position)
        {
            Stop(); body.position = position; body.rotation = 0;
            transform.position = position; wings.rotation = Quaternion.identity;
            wings.localScale = Vector3.one; artwork.color = Color.white; ShowCargo(false);
        }
    }
}
