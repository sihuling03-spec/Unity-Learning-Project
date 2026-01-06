using UnityEditor;
using System.IO;

public class BuildAssetBundles
{
    [MenuItem("Assets/Build AssetBundles")] // 在 Unity 菜单中添加一个项
    static void BuildAllAssetBundles()
    {
        // 打包输出路径
        string assetBundleDirectory = "AssetBundles"; 

        // 如果路径不存在，则创建
        if(!Directory.Exists(assetBundleDirectory))
        {
            Directory.CreateDirectory(assetBundleDirectory);
        }

        // 执行打包
        BuildPipeline.BuildAssetBundles(
            assetBundleDirectory, 
            BuildAssetBundleOptions.None, 
            BuildTarget.StandaloneWindows // 替换为您需要的平台
        );
    }
}