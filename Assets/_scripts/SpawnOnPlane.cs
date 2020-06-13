using UnityEngine;
using System.Collections.Generic;

public class SpawnOnPlane : MonoBehaviour
{
    [Header("Objects")]
    [SerializeField] GameObject plantObject;
    [SerializeField] GameObject rabbitObject;
    [SerializeField] GameObject foxObject;

    [Header("Spawn Times")]
    [SerializeField] float plantSpawnTime;
    [SerializeField] float rabbitSpawnTime;
    [SerializeField] float foxSpawnTime;

    [Header("Max Objects")]
    [SerializeField] int maxPlants;
    [SerializeField] int maxRabbits;
    [SerializeField] int maxFoxes;

    private List<GameObject> plants;
    private List<GameObject> rabbits;
    private List<GameObject> foxes;

    private void Start()
    {
        plants = new List<GameObject>();
        rabbits = new List<GameObject>();
        foxes = new List<GameObject>();

        InvokeRepeating("InstantiatePlants", plantSpawnTime, plantSpawnTime);
        InvokeRepeating("InstantiateRabbit", rabbitSpawnTime, rabbitSpawnTime);
        InvokeRepeating("InstantiateFox", foxSpawnTime, foxSpawnTime);
    }

    public void InstantiatePlants()
    {
        plants.RemoveAll(x => x == null);

        if (plants.Count >= maxPlants)
        {
            return;
        }

        var obj = Instantiate(plantObject, RandomPointOnPlane(), Quaternion.identity);
        plants.Add(obj);
    }

    public void InstantiateRabbit()
    {
        rabbits.RemoveAll(x => x == null);

        if (rabbits.Count >= maxRabbits)
        {
            return;
        }

        var obj = Instantiate(rabbitObject, RandomPointOnPlane(), Quaternion.identity);
        rabbits.Add(obj);
    }

    public void InstantiateFox()
    {
        foxes.RemoveAll(x => x == null);

        if (foxes.Count >= maxFoxes)
        {
            return;
        }

        var obj = Instantiate(foxObject, RandomPointOnPlane(), Quaternion.identity);
        foxes.Add(obj);
    }

    private Vector3 RandomPointOnPlane()
    {
        var filter = GetComponent<MeshFilter>();
        Vector3 min = filter.mesh.bounds.min;
        Vector3 max = filter.mesh.bounds.max;

        Vector3 scale = transform.localScale;

        var pos = transform.position - new Vector3((Random.Range(min.x * scale.x, max.x * scale.x)), transform.position.y, (Random.Range(min.z * scale.z, max.z * scale.z)));
        return pos;
    }
}