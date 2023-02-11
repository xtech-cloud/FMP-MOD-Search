

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
using Newtonsoft.Json;

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
                var rt = uiReference_.input.GetComponent<RectTransform>();
                rt.anchoredPosition = new Vector2(0, -style_.input.marginTop);
                rt.sizeDelta = new Vector2(style_.input.width, style_.input.height);
                uiReference_.input.transform.Find("Placeholder").GetComponent<Text>().fontSize = style_.input.fontSize;
                uiReference_.input.transform.Find("Text").GetComponent<Text>().fontSize = style_.input.fontSize;
                uiReference_.input.transform.Find("icon").GetComponent<RectTransform>().sizeDelta = new Vector2(style_.input.iconSize, style_.input.iconSize);
            }

            // 应用键盘样式
            {
                var rtKeyboard = rootUI.transform.Find("keyboard").GetComponent<RectTransform>();
                rtKeyboard.sizeDelta = new Vector2(style_.keyboard.width, style_.keyboard.height);
                uiReference_.keyS.AddRange(rootUI.transform.Find("keyboard").GetComponentsInChildren<Button>());
                foreach (var key in uiReference_.keyS)
                {
                    key.transform.Find("Text").GetComponent<Text>().fontSize = style_.keyboard.fontSize;
                }
                rootUI.transform.Find("keyboard/line1").GetComponent<GridLayoutGroup>().cellSize = new Vector2(style_.keyboard.keySize, style_.keyboard.keySize);
                rootUI.transform.Find("keyboard/line2").GetComponent<GridLayoutGroup>().cellSize = new Vector2(style_.keyboard.keySize, style_.keyboard.keySize);
                rootUI.transform.Find("keyboard/line3").GetComponent<GridLayoutGroup>().cellSize = new Vector2(style_.keyboard.keySize, style_.keyboard.keySize);
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
                var rtRecords = rootUI.transform.Find("svRecords").GetComponent<RectTransform>();
                rtRecords.sizeDelta = new Vector2(-(style_.result.margin.left + style_.result.margin.right), -(style_.result.margin.top + style_.result.margin.bottom));

                var layout = rootUI.transform.Find("svRecords/Viewport/Content").GetComponent<GridLayoutGroup>();
                layout.cellSize = new Vector2(style_.record.width, style_.record.height);
                uiReference_.recordTemplate = rootUI.transform.Find("svRecords/Viewport/Content/record").GetComponent<RectTransform>();
                uiReference_.recordTemplate.gameObject.SetActive(false);
                var rtIcon = uiReference_.recordTemplate.Find("icon").GetComponent<RectTransform>();
                rtIcon.sizeDelta = new Vector2(style_.record.iconSize, style_.record.iconSize);
                rtIcon.anchoredPosition = new Vector2(style_.record.iconMarginLeft, 0);
                var color = convertColor(style_.record.color);
                var rtText = uiReference_.recordTemplate.Find("Text").GetComponent<RectTransform>();
                rtText.GetComponent<Text>().fontSize = style_.record.fontSize;
                rtText.sizeDelta = new Vector2(-(style_.record.textMargin.left + style_.record.textMargin.right), -(style_.record.textMargin.top + style_.record.textMargin.bottom));
                rtText.anchoredPosition = new Vector2(calculateAnchoredValue(style_.record.textMargin.right, style_.record.textMargin.left), calculateAnchoredValue(style_.record.textMargin.top, style_.record.textMargin.bottom));

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

            var initialsS = preloadsRepetition["ContentInitialsS"] as Dictionary<string, string>;

            IEnumerable<KeyValuePair<string, string>> pairItor = from r in initialsS where r.Value.ToLower().Contains(_key.ToLower()) select r;
            var pairS = pairItor.ToList();
            for (int i = 0; i < style_.record.capacity && i < pairS.Count; i++)
            {
                var uri = pairS[i].Key;
                // 从预加载中获取meta的json
                var metaKey = uri + "/meta.json";
                object metaValue;
                if (preloadsRepetition.TryGetValue(metaKey, out metaValue))
                {
                    var contentSchema = JsonConvert.DeserializeObject<ContentMetaSchema>(metaValue as string);
                    var clone = GameObject.Instantiate(uiReference_.recordTemplate, uiReference_.recordTemplate.parent);
                    clone.transform.Find("Text").GetComponent<Text>().text = contentSchema.alias;
                    clone.gameObject.SetActive(true);
                    clone.GetComponent<Button>().onClick.AddListener(() =>
                    {
                        Dictionary<string, object> variableS = new Dictionary<string, object>();
                        variableS["{{uid}}"] = uid;
                        publishSubjects(style_.activateSubjects, variableS);
                    });

                    // 从预加载中获取icon图片
                    var iconKey = uri + "/icon.png";
                    object iconValue;
                    if (preloadsRepetition.TryGetValue(iconKey, out iconValue))
                    {
                        clone.transform.Find("icon").GetComponent<RawImage>().texture = iconValue as Texture2D;
                    }
                }

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

        /// <summary>
        /// 计算锚点值
        /// </summary>
        /// <param name="_positiveAxisValue">正方向轴上的边距值</param>
        /// <param name="_negativeAxisValue">负方向上轴的边距值</param>
        /// <returns></returns>
        private float calculateAnchoredValue(int _positiveAxisMargin, int _negativeAxisMargin)
        {
            // 如果正方向轴上的边距值大于负方向轴上的边距值，则锚点中心位于负方向轴上
            if (_positiveAxisMargin > _negativeAxisMargin)
            {
                return -(_positiveAxisMargin - _negativeAxisMargin) / 2.0f;

            }
            // 如果正方向轴上的边距值大于负方向轴上的边距值，则锚点中心位于负方向轴上
            else
            {
                return (_negativeAxisMargin - _positiveAxisMargin) / 2.0f;
            }
        }

    }
}
