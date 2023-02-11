
namespace XTC.FMP.MOD.Search.LIB.Unity
{
    public class MySubject : MySubjectBase
    {
        /// <summary>
        /// 嵌入
        /// </summary>
        /// <remarks>
        /// 创建后挂载到slot中
        /// </remarks>
        /// <example>
        /// var data = new Dictionary<string, object>();
        /// data["uid"] = "default";
        /// data["style"] = "default";
        /// data["uiSlot"] = a instance of UnityEngine.GameObejct;
        /// data["worldSlot"] = a instance of UnityEngine.GameObejct;
        /// model.Publish(/XTC/Search/Inlay, data);
        /// </example>
        public const string Inlay = "/XTC/Search/Inlay";

        /// <summary>
        /// 刷新内容
        /// </summary>
        /// <example>
        /// var data = new Dictionary<string, object>();
        /// data["uid"] = "default";
        /// model.Publish(/XTC/Search/Refresh, data);
        /// </example>
        public const string Refresh = "/XTC/Search/Refresh";

        /// <summary>
        /// 清空结果
        /// </summary>
        /// <example>
        /// var data = new Dictionary<string, object>();
        /// data["uid"] = "default";
        /// model.Publish(/XTC/Search/Clean, data);
        /// </example>
        public const string Clean = "/XTC/Search/Clean";
    }
}
