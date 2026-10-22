using UnityEngine;
using UnityEngine.SceneManagement;
namespace Lanternwake
{
    public class MenuController : MonoBehaviour
    {
        public void Play() { SceneManager.LoadScene("MoonGarden"); }
    }
}
