using System.Collections.Generic;
using UnityEngine;
using Elysium.AI.GOAP;

namespace Elysium.AI.GOAP
{
    public sealed class GWorld
    {
        private static readonly GWorld instance = new GWorld();
        private static WorldStates world;
        private static Queue<IResource> resources;

        static GWorld()
        {
            world = new WorldStates();
            resources = new Queue<IResource>();
        }

        public bool CheckResources()
        {
            if (resources.Count > 0)
            {
                return true;
            }

            return false;
        }

        public void AddResource(IResource resource)
        {
            resources.Enqueue(resource);
        }

        public IResource RemoveResource()
        {
            if (resources.Count == 0) 
                return null;

            return resources.Dequeue();
        }

        public static GWorld Instance
        {
            get { return instance; }
        }

        public WorldStates GetWorld()
        {
            return world;
        }
    }
}