// using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

public class NormalResourceManager : MonoBehaviour
{
    [SerializeField] private string PrefabPath = "Prefabs/CubePrefab";
    private GameObject loadedObject = null;

    [Button]
    private void LoadPrefab()
    {
        var res = Resources.Load<GameObject>(PrefabPath);
        
        if (res != null)
        {
            Debug.Log("成功加载资源");
            
            loadedObject = Instantiate(res, Vector3.zero, Quaternion.identity);
            loadedObject.name = "CubePrefab";
        }
        else
        {
            Debug.LogError($"无法加载资源，请检查路径: {PrefabPath} 是否正确，且资源在 Resources 文件夹内。");
        }
    }
}