using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Managers
{
    public class SaveManager : MonoBehaviour
    {
        [SerializeField] private List<SaveStateID> saveData;

        internal void ResetData()
        {
            foreach (var save in saveData.Where(save => GameManager.Instance.activeStates.Contains(save)))
            {
                GameManager.Instance.activeStates.Remove(save);
            }
        }
    }
}