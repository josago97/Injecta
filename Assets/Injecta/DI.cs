namespace Injecta
{
    internal static class DI
    {
        public static SceneContextRegistry SceneContextRegistry { get; } = new SceneContextRegistry();

        public static bool TryGetParentContext(ContextBase contextBase, out Context context)
        {
            context = contextBase.GetComponentInParent<Context>();

            if (SceneContextRegistry.TryGetSceneContext(contextBase.gameObject.scene, out SceneContext sceneContext))
            {
                context = sceneContext;
            }

            return context != null;
        }

        public static bool TryGetParentContainer(ContextBase contextBase, out Container container)
        {
            container = null;

            if (TryGetParentContext(contextBase, out Context parentContext))
            {
                container = parentContext.Container;
            }

            return container != null;
        }
    }
}
