using UnityEngine;

namespace Injecta
{
    public class GameObjectContext : Context
    {
        protected override Container GetContainer()
        {
            DI.TryGetParentContainer(this, out Container container);

            return container;
        }

        protected override GameObject[] GetRoots()
        {
            return new[] { gameObject };
        }

        protected bool TryGetParentContext(out Context context)
        {
            context = GetComponentInParent<Context>();

            if (DI.SceneContextRegistry.TryGetSceneContext(gameObject.scene, out SceneContext sceneContext))
            {
                context = sceneContext;
            }

            return context != null;
        }
    }
}
