using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Injecta
{
    public class SceneContext : Context
    {
        protected override void Initialize()
        {
            DI.SceneContextRegistry.Add(this);

            // Revisar esto ya que luego la clase padre lo sobreescribe
            Container = GetContainer();
            Container.BindInstance(Container);
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
