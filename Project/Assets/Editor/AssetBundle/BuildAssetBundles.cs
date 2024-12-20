using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.Asset
{
    public static partial class BuildAssetBundles
    {
        /// <summary>
        /// 打包之前执行
        /// </summary>
        public static void BuildPre()
        {
            /*
            //回滚数据
            for (int i = 0; i < AssetConfigController.Instance.Groups.Length; i++)
            {
                BundleGroup singleGroup = AssetConfigController.Instance.Groups[i];
                string atomicPath = $"{FilePathTools.assetbundlePatchPath}/Atomic{singleGroup.GroupName}.txt";
                if (!File.Exists(atomicPath))
                {
                    continue;
                }
                List<string> paths = singleGroup.GetBundlePaths(true);
                if (paths != null && paths.Count > 0)
                {
                    foreach (var p in paths)
                    {
                        string localPath = $"{FilePathTools.assetbundlePatchPath}/{p.ToLower()}";
                        string backLocalPatch = $"{FilePathTools.assetbundlePatchPath}_Back/{p.ToLower()}";
                        if (!File.Exists(backLocalPatch + ".ab") ||
                            !File.Exists(backLocalPatch + ".ab.back") ||
                            !File.Exists(backLocalPatch + ".ab.manifest") ||
                            !File.Exists(backLocalPatch + ".ab.patch"))
                        {
                            continue;//尽可能还原，走到这里可能为：刚好打包配置迭代了。
                        }
                        File.Copy(backLocalPatch + ".ab", localPath + ".ab", true);
                        File.Copy(backLocalPatch + ".ab.back", localPath + ".ab.back", true);
                        File.Copy(backLocalPatch + ".ab.manifest", localPath + ".ab.manifest", true);
                        File.Copy(backLocalPatch + ".ab.patch", localPath + ".ab.patch", true);
                    }
                }
                File.Delete(atomicPath);
            }

            if (!AssetConfigController.Instance.IsPathsNoRepetition())
            {
                throw new Exception("Duplicate path in configuration");
            }

            MarkFilesMD5ForBuildAssetbundle();

            MarkAllHashProfile();

            MarkSpriteAtlasName();

            SpriteAtlasUtility.PackAllAtlases(EditorUserBuildSettings.activeBuildTarget, false);

            CheckPatch(FilePathTools.assetbundlePatchPath);

            string backPath = FilePathTools.assetbundlePatchPath + "_Back";
            if (Directory.Exists(backPath))
            {
                DirectoryInfo dir = new DirectoryInfo(backPath);
                dir.Empty();
            }

            //back patch
            if (Directory.Exists(FilePathTools.assetbundlePatchPath))
            {
                if (!Directory.Exists(backPath))
                {
                    Directory.CreateDirectory(backPath);
                }
                FilePathTools.CopyDirectory(FilePathTools.assetbundlePatchPath, backPath);
            }

            Debug.Log("BuildPre Finish!!!");
            */
        }
    }
}