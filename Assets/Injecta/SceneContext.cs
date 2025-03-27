using UnityEngine;

namespace Injecta
{
    public class SceneContext : Context
    {
        protected override void Initialize()
        {
            DI.SceneContextRegistry.Add(this);
        }

        protected override Container GetContainer()
        {
            return new Container(ProjectContext.Instance.Container);
        }

        protected override GameObject[] GetRoots()
        {
            return gameObject.scene.GetRootGameObjects();
        }

        private void OnDestroy()
        {
            DI.SceneContextRegistry.Remove(this);
        }
    }
}
