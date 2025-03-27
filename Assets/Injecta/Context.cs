using System;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Injecta
{
    public abstract class Context : ContextBase
    {
        [SerializeField] protected ScriptableObjectInstaller[] scriptableObjectInstallers;
        [SerializeField] protected MonoInstaller[] monoInstallers;
        [SerializeField] protected MonoInstaller[] prefabInstallers;

        private Transform _prefabResolverContainer;

        public Container Container { get; private set; }

        protected void Awake()
        {
            Initialize();
            Container = GetContainer();
            InstallBindings();
            ResolveBindings();
        }

        protected virtual void Initialize() { }
        protected abstract Container GetContainer();
        protected abstract GameObject[] GetRoots();

        protected void InstallBindings()
        {
            InstallScriptableObjects();
            InstallBindings(monoInstallers);
            InstallPrefabs();
        }

        private void InstallScriptableObjects()
        {
            if (scriptableObjectInstallers == null)
                return;

            Array.ForEach(scriptableObjectInstallers, InstallBindings);
        }

        private void InstallPrefabs()
        {
            if (prefabInstallers == null || prefabInstallers.Length == 0)
                return;

            _prefabResolverContainer = new GameObject().transform;
            _prefabResolverContainer.SetParent(transform);
            _prefabResolverContainer.gameObject.SetActive(false);

            foreach (MonoInstaller prefabInstaller in prefabInstallers)
            {
                GameObject prefab = prefabInstaller.gameObject;
                MonoInstaller installer = Instantiate(prefab, _prefabResolverContainer).GetComponent<MonoInstaller>();
                InstallBindings(installer);
            }
        }

        protected void InstallBindings(IInstaller[] installers)
        {
            if (installers == null)
                return;

            Array.ForEach(installers, InstallBindings);
        }

        protected void InstallBindings(IInstaller installer)
        {
            // TODO: Hacer esto utilizando el propio sistema de inyección
            //installer.GetType().GetProperty(nameof(Container), BindingFlags.NonPublic | BindingFlags.Instance).SetValue(installer, Container);
            installer.Init(Container);
            installer.InstallBindings();
        }

        protected void ResolveBindings()
        {
            GameObject[] roots = GetRoots();
            MonoBehaviour[] allMonos = roots.SelectMany(r => r.GetComponentsInChildren<MonoBehaviour>(true)).ToArray();
            ResolveBindings(Container, allMonos);
            FreePrefabsInstantiated();
        }

        private void FreePrefabsInstantiated()
        {
            if (_prefabResolverContainer == null)
                return;

            for (int i = _prefabResolverContainer.childCount - 1; i >= 0; i--)
            {
                Transform child = _prefabResolverContainer.GetChild(i);

                child.SetParent(transform);
            }

            Destroy(_prefabResolverContainer.gameObject);
        }
    }
}
