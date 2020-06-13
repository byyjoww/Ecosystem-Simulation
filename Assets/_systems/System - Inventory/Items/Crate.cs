using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Crate", menuName = "Scriptable Object/Item/Crate")]
public class Crate : ScriptableElement
{
    [System.Serializable]
    public class CrateData
    {
        [RequireInterface(typeof(IInventoryElement))]
        public Item item;
        [Range(0, 1)] public float dropChance;
    }

    public class ResultData
    {
        [RequireInterface(typeof(IInventoryElement))]
        public Item item;
        public int quantity;

        public ResultData(Item item, int quantity)
        {
            this.item = item;
            this.quantity = quantity;
        }
    }

    public override Database.DatabaseType EnumIdentifier => Database.DatabaseType.Crate;
    [SerializeField] private bool givesUnique;
    [SerializeField] private List<CrateData> possibleItemsList = new List<CrateData>();
    
    public List<ResultData> CrateContents 
    { 
        get 
        { 
            if (givesUnique) 
            {
                return OpenUniqueChest();
            }
            else
            {
                return OpenMultiChest();
            }
        } 
    }

    private List<ResultData> OpenMultiChest()
    {
        List<ResultData> contents = new List<ResultData>();

        foreach (CrateData crateContent in possibleItemsList)
        {
            float r = Random.Range(0, 1);
            if(r < crateContent.dropChance)
            {
                contents.Add(new ResultData(crateContent.item, 1));
            }
        }

        return contents;
    }

    private List<ResultData> OpenUniqueChest()
    {
        List<ResultData> contents = new List<ResultData>();
        float totalProbability = 0;

        foreach (CrateData crateContent in possibleItemsList)
        {
            totalProbability += crateContent.dropChance;
        }

        float random = UnityEngine.Random.Range(0, totalProbability);

        float minChance = 0;

        foreach (CrateData crateContent in possibleItemsList)
        {
            if (crateContent.dropChance == 0)
            {
                continue;
            }

            if (random >= minChance && random < minChance + crateContent.dropChance)
            {
                contents.Add(new ResultData(crateContent.item, 1));
                return contents; 
            }

            minChance += crateContent.dropChance;
        }

        throw new System.Exception("NO VALID ITEMS FOUND!");
    }
}
