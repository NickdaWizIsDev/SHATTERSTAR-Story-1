using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public static class Bootstrapper
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Execute()
        {
            if(SceneManager.GetActiveScene().name == "Main Menu") return;
            var coreSceneName = "CoreScene"; 

            var isCoreLoaded = false;
            for (var i = 0; i < SceneManager.sceneCount; i++)
            {
                if (SceneManager.GetSceneAt(i).name == coreSceneName)
                {
                    isCoreLoaded = true;
                    break;
                }
            }

            if (!isCoreLoaded)
            {
                SceneManager.LoadScene(coreSceneName, LoadSceneMode.Additive);
            }
        }
    }
}