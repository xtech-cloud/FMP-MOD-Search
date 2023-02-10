
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using XTC.FMP.MOD.Search.LIB.Unity;

/// <summary>
/// 根程序类
/// </summary>
/// <remarks>
/// 不参与模块编译，仅用于在编辑器中开发调试
/// </remarks>
public class Root : RootBase
{
    public GameObject __inlayUiSlot;
    public GameObject __inlayWorldSlot;

    private Dictionary<string, string> __contentS = new Dictionary<string, string>();

    private void Awake()
    {
        doAwake();

        string assetsPath = settings_["path.assets"].AsString();
        foreach (var file in Directory.GetFiles(assetsPath, "meta.json", SearchOption.AllDirectories))
        {
            var json = File.ReadAllText(file);
            var meta = JsonConvert.DeserializeObject<ContentMetaSchema>(json);
            if (string.IsNullOrEmpty(meta.foreign_bundle_uuid))
                continue;
            if (string.IsNullOrEmpty(meta.Uuid))
                continue;
            __contentS[meta.foreign_bundle_uuid + "/" + meta.Uuid] = meta.alias;
        }
    }

    private void Start()
    {
        entry_.__DebugPreload(exportRoot);
    }

    private void OnDestroy()
    {
        doDestroy();
    }

    private void OnGUI()
    {
        if (GUI.Button(new Rect(0, 0, 60, 30), "Create"))
        {
            entry_.__DebugCreate("test", "default", "", "");
        }

        if (GUI.Button(new Rect(0, 30, 60, 30), "Open"))
        {
            entry_.__DebugOpen("test", "file", "", 0.5f);
        }

        if (GUI.Button(new Rect(0, 60, 60, 30), "Show"))
        {
            entry_.__DebugShow("test", 0.5f);
        }

        if (GUI.Button(new Rect(0, 90, 60, 30), "Hide"))
        {
            entry_.__DebugHide("test", 0.5f);
        }

        if (GUI.Button(new Rect(0, 120, 60, 30), "Close"))
        {
            entry_.__DebugClose("test", 0.5f);
        }

        if (GUI.Button(new Rect(0, 150, 60, 30), "Delete"))
        {
            entry_.__DebugDelete("test");
        }

        if (GUI.Button(new Rect(0, 180, 60, 30), "Inlay"))
        {
            entry_.__DebugInlay("test", "default", __inlayUiSlot, __inlayWorldSlot, __contentS);
        }
    }
}

