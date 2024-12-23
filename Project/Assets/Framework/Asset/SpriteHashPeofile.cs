using UnityEngine;
using UnityEditor;

namespace Framework.Asset
{
    [System.Serializable]
    public class SpriteHashPeofile
    {
        public int sprite_mode;
        public string pixels_pre_unit;
        public int sprite_mesh_type;
        public int sprite_extrude;
        public int sprite_alignment;
        public int sprite_generate_physics_shape;
        public string[] sprite_pivot;
        public string[] sprite_border;

        public bool srgb;
        public TextureImporterAlphaSource alpha_source;
        public bool alpha_is_transparency;
        public bool read_write_enable;
        public bool generate_mip_maps;
        public TextureWrapMode wrap_mode;
        public FilterMode filter_mode;
        public int aniso_level;

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

        public SpriteHashPeofile(string path)
        {
            TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
            var serObj = new UnityEditor.SerializedObject(importer);
            sprite_mode = serObj.FindProperty("m_SpriteMode").intValue;
            pixels_pre_unit = serObj.FindProperty("m_SpritePixelsToUnits").floatValue.ToString("#0.00");
            sprite_mesh_type = serObj.FindProperty("m_SpriteMeshType").intValue;
            sprite_extrude = serObj.FindProperty("m_SpriteExtrude").intValue;
            sprite_alignment = serObj.FindProperty("m_Alignment").intValue;
            sprite_generate_physics_shape = serObj.FindProperty("m_SpriteGenerateFallbackPhysicsShape").intValue;
            sprite_pivot = new string[] { importer.spritePivot.x.ToString("#0.00"), importer.spritePivot.y.ToString("#0.00") };
            sprite_border = new string[]
            {
                importer.spriteBorder.x.ToString("#0.00"), importer.spriteBorder.y.ToString("#0.00"),
                importer.spriteBorder.z.ToString("#0.00"), importer.spriteBorder.w.ToString("#0.00")
            };

            srgb = importer.sRGBTexture;
            alpha_source = importer.alphaSource;
            alpha_is_transparency = importer.alphaIsTransparency;
            read_write_enable = importer.isReadable;
            generate_mip_maps = importer.mipmapEnabled;
            wrap_mode = importer.wrapMode;
            filter_mode = importer.filterMode;
            aniso_level = importer.anisoLevel;

            TextureImporterPlatformSettings defaultPlatformSettings = importer.GetPlatformTextureSettings("DefaultTexturePlatform");
            default_max_texture_size = defaultPlatformSettings.maxTextureSize;
            default_format = defaultPlatformSettings.format;
            default_texture_compression = defaultPlatformSettings.textureCompression;
            use_crunch_compression = defaultPlatformSettings.crunchedCompression;
            crunch_compress_quality = defaultPlatformSettings.compressionQuality;

            TextureImporterPlatformSettings standalonePlatformSettings = importer.GetPlatformTextureSettings("Standalone");
            standalone_override = standalonePlatformSettings.overridden;
            standalone_max_texture_size = standalonePlatformSettings.maxTextureSize;
            standalone_format = standalonePlatformSettings.format;
            standalone_compress_quality = standalonePlatformSettings.compressionQuality;

            TextureImporterPlatformSettings iosPlatformSettings = importer.GetPlatformTextureSettings("iPhone");
            ios_override = iosPlatformSettings.overridden;
            ios_max_texture_size = iosPlatformSettings.maxTextureSize;
            ios_format = iosPlatformSettings.format;
            ios_compress_quality = iosPlatformSettings.compressionQuality;

            TextureImporterPlatformSettings androidPlatformSettings = importer.GetPlatformTextureSettings("Android");
            android_override = androidPlatformSettings.overridden;
            android_max_texture_size = androidPlatformSettings.maxTextureSize;
            android_format = androidPlatformSettings.format;
            android_compress_quality = androidPlatformSettings.compressionQuality;
        }
    }
}