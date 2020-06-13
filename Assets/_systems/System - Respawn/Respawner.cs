using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public static class Respawner
{
    private const float globalRespawnTime = 5f;

    private static List<RespawnData> respawnQueue;
    private static List<GameObject> instantiatedObjectsList;

    [System.Serializable]
    public class RespawnData
    {
        public RespawnData(GameObject objToSpawn, GameObject objToDestroy, Transform respawnTransform, float timeToSpawn = globalRespawnTime)
        {
            this.objectToSpawn = objToSpawn;
            this.objectToDestroy = objToDestroy;            
            this.secondsToSpawn = timeToSpawn;
            this.respawnTransform = respawnTransform;
        }

        public GameObject objectToSpawn;
        public GameObject objectToDestroy;
        public Transform respawnTransform;
        public float secondsToSpawn;
    }

    public static void QueueRespawn(RespawnData respawnData)
    {
        // ADD TO RESPAWN QUEUE
        respawnQueue.Add(respawnData);

        // SET TIMER
        Timer timer = new Timer();
        timer.Interval = respawnData.secondsToSpawn * 1000;
        timer.Enabled = true;
        timer.Elapsed += (object source, ElapsedEventArgs e) => Respawn(respawnData);

        // DESTROY INSTANTIATED OBJECT
        instantiatedObjectsList.Remove(respawnData.objectToDestroy);
        GameObject.Destroy(respawnData.objectToDestroy);        
    }

    private static void Respawn(RespawnData respawnData)
    {
        // REMOVE FROM RESPAWN LIST
        respawnQueue.Remove(respawnData);

        // RESPAWN OBJECT
        GameObject obj = GameObject.Instantiate(respawnData.objectToSpawn, respawnData.respawnTransform.position, respawnData.respawnTransform.rotation);
        SetRespawnParameters(respawnData);

        // ADD TO INSTANTIATED LIST
        instantiatedObjectsList.Add(obj);
    }

    private static void SetRespawnParameters(RespawnData respawnData)
    {
        // SET RESPAWN PARAMETERS
    }
}
