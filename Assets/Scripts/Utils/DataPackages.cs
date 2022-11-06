using Framework.DataManager;
using System;
using System.IO;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

namespace Packer
{
    /// <summary>
    /// 子图集的位置信息
    /// </summary>
    [Serializable]
    public class TransformInfo
    {
        public int posx;
        public int posy;
        public int width;
        public int height;

        public void Copy(RectInt rect)
        {
            this.posx = rect.x;
            this.posy = rect.y;
            this.width = rect.width;
            this.height = rect.height;
        }
    }


    /// <summary>
    /// 资源信息类
    /// </summary>
    [Serializable]
    public class ResInfo
    {
        public int id { get; set; }
        public string filename { get; set; }
        public string filepath { get; set; }
        public TransformInfo transformInfo = new TransformInfo();
    }

    [Serializable]
    public class CommonConfig
    {
        //最近打开的图集配置
        public string lastestOpenPathFile { get; set; }
        //贴图坐标原点类型
        public UVTexcoordType uVTexcoordType { get; set; }

        public static void SaveConfig(CommonConfig cfig)
        {
            bool suc=XmlUtil.Serialize(cfig, Application.persistentDataPath + "/config.xml");
            if (!suc)
                Debug.LogError("Save Common Config Failded...");
        }

        public static CommonConfig LoadConfigFronFile()
        {
            string configPath = Application.persistentDataPath + "/config.xml";
            Debug.Log(configPath);
            if (File.Exists(configPath))
               return XmlUtil.DeserializeFromFile<CommonConfig>(configPath);
            else
                return null;
        }
    }


    /// <summary>
    /// UV坐标原点
    /// </summary>
    public enum UVTexcoordType
    {
        LeftTop,
        LeftButtom
    }
}