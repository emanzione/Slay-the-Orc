using System.Collections.Generic;
using UnityEngine;

namespace MHLab.InfectionsBlaster.Extensions
{
    public static class GameObjectExtensions
    {
        private static List<Component> _componentsCache = new List<Component>(8);

        public static TComponent GetComponentNoAlloc<TComponent>(this GameObject go) where TComponent : Component
        {
            go.GetComponents(typeof(TComponent), _componentsCache);
            var component = _componentsCache.Count > 0 ? _componentsCache[0] : null;
            
            _componentsCache.Clear();
            return (TComponent)component;
        }
        
        public static bool GetComponentNoAlloc<TComponent>(this GameObject go, out TComponent component) where TComponent : Component
        {
            go.GetComponents(typeof(TComponent), _componentsCache);

            if (_componentsCache.Count == 0)
            {
                component = null;
                return false;
            }

            component = (TComponent)_componentsCache[0];
            _componentsCache.Clear();
            return true;
        }
    }
}