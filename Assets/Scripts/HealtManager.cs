using UnityEngine;
using UnityEngine.UI;

public class HealtManager : MonoBehaviour
{
    [SerializeField]  private Image healthBar;

    [SerializeField]  private Player player;

    

    void Start()
    {
        if (player == null)
        {
            player = FindObjectOfType<Player>();
        }

        UpdateHealthUI();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHealthUI();
    }

    private void UpdateHealthUI()
    {
        healthBar.fillAmount = player.CurrentHp/player.MaxHp;
    }
}
