using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public abstract class StoreItemData : ScriptableObject
{
    [SerializeField] protected string itemId;
    public string ItemSKU => Application.identifier + "." + itemId;

    [SerializeField] PriceTier priceTier;
    protected int usdPrice => usdPrices[priceTier.tier];

    [System.Serializable]
    class PriceTier
    {
        public int tier;
    }

    static readonly int[] usdPrices =
    {
        0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26,
        27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50,
        55, 60, 65, 70, 75, 80, 85, 90, 95, 100, 110, 120, 125, 130, 140, 150, 160, 170, 175, 180, 190,
        200, 210, 220, 230, 240, 250, 300, 350, 400, 450, 500, 600, 700, 800, 900, 1000
    };

    #if UNITY_EDITOR
    [SerializeField] protected string referenceName;
    [SerializeField] protected string itemName;
    [SerializeField] protected string itemDescription;
    [SerializeField] protected string reviewScreenshot = "screenshots/iaps.png";

    static readonly string[] pricesOptions =
    {
        "Tier 0 - $0.00", "Tier 1 - $0.99", "Tier 2 - $1.99", "Tier 3 - $2.99", "Tier 4 - $3.99", "Tier 5 - $4.99",
        "Tier 6 - $5.99", "Tier 7 - $6.99", "Tier 8 - $7.99", "Tier 9 - $8.99", "Tier 10 - $9.99", "Tier 11 - $10.99",
        "Tier 12 - $11.99", "Tier 13 - $12.99", "Tier 14 - $13.99", "Tier 15 - $14.99", "Tier 16 - $15.99",
        "Tier 17 - $16.99", "Tier 18 - $17.99", "Tier 19 - $18.99", "Tier 20 - $19.99", "Tier 21 - $20.99",
        "Tier 22 - $21.99", "Tier 23 - $22.99", "Tier 24 - $23.99", "Tier 25 - $24.99", "Tier 26 - $25.99",
        "Tier 27 - $26.99", "Tier 28 - $27.99", "Tier 29 - $28.99", "Tier 30 - $29.99", "Tier 31 - $30.99",
        "Tier 32 - $31.99", "Tier 33 - $32.99", "Tier 34 - $33.99", "Tier 35 - $34.99", "Tier 36 - $35.99",
        "Tier 37 - $36.99", "Tier 38 - $37.99", "Tier 39 - $38.99", "Tier 40 - $39.99", "Tier 41 - $40.99",
        "Tier 42 - $41.99", "Tier 43 - $42.99", "Tier 44 - $43.99", "Tier 45 - $44.99", "Tier 46 - $45.99",
        "Tier 47 - $46.99", "Tier 48 - $47.99", "Tier 49 - $48.99", "Tier 50 - $49.99", "Tier 51 - $54.99",
        "Tier 52 - $59.99", "Tier 53 - $64.99", "Tier 54 - $69.99", "Tier 55 - $74.99", "Tier 56 - $79.99",
        "Tier 57 - $84.99", "Tier 58 - $89.99", "Tier 59 - $94.99", "Tier 60 - $99.99", "Tier 61 - $109.99",
        "Tier 62 - $119.99", "Tier 63 - $124.99", "Tier 64 - $129.99", "Tier 65 - $139.99", "Tier 66 - $149.99",
        "Tier 67 - $159.99", "Tier 68 - $169.99", "Tier 69 - $174.99", "Tier 70 - $179.99", "Tier 71 - $189.99",
        "Tier 72 - $199.99", "Tier 73 - $209.99", "Tier 74 - $219.99", "Tier 75 - $229.99", "Tier 76 - $239.99",
        "Tier 77 - $249.99", "Tier 78 - $299.99", "Tier 79 - $349.99", "Tier 80 - $399.99", "Tier 81 - $449.99",
        "Tier 82 - $499.99", "Tier 83 - $599.99", "Tier 84 - $699.99", "Tier 85 - $799.99", "Tier 86 - $899.99",
        "Tier 87 - $999.99"
    };

    public string ToYML()
    {
        var str = "- product_id: " + ItemSKU +
                     "\n\treference_name: " + referenceName +
                     "\n\ttype: consumable" +
                     "\n\tprice_tier: " + priceTier.tier +
                     "\n\tlocales: " +
                     "\n\t\ten-US: " +
                     "\n\t\t\tname: " + itemName +
                     "\n\t\t\tdescription: " + itemDescription +
                     "\n\treview_screenshot: " + reviewScreenshot;
        return str;
    }

    [CustomPropertyDrawer(typeof(PriceTier))]
    class IngredientDrawerUIE : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            // Don't make child fields be indented
            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            var tierProperty = property.FindPropertyRelative("tier");

            tierProperty.intValue = EditorGUI.Popup(position, label.text, tierProperty.intValue, pricesOptions);

            // Set indent back to what it was
            EditorGUI.indentLevel = indent;

            EditorGUI.EndProperty();
        }
    }

    [CustomEditor( typeof( StoreItemData ), true )]
    public class StoreItemDataEditor : Editor {


        StoreItemData m_Instance;

        public void OnEnable()
        {
            m_Instance = target as StoreItemData;
        }

        public override void OnInspectorGUI () {

            if ( m_Instance == null )
                return;

            this.DrawDefaultInspector();

            EditorGUILayout.HelpBox("SKU: " + m_Instance.ItemSKU, MessageType.Info);
        }
    }

    #endif
}
