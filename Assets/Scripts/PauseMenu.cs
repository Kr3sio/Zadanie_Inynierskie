using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [Header("Input System")]
    [SerializeField] private PlayerInput playerInput;

    [Header("Główny kontener")]
    [SerializeField] private GameObject pauseMenuContainer;

    [Header("Zakładki")]
    [SerializeField] private GameObject[] tabPanels;
    [SerializeField] private Button[] tabButtons;
    [SerializeField] private GameObject[] firstSelected;

    [Header("Kolory zakładek")]
    [SerializeField] private Color activeTabColor = Color.white;
    [SerializeField] private Color inactiveTabColor = new Color(0.6f, 0.6f, 0.6f, 1f);

    private InputAction pauseAction;
    private InputAction cancelAction;
    private InputAction nextTabAction;
    private InputAction prevTabAction;

    private int currentTabIndex = 1;
    private bool isPaused = false;

    private void Awake()
    {
        pauseAction = playerInput.actions["Pause"];
        cancelAction = playerInput.actions["Cancel"];
        nextTabAction = playerInput.actions["NextTab"];
        prevTabAction = playerInput.actions["PrevTab"];
    }

    private void OnEnable()
    {
        pauseAction.performed += OnPauseInput;
        cancelAction.performed += OnCancelInput;
        nextTabAction.performed += OnNextTabInput;
        prevTabAction.performed += OnPrevTabInput;

    }

    private void OnDisable()
    {
        pauseAction.performed -= OnPauseInput;
        cancelAction.performed -= OnCancelInput;
        nextTabAction.performed-= OnNextTabInput;
        prevTabAction.performed -= OnPrevTabInput;
    }

    private void Start()
    {
        pauseMenuContainer.SetActive(false);

        for(int i = 0; i < tabButtons.Length; i++)
        {
            int index = i;
            tabButtons[i].onClick.AddListener(() => SelectTab(index));
        }
    }


    private void OnPauseInput(InputAction.CallbackContext context)
    {
        if (!isPaused) PauseGame();
    }

    private void OnCancelInput(InputAction.CallbackContext context) 
    {
        if (isPaused) ResumeGame();
    }

    private void OnNextTabInput(InputAction.CallbackContext context)
    {
        if (!isPaused) return;
        int nextTab = (currentTabIndex + 1) % tabPanels.Length;
        SelectTab(nextTab);
    }

    private void OnPrevTabInput(InputAction.CallbackContext context)
    {
        if (!isPaused) return;
        int prevTab = (currentTabIndex - 1+tabPanels.Length) % tabPanels.Length;
        SelectTab(prevTab);
    }

    public void PauseGame()
    {
        isPaused = true;
        pauseMenuContainer.SetActive(true);
        Time.timeScale = 0f;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        playerInput.SwitchCurrentActionMap("UI");
        SelectTab(1);
    }

    public void ResumeGame()
    {
        isPaused = false;
        pauseMenuContainer.SetActive(false);
        Time.timeScale = 1f;

        Cursor.visible = false;

        playerInput.SwitchCurrentActionMap("Player");
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void SelectTab(int index)
    {
        currentTabIndex = index;

        for(int i =0; i < tabPanels.Length; i++)
        {
            bool isActive = (i == index);
            tabPanels[i].SetActive(isActive);

            var colors = tabButtons[i].colors;
            colors.normalColor = isActive ? activeTabColor : inactiveTabColor;
            tabButtons[i].colors = colors;

        }

        if (firstSelected != null && index < firstSelected.Length && firstSelected[index] != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(firstSelected[index]);
        }
    }

    public void OnSaveClicked() => Debug.Log("Zapisano grę");
    public void OnLoadClicked() => Debug.Log("Wczytano zapis");
    public void OnOptionsClicked() => Debug.Log("opcje");
    public void OnQuitClicked()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
