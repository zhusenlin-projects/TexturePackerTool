using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace Packer
{
    public class GenerateAndLoadUI : MonoBehaviour
    {
        public FileManager fileManager;
        public InputField targetFolderPathFeild;
        public Button generateAtlasButton;
        public Button loadAtlasButton;
        public Text pathText;

        [Space(5)]
        public MainProcess mainProcess;

        private void Start()
        {
            generateAtlasButton.onClick.AddListener(() =>
            {
                string saveFilePath = fileManager.SaveFile();
                string targetForder = targetFolderPathFeild.text.Trim();
                if (saveFilePath != null && Directory.Exists(targetForder))
                {
                    //生成图集并加载相关信息
                    mainProcess.StartProcess(targetForder, saveFilePath);
                }
            });

            loadAtlasButton.onClick.AddListener(() =>
            {
                string xmlPath = fileManager.OpenFile("");
                mainProcess.LoadFromXml(xmlPath);
            });
        }
    }
}
