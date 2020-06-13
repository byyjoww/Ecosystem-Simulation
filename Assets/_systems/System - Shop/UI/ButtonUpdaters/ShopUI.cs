using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopUI : MonoBehaviour
{
    [System.Serializable]
    public class ShopSection 
    {
        public string name;
        [HideInInspector] public GameObject tab;
        [HideInInspector] public GameObject panel;
        public Transform Viewport => panel.transform.GetChild(0);

        public List<Purchase> elementsToInstantiate;
    }

    #region TABS
    [SerializeField] private GameObject pfTabElement;
    [SerializeField] private Transform tTabElement;
    private List<GameObject> instantiatedTabsList;
    #endregion

    #region PANELS
    [SerializeField] public GameObject pfPanelElement;
    [SerializeField] private Transform tPanelElement;
    private List<GameObject> instantiatedPanelList;
    #endregion

    #region  ELEMENTS
    private List<GameObject> instantiatedElementsList;
    #endregion

    #region SECTIONS
    public List<ShopSection> sectionsToInstantiate = new List<ShopSection>();
    private ShopSection selectedSection { get; set; }
    #endregion

    #region EVENTS
    private event Action OnShopChanged;     
    public event Action<ShopSection> OnTabSelected;
    public event Action<ShopSection> OnTabDeselected;
    #endregion

    #region INITIALIZE
    private bool isInitialized;

    public void Initialize()
    {
        if (isInitialized)
        {
            return;
        }

        instantiatedTabsList = new List<GameObject>();
        instantiatedPanelList = new List<GameObject>();
        instantiatedElementsList = new List<GameObject>();

        CreatePanels();
        CreateTabs();
        RefreshUI();

        isInitialized = true;
    }

    public void OnEnable()
    {
        Initialize();
        RefreshUI();
        TabClicked(sectionsToInstantiate[0]);        
        OnShopChanged += RefreshUI;
    }

    private void OnDisable()
    {
        OnShopChanged -= RefreshUI;        
    }
    #endregion

    #region SETUP
    public void CreateTabs()
    {
        foreach (var section in sectionsToInstantiate)
        {
            GameObject tab = Instantiate(pfTabElement, tTabElement);
            section.tab = tab;
            tab.GetComponentInChildren<TMP_Text>().text = section.name;
            tab.GetComponent<Button>().onClick.AddListener(delegate { TabClicked(section); });
            instantiatedTabsList.Add(tab);
        }
    }

    public void CreatePanels()
    {
        foreach (var section in sectionsToInstantiate)
        {
            GameObject panel = Instantiate(pfPanelElement, tPanelElement);
            section.panel = panel;
            instantiatedPanelList.Add(panel);
        }
    }

    public void RefreshUI()
    {
        foreach (GameObject obj in instantiatedElementsList)
        {
            Destroy(obj);
        }

        instantiatedElementsList.Clear();

        foreach (var section in sectionsToInstantiate)
        {
            Transform tShopElement = section.panel.transform.GetChild(0);

            foreach (var element in section.elementsToInstantiate)
            {
                var obj = (element).CreateUIElement(section.Viewport);
                instantiatedElementsList.Add(obj);
            }            
        }
    }
    #endregion

    #region TAB MANAGEMENT
    public void Select(ShopSection section)
    {
        OnTabSelected?.Invoke(section);
    }

    public void Deselect(ShopSection section)
    {
        OnTabDeselected?.Invoke(section);
    }

    public void TabClicked(ShopSection section)
    {
        if (selectedSection != null)
        {
            Deselect(selectedSection);
        }

        selectedSection = section;
        Select(section);
        ResetTabs(section);
    }

    private void ResetTabs(ShopSection section)
    {
        foreach (var panel in instantiatedPanelList)
        {
            panel.SetActive(false);
        }

        section.panel.SetActive(true);
    }
    #endregion
}