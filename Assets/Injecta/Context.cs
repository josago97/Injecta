using System;
using System.Linq;
using UnityEngine;

namespace Injecta
{
    public abstract class Context : ContextBase
    {
        [SerializeField] protected MonoInstaller[] monoInstallers;
        [SerializeField] protected MonoInstaller[] prefabInstallers;
        [SerializeField] protected ScriptableObjectInstaller[] scriptableObjectInstallers;

        private Transform _prefabResolverContainer;

        public Container Container { get; protected set; }

        protected void Awake()
        {
            Debug.Log(gameObject.name);

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
            Debug.Log(gameObject.name + scriptableObjectInstallers);
            Debug.Log(gameObject.name + monoInstallers);
            Debug.Log(gameObject.name + prefabInstallers);

            InstallScriptableObjects();
            InstallBindings(monoInstallers);
            InstallPrefabs();
        }

        private void InstallScriptableObjects()
        {
            if (scriptableObjectInstallers != null)
            {
                Array.ForEach(scriptableObjectInstallers, InstallBindings);
            }
        }

        private void InstallPrefabs()
        {
            if (prefabInstallers != null && prefabInstallers.Length > 0)
            {
                _prefabResolverContainer = new GameObject().transform;
                _prefabResolverContainer.SetParent(transform);
                _prefabResolverContainer.gameObject.SetActive(false);

                for (int i = 0; i < prefabInstallers.Length; i++)
                {
                    GameObject prefab = prefabInstallers[i].gameObject;
                    MonoInstaller installer = Instantiate(prefab, _prefabResolverContainer).GetComponent<MonoInstaller>();
                    InstallBindings(installer);
                }
            }
        }

        protected void InstallBindings(IInstaller[] installers)
        {
            if (installers != null)
            {
                Array.ForEach(installers, InstallBindings);
            }
        }

        protected void InstallBindings(IInstaller installer)
        {
            installer.Init(Container);
            installer.InstallBindings();
        }

        protected void ResolveBindings()
        {
            GameObject[] roots = GetRoots();
            MonoBehaviour[] allMonos = roots.SelectMany(r => r.GetComponentsInChildren<MonoBehaviour>(true)).ToArray();
            ResolveBindings(Container, allMonos);

            // TODO: ver para que era esto
            //FreePrefabsInstantiated();
        }

        private void FreePrefabsInstantiated()
        {
            for (int i = _prefabResolverContainer.childCount - 1; i >= 0; i--)
            {
                Transform child = _prefabResolverContainer.GetChild(i);

                child.SetParent(transform);
            }

            Destroy(_prefabResolverContainer.gameObject);
        }
    }
}
