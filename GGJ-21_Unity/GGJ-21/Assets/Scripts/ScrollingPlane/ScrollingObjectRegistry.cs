using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace ScrollingPlane
{
    public class ScrollingObjectRegistry : MonoBehaviour
    {
        private static ScrollingObjectRegistry instance;

        private List<ScrollingObject> currentScrollingObjects = new List<ScrollingObject>();

        private void Awake()
        {
            Assert.IsNull(instance, "There can be only one ScrollingObjectRegistry in the scene!");
            instance = this;
        }

        public static void Add(ScrollingObject scrollingObject)
        {
            Assert.IsFalse(instance.currentScrollingObjects.Contains(scrollingObject), "Already registered!");
            instance.currentScrollingObjects.Add(scrollingObject);
        }
        
        public static void Remove(ScrollingObject scrollingObject)
        {
            Assert.IsTrue(instance.currentScrollingObjects.Contains(scrollingObject), "Trying to deregister non-existent object!");
            instance.currentScrollingObjects.Remove(scrollingObject);
        }

        public static List<ScrollingObject> GetScrollingObjects()
        {
            return instance.currentScrollingObjects;
        }
    }
}
