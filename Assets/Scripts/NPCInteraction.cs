using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class NPCInteraction2D : MonoBehaviour
{
    [Header("UI Panel")]
    [Tooltip("Przeciągnij tutaj panel UI sklepu lub kowala")]
    public GameObject Panel;

    private Player player;

    private Animator playerAnimator;

    [Header("Konfiguracja")]
    public string interactActionName = "Interact";

    private bool isPlayerInRange = false;
    private InputAction interactAction;

    [SerializeField] private PlayerInput playerInput;

    private void Awake()
    {
        //PlayerInput playerInput = FindFirstObjectByType<PlayerInput>();
        if (playerInput != null && playerInput.actions != null)
        {
            interactAction = playerInput.actions[interactActionName];
        }
        else
        {
            Debug.LogError("Nie znaleziono komponentu PlayerInput na scenie!");
        }
        player = FindFirstObjectByType<Player>();
        if (player != null)
        {
            Debug.Log("Znaleziono komponent Player na scenie!");
            playerAnimator = player.GetComponentInChildren<Animator>();
        }
        else
        {
            Debug.LogError("Nie znaleziono komponentu Player na scenie!");
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
            StopPlayer();
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
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            if (Panel != null && Panel.activeSelf)
            {
                CloseMenu();
            }
            Debug.Log("Gracz opuścił strefę NPC.");
        }
    }

    private void OnInteractPerformed(InputAction.CallbackContext context)
    {
        if (!isPlayerInRange) return;

        if (Panel != null && !Panel.activeSelf)
        {
            
            Panel.SetActive(true);
            StopPlayer();
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            Debug.Log("Otwarto panel!");

        }
        else
        {
            Debug.Log("Interakcja 2D działa! (Brak przypisanego panelu)");
        }
    }

    public void CloseMenu()
    {
        if (Panel != null)
        {
            Panel.SetActive(false);
            StartPlayer();

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            Debug.Log("Zamknięto panel za pomocą przycisku UI!");
        }
    }

    private void StopPlayer()
    {
        Debug.Log("Gracz zatrzymany");
        if (playerInput != null)
        {
            playerInput.SwitchCurrentActionMap("UI");
        }
        if (player != null)
        {
            player.MovementStop();
            player.AttackStop();
            
            Debug.Log("ruch gracza zatrzymany");
        }

        if(playerAnimator != null)
        {
            Debug.Log("Animacje gracza zatrzymane");
            playerAnimator.SetBool("InPanel", true);
            
        }
    }

    private void StartPlayer()
    {
        if (playerInput != null)
        {
            playerInput.SwitchCurrentActionMap("Player");
        }
        if (player != null)
        {
            player.MovementStart();
            player.AttackStart();
            
        }

        if (playerAnimator != null)
        {
            
            playerAnimator.SetBool("InPanel", false);
        }
    }
}