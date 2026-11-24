using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;
using System.Collections.Generic;
using TMPro;
using System;

public class BlacksmithInventoryManager : MonoBehaviour
{
    [SerializeField] private List<WeaponData> allWeapons = new List<WeaponData>();
    [SerializeField] private List<CharacterClass> allClass = new List<CharacterClass>();

    [Header("Referencje do kontenerów (content)")]
    [SerializeField] private Transform unlockedContentContainer;
    [SerializeField] private Transform shopContentContainer;
    [SerializeField] private Transform classContentContainer;

    [Header("Prefab przedmiotu UI")]
    [SerializeField] private GameObject weaponUIPrefab;

    [Header("Przyciski zakładek")]
    [SerializeField] private Button btnUnlocked;
    [SerializeField] private Button btnShop;

    [SerializeField] private Button btnClass;

    [Header("Panele zakładek")]
    [SerializeField] private GameObject unlockedPanel;
    [SerializeField] private GameObject shopPanel;

    [SerializeField] private GameObject classPanel;

    [SerializeField] private GameObject ItemDetailsPanel;

    [SerializeField] private GameObject ClassDetailsPanel;

    [Header("Panel Szczegółów")]
    [SerializeField] private TMP_Text detailsNameText;
    [SerializeField] private TMP_Text detailsStatsText;
    [SerializeField] private Image detailsIconImage;
    [SerializeField] private Button actionButton;
    [SerializeField] private TMP_Text actionButtonText;

    private WeaponData currentlySelectedWeapon;

    private CharacterClass currentlySelectedClass;

    private PlayerClassHandler playerClassHandler;


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
        foreach (Transform child in classContentContainer) Destroy(child.gameObject);


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

        foreach (var characterClass in allClass)
        {
            
             newUIItem = Instantiate(weaponUIPrefab, classContentContainer);
             SetupClassButton(newUIItem, characterClass);

            
               
            
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

    private void SetupClassButton(GameObject uiItem, CharacterClass characterClass)
    {
        TMP_Text nameText = uiItem.transform.Find("NameText")?.GetComponent<TMP_Text>();
        if (nameText) nameText.text = characterClass.className;

        //Image iconImage = uiItem.transform.Find("Icon")?.GetComponent<Image>();
        //if (iconImage) iconImage.sprite = class.classIcon;

        Button btn = uiItem.GetComponent<Button>();
        if (btn != null)
        {
            // Kliknięcie myszką / zatwierdzenie padem wybiera ten przedmiot do panelu szczegółów
            btn.onClick.AddListener(() =>
            {
                DisplayClassDetails(characterClass);
            });
        }

        // Obsługa fokusu dla pada (gdy gracz najeżdża padem na kafelek, automatycznie pokazuj jego statysty!)
        EventTrigger trigger = uiItem.AddComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.Select;
        entry.callback.AddListener((eventData) => { DisplayClassDetails(characterClass); });
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

    public void DisplayClassDetails(CharacterClass clas)
    {
        currentlySelectedClass = clas;

        if (detailsNameText) detailsNameText.text = clas.className;

        if (detailsStatsText)
        {
            detailsStatsText.text = $"Puknty życia: {clas.baseMaxHP}\n" +
                                    $"Obrażenia: {clas.baseDMG}\n" +
                                    $"Prędkość ataku: {clas.baseAttackSpeed}\n" +
                                    $"Obrona: {clas.baseDEF}\n" +
                                    $"Szybkość: {clas.baseSPEED}\n";
                                    
        }

        //if (detailsIconImage) detailsIconImage.sprite = clas.classIcon;

        // Konfiguracja przycisku akcji (Załóż / Kup)
        if (actionButton != null && actionButtonText != null)
        {
            actionButton.onClick.RemoveAllListeners();

            if (clas.isUnlocked)
            {
                actionButtonText.text = "Użyj";
                actionButton.onClick.AddListener(() => EquipClass(clas));
            }
            else
            {
                actionButtonText.text = "W użyciu";
                
            }
        }
    }

    private void EquipWeapon(WeaponData weapon)
    {
        Debug.Log("Założono broń: " + weapon.weaponName);
        // Tutaj dodaj logikę wyposażania postaci w tę broń
    }

    private void EquipClass(CharacterClass clas)
    {
        Debug.Log("Klasa w użyciu: " + clas.className);
        playerClassHandler.InitializeClass(clas);
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
        classPanel.SetActive(false);
        ItemDetailsPanel.SetActive(true);
        ClassDetailsPanel.SetActive(false);
        SelectFirstItemInContainer(unlockedContentContainer);
    }

    public void OpenShopTab()
    {
        unlockedPanel.SetActive(false);
        shopPanel.SetActive(true);
        classPanel.SetActive(false);
        ItemDetailsPanel.SetActive(true);
        ClassDetailsPanel.SetActive(false);
        SelectFirstItemInContainer(shopContentContainer);
    }

    public void OpenClassTab()
    {
        unlockedPanel.SetActive(false);
        shopPanel.SetActive(false);
        classPanel.SetActive(true);
        ItemDetailsPanel.SetActive(false);
        ClassDetailsPanel.SetActive(true);
        SelectFirstItemInContainer(classContentContainer);
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
