using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Framework.DataManager;
using System.Drawing;

namespace Packer
{
    

    public class MainProcess : MonoBehaviour
    {

        private string rootFolderPath;
        private string outputFilePath;
        private string outputXmlPath;
        [SerializeField] private List<ResInfo> resList=new List<ResInfo>();

        [Space(5)]
        [SerializeField] private TextureSampler ts1;
        [SerializeField] private TextureSampler ts2;
        [SerializeField] private ResListManager resListMsg;
        [SerializeField] private CurrentSubTextureLog log;

        [SerializeField] private UVTexcoordType uvTexcoordType= UVTexcoordType.LeftButtom;

        private void Start()
        {
            CommonConfig config = CommonConfig.LoadConfigFronFile();
            if(config!=null)
            {
                if(File.Exists(config.lastestOpenPathFile))
                    LoadFromXml(config.lastestOpenPathFile);
                uvTexcoordType = config.uVTexcoordType;
            }
        }

        /// <summary>
        /// 开始处理
        /// </summary>
        public void StartProcess(string targetFolder,string outputPath)
        {
            rootFolderPath = targetFolder;
            outputFilePath = $"{outputPath}.png";
            outputXmlPath = $"{outputPath}.xml";

            RefreshResourceList();

            int i = 0;
            //遍历所有资源
            foreach (var res in resList)
            {
                res.id = i++;
                SubTexture subTex = new SubTexture(res.filepath);
                subTex.PackTexture();
                res.transformInfo.Copy(subTex.position);
                if (uvTexcoordType == UVTexcoordType.LeftButtom)
                    res.transformInfo.posy = 2048 - subTex.position.y - subTex.position.height;
                subTex.Destroy();
            }

            //输出到磁盘
            TextureAtlasManager.instance.textureAtlas.Save(outputFilePath);
            XmlUtil.Serialize(resList, outputXmlPath);
            UpdateTexureSamplersInScene(0);
            resListMsg?.UpdateList(resList);
        }

        //从Xml中更新数据
        public void LoadFromXml(string xmlFile)
        {
            if (!File.Exists(xmlFile))
                return;

            outputXmlPath = xmlFile;
            if (resList == null)
                resList = new List<ResInfo>();
            resList.Clear();
            resList = XmlUtil.DeserializeFromFile<List<ResInfo>>(xmlFile);
            resListMsg?.UpdateList(resList);
            string fileName = Path.GetFileNameWithoutExtension(xmlFile);
            string picPath=$"{Path.GetDirectoryName(xmlFile)}\\{fileName}.png";
            outputFilePath = picPath;
            if(File.Exists(picPath))
            {
                Texture texture = LoadLocalFileToTexture(picPath);
                ts1?.UpdateTexture(texture, resList[0].transformInfo);
                ts2?.UpdateTexture(texture, resList[0].transformInfo);
                log?.UpdateLog(resList[0], picPath);
            }
        }

        /// <summary>
        /// 更新采样
        /// </summary>
        public void UpdateTextureSamplers(TransformInfo subTexRect)
        {
            ts1.UpdateTexture(subTexRect);
            ts2.UpdateTexture(subTexRect);
        }

        /// <summary>
        /// 更新当前子图的log信息
        /// </summary>
        public void UpdateCurrentSubTextureLog(ResInfo info)
        {
            log?.UpdateLog(info, outputFilePath);
        }

        /// <summary>
        /// 更新场景中的采样展示UI
        /// </summary>
        private void UpdateTexureSamplersInScene(int idx)
        {
            Texture2D texture = LoadLocalFileToTexture(outputFilePath);
            ts1?.UpdateTexture(texture, resList[idx].transformInfo);
            ts2?.UpdateTexture(texture, resList[idx].transformInfo);
            log?.UpdateLog(resList[idx], outputFilePath);
        }


        private void RefreshResourceList()
        {
            _GetAllFiles(new DirectoryInfo(rootFolderPath));
            string log = "";
            for (int i = 0; i < resList.Count; i++)
                log += $"[{i + 1}] {resList[i].filepath}\n";
            Debug.Log(log);

        }

        private void _GetAllFiles(DirectoryInfo dir)
        {
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo fi in files)
            {
                resList.Add(new ResInfo { filename = fi.Name, filepath = fi.FullName });
            }
            DirectoryInfo[] allDir = dir.GetDirectories();
            foreach (DirectoryInfo d in allDir)
                _GetAllFiles(d);
        }

        public Texture2D LoadLocalFileToTexture(string path)
        {
            if (!File.Exists(path))
            {
                Debug.LogError($"<{path}> Not Exits...");
                return null;
            }
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            System.Drawing.Image image = System.Drawing.Image.FromStream(fs);
            var buffer = new byte[fs.Length];
            fs.Position = 0;
            fs.Read(buffer, 0, buffer.Length);
            Texture2D texture = new Texture2D(image.Width, image.Height);
            texture.LoadImage(buffer);
            return texture;
        }

        private void OnDestroy()
        {
            //保存配置
            CommonConfig config=new CommonConfig();
            if(outputXmlPath!=null)
            {
                config.lastestOpenPathFile = outputXmlPath;
                config.uVTexcoordType = uvTexcoordType;
                CommonConfig.SaveConfig(config);
                Debug.Log("保存通用配置");
            }
        }
    }
}
