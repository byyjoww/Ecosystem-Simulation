using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

public class GameAssets : MonoBehaviour
{    
    private static GameAssets _i;

    private void Awake()
    {
        cameraShake = Camera.main.GetComponent<CameraShake>();
    }

    public static GameAssets Instance
    {
        get
        {
            if (_i == null)
            {
                _i = Instantiate(Resources.Load<GameAssets>("GameAssets"), GameObject.Find("Canvas").transform);
            }
            return _i;
        }
    }

    [Header("Visuals")]
    public List<GameObject> panelList;
    public GameObject pfDamagePopup;

    [Header("VFX")]
    public List<GameObject> effectsList;
    public CameraShake cameraShake;

    [Header("Popups")]
    public GameObject pfTitleTextPopup;
    public GameObject pfTitleTextButtonPopup;
    public GameObject pfTitleTextImagePopup;

    [Header("Layout Elements")]
    public GameObject pfShopItemLayoutElement;
    public GameObject pfShopManualLayoutElement;
    public GameObject pfItemPurchaseDetailsPanel;
    public GameObject pfManualPurchaseDetailsPanel;
}
