using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace Lanternwake
{
    public class GardenHud : MonoBehaviour
    {
        [SerializeField] RoundController round;
        [SerializeField] TMP_Text timeText, progressText, taskText, noticeText;
        [SerializeField] Image timeFill;
        [SerializeField] GameObject resultPanel;
        [SerializeField] TMP_Text resultTitle, resultBody;
        [SerializeField] RectTransform mapPlayer, mapPickup, mapShrine;
        void LateUpdate()
        {
            int seconds = Mathf.CeilToInt(Mathf.Max(0, round.Remaining));
            timeText.text = $"{seconds / 60:00}:{seconds % 60:00}";
            timeText.color = seconds <= 30 ? new Color(1, .55f, .42f) : new Color(1, .91f, .7f);
            progressText.text = $"{round.Completed:00} <color=#7D9F9E>/ {round.Goal:00}</color>";
            taskText.text = round.Delivery.Carrying ? "DELIVER STARLIGHT" : "GATHER STARLIGHT";
            noticeText.text = Time.time < round.NoticeUntil && round.IsActive ? round.Notice : "";
            timeFill.fillAmount = Mathf.Clamp01(round.Remaining / 180);
            SetMap(mapPlayer, round.Player.transform.position);
            SetMap(mapPickup, round.Routes.Pickup.transform.position);
            SetMap(mapShrine, round.Routes.Shrine.transform.position);
            mapPickup.gameObject.SetActive(!round.Delivery.Carrying);
            Vector3 objective = round.Delivery.Carrying ? round.Routes.Shrine.transform.position : round.Routes.Pickup.transform.position;
            resultPanel.SetActive(round.State != RoundState.Active);
            if (round.State == RoundState.Won)
            {
                resultTitle.text = "The garden awakens.";
                resultBody.text = $"All {round.Goal} lanterns are burning bright.\nYou brought the night back to life.\n\n{seconds} seconds to spare  /  {round.Completed} lights delivered";
            }
            else if (round.State == RoundState.Lost)
            {
                resultTitle.text = "A light for another night.";
                resultBody.text = $"Dawn arrived before the last lantern.\nThe garden will wait for you.\n\n{round.Completed} of {round.Goal} lights delivered";
            }
        }
        void SetMap(RectTransform marker, Vector3 position) { marker.anchoredPosition = new Vector2(position.x / 18 * 85, position.y / 14 * 60); }
    }
}
