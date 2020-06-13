using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class HealthAnimations : MonoBehaviour
{
    [SerializeField] private Health health;
    [SerializeField] private List<HealthDamageData> healthDamageDataElements = new List<HealthDamageData>();
    private List<GameObject> currentEffects;

    [System.Serializable]
    public class HealthDamageData
    {
        public int minPercent;
        public int maxPercent;

        public GameObject effect;
    }    

    public void Start()
    {
        currentEffects = new List<GameObject>();
        health.OnChanged += SetVisuals;
        SetVisuals(health.Current, health.Current);
    }

    public void SetVisuals(int previous, int current)
    {
        foreach (var effect in currentEffects)
        {
            Destroy(effect);
        }

        foreach (var dataElement in healthDamageDataElements)
        {
            if(health.Current > dataElement.minPercent && health.Current < dataElement.maxPercent)
            {
                InstantiateEffect(dataElement);
            }
        }
    }

    private void InstantiateEffect(HealthDamageData dataElement)
    {
        GameObject effect = GameObject.Instantiate(dataElement.effect, transform);
        currentEffects.Add(effect);
    }

    private void OnValidate()
    {
        if (health == null) health = GetComponent<Health>();
    }
}
