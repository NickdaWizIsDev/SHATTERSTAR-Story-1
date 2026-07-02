using System;
using Helpers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace General_Use
{
    public class CoolDeactivator : MonoBehaviour
    {
        [SerializeField] private SceneReference targetScene;
        [SerializeField] private GameObject targetObj;

        private void Update()
        {
            if (SceneManager.GetActiveScene().path == targetScene.ScenePath && targetObj.activeInHierarchy)
            {
                targetObj.SetActive(false);
            }
            else if(SceneManager.GetActiveScene().path != targetScene.ScenePath && !targetObj.activeInHierarchy)
            {
                targetObj.SetActive(true);
            }
        }
    }
}