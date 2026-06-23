using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// This forces Unity to make sure a native Button and Image exist on this object
namespace UI
{
    public class UIButtonVisuals : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
    {
        private Material mat;
        private Image targetImage;
        private Button nativeButton;

        [Header("Shader Settings")]
        [SerializeField] private Color idleColor = Color.white;
        [SerializeField] private Color hoverColor = Color.white;
        [SerializeField] private Color clickColor = Color.gray;

        private string colorRef = "_EmissionColor";
        private string strengthRef = "_EmissionStrength";

        private void Awake()
        {
            targetImage = GetComponent<Image>();
            nativeButton = GetComponent<Button>();

            mat = Instantiate(targetImage.material);
            targetImage.material = mat;
        }

        private void Start()
        {
            ResetVisuals();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!nativeButton.interactable) return;
            mat.SetColor(colorRef, hoverColor);
            mat.SetFloat(strengthRef, 1f);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!nativeButton.interactable) return;
            ResetVisuals();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (!nativeButton.interactable) return;
            mat.SetColor(colorRef, clickColor);
            mat.SetFloat(strengthRef, 3f);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (!nativeButton.interactable) return;
            mat.SetColor(colorRef, hoverColor);
            mat.SetFloat(strengthRef, 1f); 
        }

        private void ResetVisuals()
        {
            mat.SetColor(colorRef, idleColor);
            mat.SetFloat(strengthRef, 0f);
        }
    }
}