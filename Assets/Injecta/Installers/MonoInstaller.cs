using UnityEngine;

namespace Injecta
{
    public abstract class MonoInstaller : MonoBehaviour, IInstaller
    {
        [Inject]
        protected Container Container { get; private set;}

        public abstract void InstallBindings();
    }
}