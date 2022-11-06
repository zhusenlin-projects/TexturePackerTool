using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Packer
{
    public class ResListManager : MonoBehaviour
    {
        public Transform itemRoot;
        public GameObject itemTemplate;
        public InputField searchKeywordInputField;
        public Button startSearchBtn;
        public Text searchLog;

        [Space(5)]
        public MainProcess mainProcess;


        private Dictionary<string, GameObject> mResList = null;
        private const string searchLogFormat="共{0}项，已筛选{1}项";

        private void Awake()
        {
            startSearchBtn.onClick.AddListener(() =>
            {
                int activeNum = 0;
                foreach(var item in mResList)
                {
                    if (item.Key.Contains(searchKeywordInputField.text.Trim()))
                    {
                        item.Value.SetActive(true);
                        activeNum++;
                    }
                    else
                        item.Value.SetActive(false);
                }
                searchLog.text = string.Format(searchLogFormat, mResList.Count, activeNum);
            });
        }

        /// <summary>
        /// 更新列表
        /// </summary>
        /// <param name="infos"></param>
        public void UpdateList(List<ResInfo> infos)
        {
            if(mResList == null)
                mResList = new Dictionary<string, GameObject>();
            mResList.Clear();

            foreach (var info in infos)
            {
                GameObject newItem = GameObject.Instantiate(itemTemplate, itemRoot);
                Button button = newItem.GetComponent<Button>();
                Text   text = newItem.GetComponentInChildren<Text>();
                if (text != null)
                    text.text = info.filename;
                if (button != null)
                {
                    button.onClick.AddListener(() =>
                    {
                        mainProcess?.UpdateTextureSamplers(info.transformInfo);
                        mainProcess?.UpdateCurrentSubTextureLog(info);
                    });
                }
                mResList.Add(info.filename, newItem);
            }
            searchLog.text=string.Format(searchLogFormat, mResList.Count, mResList.Count);
        }
    }
}