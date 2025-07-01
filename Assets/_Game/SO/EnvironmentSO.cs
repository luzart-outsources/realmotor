namespace Luzart
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    
    [CreateAssetMenu(menuName = "SO/EnvironmentSO", fileName = "EnvironmentSO")]
    public class EnvironmentSO : ScriptableObject
    {
        private const string PATH_ENVIRONMENT = "Environment";
        public EnvironmentInforPath[] allEnvironment; 
        public string GetPath(int id)
        {
            return $"{PATH_ENVIRONMENT}/{GetPathEnvironment(id)}";
        }
        public string GetPathScene(int id)
        {
            return GetPathEnvironment(id);
        }
        private string GetPathEnvironment(int id)
        {
            for (int i = 0; i < allEnvironment.Length; i++)
            {
                if (allEnvironment[i].id == id)
                {
                    return allEnvironment[i].path;
                }
            }
            return null;
        }
    }
    [System.Serializable]
    public struct EnvironmentInforPath
    {
        public int id;
        public string path;
    }
}
