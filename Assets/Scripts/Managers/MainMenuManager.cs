using Helpers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class MainMenuManager : MonoBehaviour
    {
        [SerializeField] private SceneReference firstRoom;
        [SerializeField] private SceneReference coreScene;
        public void LoadFirstScene()
        {
            SceneManager.LoadScene(firstRoom.ScenePath);
            SceneManager.LoadSceneAsync(coreScene.ScenePath, LoadSceneMode.Additive);
        }
    }
}