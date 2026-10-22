using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Lanternwake
{
    public enum RoundState { Active, Won, Lost }
    public class RoundController : MonoBehaviour
    {
        [SerializeField, Min(1)] float startingTime = 180;
        [SerializeField, Min(10)] int deliveryGoal = 10;
        [SerializeField, Min(0)] float deliveryBonus = 5;
        [SerializeField] MothPilot player;
        [SerializeField] DeliveryRules delivery;
        [SerializeField] RoutePlanner routes;
        [SerializeField] BoostFlower[] boosts;
        [SerializeField] ParticleSystem feedback;
        [SerializeField] FollowCamera followCamera;
        [SerializeField] InputActionReference restart;
        [SerializeField] InputActionReference menu;
        [SerializeField] Vector2 startPosition = new Vector2(0, -10);
        public float Remaining { get; private set; }
        public int Completed { get; private set; }
        public int Goal => deliveryGoal;
        public RoundState State { get; private set; }
        public bool IsActive => State == RoundState.Active && Remaining > 0;
        public MothPilot Player => player;
        public DeliveryRules Delivery => delivery;
        public RoutePlanner Routes => routes;
        public string Notice { get; private set; }
        public float NoticeUntil { get; private set; }
        void OnEnable() { restart.action.Enable(); menu.action.Enable(); }
        void OnDisable() { restart.action.Disable(); menu.action.Disable(); }
        void Start() { ResetRound(); }
        void Update()
        {
            if (State == RoundState.Active) Tick(Time.deltaTime);
            if (State != RoundState.Active && restart.action.WasPressedThisFrame()) ResetRound();
            if (menu.action.WasPressedThisFrame()) SceneManager.LoadScene("MainMenu");
        }
        public void Tick(float delta)
        {
            if (State != RoundState.Active) return;
            Remaining = Mathf.Max(0, Remaining - Mathf.Max(0, delta));
            if (Remaining <= 0) Finish(RoundState.Lost);
        }
        public bool CountDelivery()
        {
            if (!IsActive) return false;
            Completed++;
            player.CancelBoost();
            if (Completed >= deliveryGoal) Finish(RoundState.Won);
            else { Remaining += deliveryBonus; Announce("LANTERN LIT  /  +5 SECONDS"); }
            return true;
        }
        void Finish(RoundState result)
        {
            State = result; player.Stop();
            feedback.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
        public void ResetRound()
        {
            Remaining = startingTime; Completed = 0; State = RoundState.Active;
            player.ResetPilot(startPosition); delivery.ResetCargo();
            feedback.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            foreach (var boost in boosts) boost.ResetPickup();
            Physics2D.SyncTransforms(); routes.ResetRoutes(); followCamera.Snap();
            Announce("THE GARDEN IS WAITING FOR YOUR LIGHT");
        }
        public void Announce(string message) { Notice = message; NoticeUntil = Time.time + 2.6f; }
        public void Spark(Vector3 position)
        {
            feedback.transform.position = position;
            feedback.Emit(22);
        }
    }
}
