using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Injecta
{
    public class SceneContextRegistry
    {
        private Dictionary<string, SceneContext> _map = new Dictionary<string, SceneContext>();

        public void Add(SceneContext context)
        {
            string sceneName = context.gameObject.scene.name;

            if (!_map.TryAdd(sceneName, context))
            {
                Debug.LogError($"The {sceneName} scene already has a Scene context.There should only be one Scene context in the scene.");
            }
        }
        public void Remove(SceneContext context)
        {
            _map.Remove(context.gameObject.scene.name);
        }

        public bool TryGetSceneContext(string sceneName, out SceneContext context)
        {
            return _map.TryGetValue(sceneName, out context);
        }

        public bool TryGetSceneContext(Scene scene, out SceneContext context)
        {
            return TryGetSceneContext(scene.name, out context);
        }
    }
}
