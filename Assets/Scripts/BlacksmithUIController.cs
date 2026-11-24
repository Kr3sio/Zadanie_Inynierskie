using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class BlacksmithUIController : MonoBehaviour
{

    //[Header("Panels")]
    //[SerializeField] private GameObject unlockedContent;
    //[SerializeField] private GameObject shopContent;
    //[SerializeField] private GameObject classContent;

    //[Header("Buttons")]
    //[SerializeField] private Button unlockedButton;
    //[SerializeField] private Button shopButton;

    
    //[SerializeField] private Button exitButton;

    //[Header(" ")]
    //[SerializeField] private GameObject firstSelectedOnOpen;


    


    //private void OnEnable()
    //{
    //    ShowUnlockedTab();
    //}

    //public void ShowUnlockedTab()
    //{
    //    unlockedContent.SetActive(true);
    //    shopContent.SetActive(false);
    //    classContent.SetActive(false);

    //    unlockedButton.interactable = false;
    //    shopButton.interactable = true;

    //    FocusFirstElement(unlockedButton.gameObject);
    //}

    //public void ShowShopTab()
    //{
    //    unlockedContent.SetActive(false);
    //    shopContent.SetActive(true);
    //    classContent.SetActive(false);

    //    unlockedButton.interactable = true;
    //    shopButton.interactable = false;

    //    FocusFirstElement(unlockedButton.gameObject);
    //}
    ////public void ShowShopTab()
    ////{
    ////    unlockedPanel.SetActive(false);
    ////    shopPanel.SetActive(false);
    ////    classPanel.SetActive(true);
    ////    SelectFirstItemInContainer(classContentContainer);

    ////    unlockedButton.interactable = true;
    ////    shopButton.interactable = false;

    ////    FocusFirstElement(unlockedButton.gameObject);
    ////}

    //private void FocusFirstElement(GameObject elementToSelect)
    //{
    //    if (EventSystem.current != null)
    //    {

    //        EventSystem.current.SetSelectedGameObject(null);
    //        EventSystem.current.SetSelectedGameObject(elementToSelect);
    //    }
    //}
}
