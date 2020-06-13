using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Elysium.AI.GOAP;

namespace Elysium.AI.GOAP
{
    public enum ResourceType { Cubicle = 1, Patient = 2 }

    public interface IResource
    {
        ResourceType resourceType { get; }
        GameObject gameObject { get; set; }
    }

    public class Cubicle : IResource
    {
        public ResourceType resourceType => ResourceType.Cubicle;
        public GameObject gameObject { get; set; }

        public Cubicle(GameObject gameObject)
        {
            this.gameObject = gameObject;
        }
    }
}