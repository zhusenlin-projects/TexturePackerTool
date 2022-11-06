using System;
using UnityEngine;

namespace Packer
{
    /// <summary>
	/// 图集管理器.
	/// 用于维护图集对象.
	/// </summary>
    public class TextureAtlasManager : MonoBehaviour
    {
        [SerializeField] private int atlasSize = 1024;
        [SerializeField] private System.Drawing.Imaging.PixelFormat textureFormat = System.Drawing.Imaging.PixelFormat.Format32bppArgb;

        private TextureAtlas _textureAtlas;

        public TextureAtlas textureAtlas
        {
            get
            {
                if (_textureAtlas == null)
                    _textureAtlas = new TextureAtlas(atlasSize, textureFormat);
                return _textureAtlas;
            }
        }

        private static TextureAtlasManager _instance;

        //图集管理器单例
        public static TextureAtlasManager instance
        {
            get
            {
                if(_instance == null)
                {
                    _instance = FindObjectOfType<TextureAtlasManager>();
                    if(_instance == null )
                    {
                        var name = "TextureAtlasManager";
                        var go = GameObject.Find(name);
                        if (go == null)
                            go = new GameObject(name);

                        _instance = go.AddComponent<TextureAtlasManager>();
                    }
                }
                return _instance;
            }
        }

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Debug.LogWarning("TextureAtlasManager已存在，请勿重复创建!");
                Destroy(this);
                return;
            }

            _instance = this;
        }

        private void OnDestroy()
        {
            _textureAtlas?.Destroy();
            _textureAtlas = null;

            if (_instance == this)
                _instance = null;
        }

        public void NewSubTexture(SubTexture agent, System.Drawing.Image img, int width, int height, Action<TextureAtlas, RectInt> onSuccess)
        {
            textureAtlas.NewSubTexture(agent, img, width, height,onSuccess);
        }
    }
}
