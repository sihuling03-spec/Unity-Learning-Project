using System;
using UnityEngine;
using System.Collections; // **注意：异步加载需要使用协程（IEnumerator）
using System.IO;

public class AssetBundleLoaderAsync : MonoBehaviour
{
    private void Awake()
    {
        var cameras = GameObject.FindWithTag("MainCamera");
        var obj= GameObject.Find("GameObject");
        var tag = obj.tag;
    }

    void Start()
    {
        StartCoroutine(LoadBundleAndAssetAsync());
    }

    IEnumerator LoadBundleAndAssetAsync()
    {
        string bundleName = "sceneassets"; 
        string prefabName = "YourPrefabName"; // 替换为您的资源名称
        
        // 1. 构建路径
        // 在实际移动设备上，通常使用 Application.streamingAssetsPath 或 Application.persistentDataPath
        string bundlePath = Path.Combine(Application.dataPath, "../AssetBundles/" + bundleName);

        Debug.Log($"开始异步加载 AssetBundle 文件: {bundlePath}");

        // --- 第一步：异步加载 AssetBundle 文件容器 ---
        // 使用 LoadFromFileAsync，它返回一个 AssetBundleCreateRequest
        AssetBundleCreateRequest bundleRequest = AssetBundle.LoadFromFileAsync(bundlePath);

        // 使用 yield return 等待加载完成，同时让主线程继续渲染
        yield return bundleRequest; 

        AssetBundle loadedBundle = bundleRequest.assetBundle;

        if (loadedBundle == null)
        {
            Debug.LogError("异步加载 AssetBundle 失败，请检查路径和文件名！");
            yield break; // 退出协程
        }

        Debug.Log("AssetBundle 文件容器加载完成，开始加载内部资源...");

        // --- 第二步：异步加载包内的具体资源 ---
        // 使用 LoadAssetAsync，它返回一个 AssetBundleRequest
        AssetBundleRequest assetRequest = loadedBundle.LoadAssetAsync<GameObject>(prefabName);

        // 使用 yield return 等待加载完成
        // 您可以在这里添加一个 while 循环来实时显示 assetRequest.progress
        yield return assetRequest;

        GameObject loadedPrefab = assetRequest.asset as GameObject;

        if (loadedPrefab != null)
        {
            // 3. 实例化资源
            Instantiate(loadedPrefab);
            Debug.Log("成功异步加载并实例化资源: " + prefabName);
        }
        else
        {
            Debug.LogError($"无法从 AssetBundle 中找到资源: {prefabName}");
        }

        // 4. 卸载 AssetBundle
        // 注意：Unload(false) 卸载容器，保留已实例化的对象
        loadedBundle.Unload(false);
    }
}