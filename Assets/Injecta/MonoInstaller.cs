using UnityEngine;

namespace Injecta
{
    public abstract class MonoInstaller : MonoBehaviour, IInstaller
    {
        protected Container Container { get; private set; }

        public void Init(Container container)
        {
            Container = container;
        }

        public abstract void InstallBindings();
    }
}