using System.Collections.Generic;
using UnityEngine;
using Elysium.AI.GOAP;

namespace Elysium.AI.GOAP
{
    public class GInventory
    {
        // Store our items in a List
        public List<IResource> resources = new List<IResource>();

        // Method to add items to our list
        public void AddItem(IResource resource)
        {

            resources.Add(resource);
        }

        // Method to search for a particular item
        public IResource FindObjectByType(ResourceType type)
        {

            // Iterate through all the items
            foreach (var i in resources)
            {

                // Found a match
                if (i.resourceType == type)
                {

                    return i;
                }
            }
            // Nothing found
            return null;
        }

        // Remove an item from our list
        public void RemoveItem(GameObject resource)
        {

            int indexToRemove = -1;

            // Search through the list to see if it exists
            foreach (var res in resources)
            {

                // Initially set indexToRemove to 0. The first item in the List
                indexToRemove++;
                // Have we found it?
                if (res.gameObject == resource)
                {

                    break;
                }
            }
            // Do we have something to remove?
            if (indexToRemove >= 1)
            {

                // Yes we do.  So remove the item at indexToRemove
                resources.RemoveAt(indexToRemove);
            }
        }
    }
}