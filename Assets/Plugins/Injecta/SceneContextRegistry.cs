using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace Injecta
{
    public class SceneContextRegistry
    {
        private Dictionary<Scene, SceneContext> _map = new Dictionary<Scene, SceneContext>();

        public void Add(SceneContext context)
        {
            _map.Add(context.gameObject.scene, context);
        }

        public void Remove(SceneContext context)
        {
            _map.Remove(context.gameObject.scene);
        }

        public bool TryGetSceneContext(string name, out SceneContext context)
        {
            Scene scene = SceneManager.GetSceneByName(name);

            return TryGetSceneContext(scene, out context);
        }

        public bool TryGetSceneContext(Scene scene, out SceneContext context)
        {
            return _map.TryGetValue(scene, out context);
        }
    }
}
