using UnityEngine;

namespace Injecta
{
    public class AutoInjecter : ContextBase
    {
        private void Awake()
        {
            if (DI.TryGetParentContext(this, out Context context))
                ResolveBindings(context.Container, GetComponentsInChildren<MonoBehaviour>(true));
        }
    }
}