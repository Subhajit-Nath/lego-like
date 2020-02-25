using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    [SerializeField] private ColorPicker picker;
    [SerializeField] private Transform[] selectionItems;
    [SerializeField] private Transform highlightTransform;
    [SerializeField] private Image selectedColorImage;
    [SerializeField] private GameObject quitMenu;

    private void Awake()
    {
        instance = this;
    }

    public void ToggleColorPicker()
    {
        if (picker.gameObject.activeSelf)
        {
            picker.gameObject.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1f;
        }
        else
        {
            picker.gameObject.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0f;
        }
    }

    public void UpdateSelectionUI(int index)
    {
        highlightTransform.position = selectionItems[index].position;
    }

    public void SetSelectedColorSample(Color color)
    {
        selectedColorImage.color = color;
    }

    public void ShowQuitMenu()
    {
        if (quitMenu.activeSelf)
        {
            CancelQuit();
        }
        else
        {
            Time.timeScale = 0;
            quitMenu.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void CancelQuit()
    {
        Time.timeScale = 1;
        quitMenu.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }
}
