using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;
using System.Collections.Generic;
using TMPro;
using System;

public class BlacksmithInventoryManager : MonoBehaviour
{
    [SerializeField] private List<WeaponData> allWeapons = new List<WeaponData>();


    [Header("Referencje do kontenerów (content)")]
    [SerializeField] private Transform unlockedContentContainer;
    [SerializeField] private Transform shopContentContainer;



    [Header("Prefab przedmiotu UI")]
    [SerializeField] private GameObject weaponUIPrefab;

    [Header("Przyciski zakładek")]
    [SerializeField] private Button btnUnlocked;
    [SerializeField] private Button btnShop;

    [Header("Panele zakładek")]
    [SerializeField] private GameObject unlockedPanel;
    [SerializeField] private GameObject shopPanel;

    [Header("Panel Szczegółów")]
    [SerializeField] private TMP_Text detailsNameText;
    [SerializeField] private TMP_Text detailsStatsText;
    [SerializeField] private Image detailsIconImage;
    [SerializeField] private Button actionButton;
    [SerializeField] private TMP_Text actionButtonText;

    private WeaponData currentlySelectedWeapon;


    private void OnEnable()
    {
        RefreshBlacksmithUI();
        OpenUnlockedTab();
    }

    private void RefreshBlacksmithUI()
    {
        GameObject newUIItem = null;

        foreach (Transform child in unlockedContentContainer) Destroy(child.gameObject);
        foreach (Transform child in shopContentContainer) Destroy(child.gameObject);


        foreach (var weapon in allWeapons)
        {
            if (weapon.isUnlocked)
            {
                newUIItem = Instantiate(weaponUIPrefab, unlockedContentContainer);
                SetupItemButton(newUIItem, weapon);

            }
            else
            {
                newUIItem = Instantiate(weaponUIPrefab, shopContentContainer);
                SetupItemButton(newUIItem, weapon);
            }
        }

        SelectFirstAvailableWeapon();
    }

    private void SetupItemButton(GameObject uiItem, WeaponData weapon)
    {
        TMP_Text nameText = uiItem.transform.Find("NameText")?.GetComponent<TMP_Text>();
        if (nameText) nameText.text = weapon.weaponName;

        Image iconImage = uiItem.transform.Find("Icon")?.GetComponent<Image>();
        if (iconImage) iconImage.sprite = weapon.weaponIcon;

        Button btn = uiItem.GetComponent<Button>();
        if (btn != null)
        {
            // Kliknięcie myszką / zatwierdzenie padem wybiera ten przedmiot do panelu szczegółów
            btn.onClick.AddListener(() =>
            {
                DisplayWeaponDetails(weapon);
            });
        }

        // Obsługa fokusu dla pada (gdy gracz najeżdża padem na kafelek, automatycznie pokazuj jego statysty!)
        EventTrigger trigger = uiItem.AddComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.Select;
        entry.callback.AddListener((eventData) => { DisplayWeaponDetails(weapon); });
        trigger.triggers.Add(entry);
    }


    public void DisplayWeaponDetails(WeaponData weapon)
    {
        currentlySelectedWeapon = weapon;

        if (detailsNameText) detailsNameText.text = weapon.weaponName;

        if (detailsStatsText)
        {
            detailsStatsText.text = $"Obrażenia: {weapon.baseDMG}\n" +
                                    $"Prędkość ataku: {weapon.attackSpeed}\n" +
                                    (weapon.isUnlocked ? "" : $"Cena: {weapon.price} Złota");
        }

        if (detailsIconImage) detailsIconImage.sprite = weapon.weaponIcon;

        // Konfiguracja przycisku akcji (Załóż / Kup)
        if (actionButton != null && actionButtonText != null)
        {
            actionButton.onClick.RemoveAllListeners();

            if (weapon.isUnlocked)
            {
                actionButtonText.text = "Załóż";
                actionButton.onClick.AddListener(() => EquipWeapon(weapon));
            }
            else
            {
                actionButtonText.text = "Kup (" + weapon.price + ")";
                actionButton.onClick.AddListener(() => TryBuyWeapon(weapon));
            }
        }
    }

    private void EquipWeapon(WeaponData weapon)
    {
        Debug.Log("Założono broń: " + weapon.weaponName);
        // Tutaj dodaj logikę wyposażania postaci w tę broń
    }

    private void TryBuyWeapon(WeaponData weapon)
    {
        Debug.Log("Próba zakupu: " + weapon.weaponName);

        // Przykład logiki zakupu:
        // if (playerGold >= weapon.price)
        // {
        //     playerGold -= weapon.price;
        //     weapon.isUnlocked = true;
        //     RefreshBlacksmithUI();
        //     DisplayWeaponDetails(weapon); // Odśwież widok po zakupie
        // }
    }


    public void OpenUnlockedTab()
    {
        unlockedPanel.SetActive(true);
        shopPanel.SetActive(false);
        SelectFirstItemInContainer(unlockedContentContainer);
    }

    public void OpenShopTab()
    {
        unlockedPanel.SetActive(false);
        shopPanel.SetActive(true);
        SelectFirstItemInContainer(shopContentContainer);
    }

    private void SelectFirstItemInContainer(Transform container)
    {
        if (EventSystem.current == null) return;

        EventSystem.current.SetSelectedGameObject(null);

        if (container.childCount > 0)
        {
            GameObject firstItem = container.GetChild(0).gameObject;
            Button firstButton = firstItem.GetComponent<Button>();
            if (firstButton != null)
            {
                EventSystem.current.SetSelectedGameObject(firstButton.gameObject);
                // Wywołaj od razu szczegóły dla pierwszego elementu
                // (EventTrigger wywoła to automatycznie, ale dla pewności możemy też wywołać bezpośrednio)
            }
        }
    }

    private void SelectFirstAvailableWeapon()
    {
        Transform activeContainer = unlockedPanel.activeSelf ? unlockedContentContainer : shopContentContainer;
        if (activeContainer.childCount > 0)
        {
            GameObject firstItem = activeContainer.GetChild(0).gameObject;
            EventSystem.current.SetSelectedGameObject(firstItem);
        }
    }
}
