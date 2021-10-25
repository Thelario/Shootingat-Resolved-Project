using UnityEngine;
using UnityEngine.UI;
using TMPro;
using PabloLario.Managers;

namespace PabloLario.UI
{
    public class ButtonTrigger : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Image buttonImage;
        [SerializeField] private TMP_Text buttonText;

        [Header("Fields")]
        [Header("Image")]
        [SerializeField] private Color defaultImageColor;
        [SerializeField] private Color selectedImageColor;
        [Header("Text")]
        [SerializeField] private Color defaultTextColor;
        [SerializeField] private Color selectedTextColor;

        private bool checkInput = false;

        private Popup popup;

        private void Awake()
        {
            popup = buttonImage.GetComponent<Popup>();
        }

        private void Update()
        {
            if (!checkInput)
                return;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                buttonImage.GetComponent<ISelectable>().SelectButton();
                SoundManager.Instance.PlaySound(SoundType.Blop, 1f);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                buttonImage.color = selectedImageColor;
                buttonText.color = selectedTextColor;
                checkInput = true;
                popup.AnimatePopup();
                SoundManager.Instance.PlaySound(SoundType.Blop, 1f);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                buttonImage.color = defaultImageColor;
                buttonText.color = defaultTextColor;
                checkInput = false;
            }
        }
    }
}
