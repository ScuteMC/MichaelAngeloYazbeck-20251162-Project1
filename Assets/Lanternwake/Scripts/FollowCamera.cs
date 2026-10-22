using UnityEngine;
namespace Lanternwake
{
    public class FollowCamera : MonoBehaviour
    {
        [SerializeField] Transform target;
        [SerializeField] Camera view;
        [SerializeField] Vector2 worldHalfSize = new Vector2(18, 14);
        [SerializeField] float smoothTime = .16f;
        Vector3 velocity;
        Vector3 Desired()
        {
            float x = Mathf.Max(0, worldHalfSize.x - view.orthographicSize * view.aspect);
            float y = Mathf.Max(0, worldHalfSize.y - view.orthographicSize);
            return new Vector3(Mathf.Clamp(target.position.x, -x, x), Mathf.Clamp(target.position.y, -y, y), -10);
        }
        void LateUpdate() { transform.position = Vector3.SmoothDamp(transform.position, Desired(), ref velocity, smoothTime); }
        public void Snap() { transform.position = Desired(); velocity = Vector3.zero; }
    }
}
