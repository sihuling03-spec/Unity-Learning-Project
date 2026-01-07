using UnityEngine;
using System.IO;

public class AssetBundleLoader : MonoBehaviour
{
    void Start()
    {
        // 假设您的 AB 包名为 "sceneassets"
        string bundleName = "sceneassets"; 
        
        // 确定 AB 包的完整路径
        // Application.dataPath 指向 Assets 文件夹，所以需要返回上一级到项目根目录
        string bundlePath = Path.Combine(Application.dataPath, "../AssetBundles/" + bundleName);

        // **注意：** 在实际项目中，建议使用 Application.streamingAssetsPath 或持久化路径。
        
        // 1. 加载 AssetBundle 文件
        AssetBundle loadedBundle = AssetBundle.LoadFromFile(bundlePath);

        if (loadedBundle == null)
        {
            Debug.LogError("无法加载 AssetBundle: " + bundlePath);
            return;
        }

        // 2. 加载包内的资源
        // 这里的 PrefabName 必须是资源在 Assets 文件夹中的名称
        string prefabName = "Cube"; // 请替换为您的预制件或资源的名称
        GameObject loadedPrefab = loadedBundle.LoadAsset<GameObject>(prefabName);

        if (loadedPrefab != null)
        {
            // 3. 实例化资源到场景中
            Instantiate(loadedPrefab);
            Debug.Log("成功加载并实例化资源: " + prefabName);
        }
        else
        {
            Debug.LogError($"无法从 AssetBundle 中找到资源: {prefabName}");
        }

        // 4. 卸载 AssetBundle（注意：如果设置为 false，则不会卸载已实例化的对象）
        loadedBundle.Unload(false);
    }
}