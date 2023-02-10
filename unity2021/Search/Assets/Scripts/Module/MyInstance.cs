

using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using LibMVCS = XTC.FMP.LIB.MVCS;
using XTC.FMP.MOD.Search.LIB.Proto;
using XTC.FMP.MOD.Search.LIB.MVCS;
using System;
using System.Linq;
using System.IO;

namespace XTC.FMP.MOD.Search.LIB.Unity
{
    /// <summary>
    /// 实例类
    /// </summary>
    public class MyInstance : MyInstanceBase
    {
        public class UiReference
        {
            public InputField input;
            public RectTransform recordTemplate;
            public List<Button> keyS = new List<Button>();

            public Color primaryColor = Color.white;
        }

        private UiReference uiReference_ = new UiReference();
        private List<DummyModel.Record> records_ = new List<DummyModel.Record>();
        private ContentReader contentRender_ = null;

        public MyInstance(string _uid, string _style, MyConfig _config, MyCatalog _catalog, LibMVCS.Logger _logger, Dictionary<string, LibMVCS.Any> _settings, MyEntryBase _entry, MonoBehaviour _mono, GameObject _rootAttachments)
            : base(_uid, _style, _config, _catalog, _logger, _settings, _entry, _mono, _rootAttachments)
        {
        }

        /// <summary>
        /// 当被创建时
        /// </summary>
        /// <remarks>
        /// 可用于加载主题目录的数据
        /// </remarks>
        public void HandleCreated()
        {
            Func<string, Color> convertColor = (_color) =>
            {
                Color color = Color.white;
                ColorUtility.TryParseHtmlString(_color, out color);
                return color;
            };
            uiReference_.primaryColor = convertColor(style_.primaryColor);

            // 应用背景样式
            {
                var color = convertColor(style_.background.color);
                var background = rootUI.transform.Find("Background").GetComponent<RawImage>();
                background.color = color;
                background.gameObject.SetActive(style_.background.visible);
            }

            // 应用输入框样式
            {
                uiReference_.input = rootUI.transform.Find("InputField").GetComponent<InputField>();
                uiReference_.input.GetComponent<Image>().color = uiReference_.primaryColor;
                uiReference_.input.transform.Find("icon").GetComponent<Image>().color = uiReference_.primaryColor;
            }

            // 应用按钮样式
            {
                uiReference_.keyS.AddRange(rootUI.transform.Find("keyboard").GetComponentsInChildren<Button>());
                if (!string.IsNullOrEmpty(style_.keyboard.keyImage))
                {
                    loadTextureFromTheme(style_.keyboard.keyImage, (_texture) =>
                    {
                        Sprite sprite = Sprite.Create(_texture, new Rect(0, 0, _texture.width, _texture.height), new Vector2(0.5f, 0.5f));
                        foreach (Button key in uiReference_.keyS)
                        {
                            key.GetComponent<Image>().sprite = sprite;
                        }
                    }, () =>
                    {
                    });
                }
            }

            // 应用结果样式
            {
                var layout = rootUI.transform.Find("svRecords/Viewport/Content").GetComponent<GridLayoutGroup>();
                layout.cellSize = new Vector2(style_.record.width, style_.record.height);
                uiReference_.recordTemplate = rootUI.transform.Find("svRecords/Viewport/Content/record").GetComponent<RectTransform>();
                uiReference_.recordTemplate.gameObject.SetActive(false);
                var color = convertColor(style_.record.color);
                uiReference_.recordTemplate.GetComponent<Image>().color = color;
                loadTextureFromTheme(style_.record.iconImage, (_texture) =>
                {
                    uiReference_.recordTemplate.Find("icon").GetComponent<RawImage>().texture = _texture;
                }, () => { });
            }

            // 绑定按钮事件
            foreach (Button key in uiReference_.keyS)
            {
                key.onClick.AddListener(() =>
                {
                    if (key.gameObject.name.Equals("back"))
                    {
                        uiReference_.input.text = uiReference_.input.text.Length > 0 ? uiReference_.input.text.Substring(0, uiReference_.input.text.Length - 1) : "";
                    }
                    else if (key.gameObject.name.Equals("clear"))
                    {
                        uiReference_.input.text = "";
                    }
                    else if (uiReference_.input.text.Length < 24)
                    {
                        uiReference_.input.text += key.gameObject.name;
                    }
                    else
                    {
                        return;
                    }
                    submitSearch(uiReference_.input.text);
                });
            }
        }

        /// <summary>
        /// 当被删除时
        /// </summary>
        public void HandleDeleted()
        {
        }

        /// <summary>
        /// 当被打开时
        /// </summary>
        /// <remarks>
        /// 可用于加载内容目录的数据
        /// </remarks>
        public void HandleOpened(string _source, string _uri)
        {
            contentRender_ = new ContentReader(contentObjectsPool);
            contentRender_.AssetRootPath = settings_["path.assets"].AsString();
            rootUI.gameObject.SetActive(true);
            rootWorld.gameObject.SetActive(true);
            clearResults();
        }

        /// <summary>
        /// 当被关闭时
        /// </summary>
        public void HandleClosed()
        {
            rootUI.gameObject.SetActive(false);
            rootWorld.gameObject.SetActive(false);
            contentRender_ = null;
        }

        /// <summary>
        /// 绑定内容，同时生成首字母
        /// </summary>
        /// <param name="_contents">内容列表，key是uri，value是alias</param>
        public void BindContents(Dictionary<string, string> _contents)
        {
            foreach (var pair in _contents)
            {
                var record = new DummyModel.Record();
                record.uri = pair.Key;
                record.alias = pair.Value;
                record.initials = NPinyin.Pinyin.GetInitials(pair.Value);
                records_.Add(record);
            }
        }

        public void CleanResults()
        {
            uiReference_.input.text = "";
            clearResults();
        }

        private void submitSearch(string _key)
        {
            clearResults();

            if (string.IsNullOrWhiteSpace(_key))
            {
                return;
            }

            IEnumerable<DummyModel.Record> recordsItr = from r in records_ where r.initials.ToLower().Contains(_key.ToLower()) select r;
            var records = recordsItr.ToList();
            for (int i = 0; i < style_.record.capacity && i < records.Count; i++)
            {
                var record = records[i];
                var clone = GameObject.Instantiate(uiReference_.recordTemplate, uiReference_.recordTemplate.parent);
                clone.transform.Find("Text").GetComponent<Text>().text = record.alias;
                clone.gameObject.SetActive(true);
                contentRender_.ContentUri = record.uri;
                contentRender_.LoadTexture("icon.png", (_texture) =>
                {
                    if (null == clone)
                        return;
                    clone.transform.Find("icon").GetComponent<RawImage>().texture = _texture;
                }, () =>
                {

                });

                clone.GetComponent<Button>().onClick.AddListener(() =>
                {
                    Dictionary<string, object> variableS = new Dictionary<string, object>();
                    variableS["{{uid}}"] = uid;
                    publishSubjects(style_.activateSubjects, variableS);
                });
            }
        }

        private void clearResults()
        {
            List<GameObject> records = new List<GameObject>();
            // 跳过模板
            for (int i = 1; i < uiReference_.recordTemplate.parent.childCount; ++i)
            {
                records.Add(uiReference_.recordTemplate.parent.GetChild(i).gameObject);
            }
            foreach (var go in records)
            {
                UnityEngine.GameObject.Destroy(go);
            }
        }

    }
}
