using UnityEngine;

namespace Injecta
{
    public class ProjectContext : Context
    {
        private const string PREFAB_NAME = Constants.PROJECT_CONTEXT_PREFAB_NAME;

        private static ProjectContext _instance;

        public static ProjectContext Instance 
        {
            get
            {
                if (_instance == null) _instance = CreateInstance();

                return _instance;
            }
        }

        private static ProjectContext CreateInstance()
        {
            ProjectContext projectContext;
            GameObject[] prefabs = Resources.LoadAll<GameObject>(PREFAB_NAME);

            if (prefabs.Length > 0)
            {
                if (prefabs.Length > 1)
                {
                    Debug.LogWarning($"Found multiples project context prefabs at resource path '{PREFAB_NAME}'");
                }

                GameObject prefab = prefabs[0];
                projectContext = Instantiate(prefab).GetComponent<ProjectContext>();
            }
            else
            {
                projectContext = new GameObject(PREFAB_NAME).AddComponent<ProjectContext>();
            }

            return projectContext;
        }

        protected override void Initialize()
        {
            DontDestroyOnLoad(gameObject);
        }

        protected override Container GetContainer()
        {
            return new Container();
        }

        protected override GameObject[] GetRoots()
        {
            return new[] { gameObject };
        }
    }
}
