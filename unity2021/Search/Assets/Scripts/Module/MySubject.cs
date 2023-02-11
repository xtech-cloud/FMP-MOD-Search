
namespace XTC.FMP.MOD.Search.LIB.Unity
{
    public class MySubject : MySubjectBase
    {
        /// <summary>
        /// Ƕ��
        /// </summary>
        /// <remarks>
        /// ��������ص�slot��
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
        /// ˢ������
        /// </summary>
        /// <example>
        /// var data = new Dictionary<string, object>();
        /// data["uid"] = "default";
        /// model.Publish(/XTC/Search/Refresh, data);
        /// </example>
        public const string Refresh = "/XTC/Search/Refresh";

        /// <summary>
        /// ��ս��
        /// </summary>
        /// <example>
        /// var data = new Dictionary<string, object>();
        /// data["uid"] = "default";
        /// model.Publish(/XTC/Search/Clean, data);
        /// </example>
        public const string Clean = "/XTC/Search/Clean";
    }
}
