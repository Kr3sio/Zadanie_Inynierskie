using UnityEngine;
using UnityEngine.InputSystem;

public class NPCInteraction2D : MonoBehaviour
{
    [Header("UI Panel")]
    [Tooltip("Przeciągnij tutaj panel UI sklepu lub kowala")]
    public GameObject shopPanel;

    [Header("Konfiguracja")]
    public string interactActionName = "Interact";

    private bool isPlayerInRange = false;
    private InputAction interactAction;

    private void Awake()
    {
        PlayerInput playerInput = FindFirstObjectByType<PlayerInput>();
        if (playerInput != null && playerInput.actions != null)
        {
            interactAction = playerInput.actions[interactActionName];
        }
        else
        {
            Debug.LogError("Nie znaleziono komponentu PlayerInput na scenie!");
        }
    }

    private void OnEnable()
    {
        if (interactAction != null)
        {
            interactAction.Enable();
            interactAction.performed += OnInteractPerformed;
        }
    }

    private void OnDisable()
    {
        if (interactAction != null)
        {
            interactAction.performed -= OnInteractPerformed;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            Debug.Log("Naciśnij przycisk interakcji, aby otworzyć okno.");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Sprawdzamy tag, ale resetujemy też stan, żeby uniknąć zablokowania zmiennej
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            if (shopPanel != null && shopPanel.activeSelf)
            {
                shopPanel.SetActive(false);
            }
            Debug.Log("Gracz opuścił strefę NPC.");
        }
    }

    private void OnInteractPerformed(InputAction.CallbackContext context)
    {
        // KLUCZOWE: Sprawdzamy, czy gracz na pewno jest w strefie
        if (!isPlayerInRange) return;

        if (shopPanel != null)
        {
            bool isActive = shopPanel.activeSelf;
            shopPanel.SetActive(!isActive);
            Debug.Log("Przełączono stan panelu sklepu!");
        }
        else
        {
            Debug.Log("Interakcja 2D działa! (Brak przypisanego panelu)");
        }
    }
}