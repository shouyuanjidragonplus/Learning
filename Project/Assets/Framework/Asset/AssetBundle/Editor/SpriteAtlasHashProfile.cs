using UnityEngine;
using UnityEditor;
using UnityEditor.U2D;
using UnityEngine.U2D;

namespace Framework.Asset
{
    [System.Serializable]
    public class SpriteAtlasHashProfile
    {
        public int type; //0-master; 1-variant
        public string master_atlas_guid;
        public bool include_in_build;
        public string variant_scale;

        public bool allow_rotation;
        public bool tight_packing;
        public int block_offset;
        public int padding;

        public bool read_write_enabled;
        public bool generate_mip_maps;
        public bool srgb;
        public FilterMode filter_model;

        public int default_max_texture_size;
        public TextureImporterFormat default_format;
        public TextureImporterCompression default_texture_compression;
        public bool use_crunch_compression;
        public int crunch_compress_quality;

        public bool standalone_override;
        public int standalone_max_texture_size;
        public TextureImporterFormat standalone_format;
        public int standalone_compress_quality;

        public bool ios_override;
        public int ios_max_texture_size;
        public TextureImporterFormat ios_format;
        public int ios_compress_quality;

        public bool android_override;
        public int android_max_texture_size;
        public TextureImporterFormat android_format;
        public int android_compress_quality;

        public string[] packing_guid;

        public SpriteAtlasHashProfile(string path)
        {
            var atlas = AssetDatabase.LoadAssetAtPath<SpriteAtlas>(path);

            type = atlas.isVariant ? 1 : 0;
            master_atlas_guid = "";
            var serObj = new UnityEditor.SerializedObject(atlas);
            SerializedProperty property = serObj.FindProperty("m_MasterAtlas");
            if (null != property)
            {
                UnityEngine.Object refObj = property.objectReferenceValue;
                if (null != refObj)
                {
                    string guid;
                    long file;
                    if (AssetDatabase.TryGetGUIDAndLocalFileIdentifier(refObj, out guid, out file))
                    {
                        master_atlas_guid = guid;
                    }
                }
            }

            property = serObj.FindProperty("m_EditorData.bindAsDefault");
            if (null != property)
            {
                include_in_build = property.boolValue;
            }

            property = serObj.FindProperty("m_EditorData.variantMultiplier");
            if (null != property)
            {
                variant_scale = property.floatValue.ToString("#0.00");
            }

            var packingSettings = atlas.GetPackingSettings();
            allow_rotation = packingSettings.enableRotation;
            tight_packing = packingSettings.enableTightPacking;
            block_offset = packingSettings.blockOffset;
            padding = packingSettings.padding;

            var textureSettings = atlas.GetTextureSettings();
            read_write_enabled = textureSettings.readable;
            generate_mip_maps = textureSettings.generateMipMaps;
            srgb = textureSettings.sRGB;
            filter_model = textureSettings.filterMode;

            TextureImporterPlatformSettings defaultPlatformSettings = atlas.GetPlatformSettings("DefaultTexturePlatform");
            default_max_texture_size = defaultPlatformSettings.maxTextureSize;
            default_format = defaultPlatformSettings.format;
            default_texture_compression = defaultPlatformSettings.textureCompression;
            use_crunch_compression = defaultPlatformSettings.crunchedCompression;
            crunch_compress_quality = defaultPlatformSettings.compressionQuality;

            TextureImporterPlatformSettings standalonePlatformSettings = atlas.GetPlatformSettings("Standalone");
            standalone_override = standalonePlatformSettings.overridden;
            standalone_max_texture_size = standalonePlatformSettings.maxTextureSize;
            standalone_format = standalonePlatformSettings.format;
            standalone_compress_quality = standalonePlatformSettings.compressionQuality;

            TextureImporterPlatformSettings iosPlatformSettings = atlas.GetPlatformSettings("iPhone");
            ios_override = iosPlatformSettings.overridden;
            ios_max_texture_size = iosPlatformSettings.maxTextureSize;
            ios_format = iosPlatformSettings.format;
            ios_compress_quality = iosPlatformSettings.compressionQuality;

            TextureImporterPlatformSettings androidPlatformSettings = atlas.GetPlatformSettings("Android");
            android_override = androidPlatformSettings.overridden;
            android_max_texture_size = androidPlatformSettings.maxTextureSize;
            android_format = androidPlatformSettings.format;
            android_compress_quality = androidPlatformSettings.compressionQuality;

            UnityEngine.Object[] objs = atlas.GetPackables();
            packing_guid = new string[objs.Length];
            for (int i = 0; i < objs.Length; i++)
            {
                string guid;
                long file;
                if (null != objs[i] && AssetDatabase.TryGetGUIDAndLocalFileIdentifier(objs[i], out guid, out file))
                {
                    packing_guid[i] = guid;
                }
                else
                {
                    packing_guid[i] = "";
                }
            }
        }
    }
}