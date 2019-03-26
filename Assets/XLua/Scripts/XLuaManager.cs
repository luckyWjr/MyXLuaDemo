using System;
using System.Collections;
using System.IO;
using System.Linq;
using UnityEngine;
using XLua;

public class XLuaManager : IDisposable {
    AssetBundle m_bundle;

    static XLuaManager m_instance;
    public static XLuaManager instance {
        get { return m_instance ?? (m_instance = new XLuaManager()); }
    }

    public LuaEnv LuaEnv {
        private set;
        get;
    }

    #region static

    /// <summary>
    /// 获取指定游戏对象上挂的lua组件
    /// </summary>
    public static LuaTable GetLuaComponent(Transform transform) {
        if(transform == null) {
            Debug.LogError("Argument null exception, transform == null");
            return null;
        }

        var loaders = transform.GetComponents<LuaComponentLoader>();
        var rightLoader = loaders.FirstOrDefault(lt => lt.luaTable != null);

        return rightLoader != null ? rightLoader.luaTable : null;
    }

    /// <summary>
    /// 获取指定游戏对象上挂的lua组件
    /// </summary>
    public static LuaTable GetLuaComponent(GameObject gameObject) {
        return GetLuaComponent(gameObject.transform);
    }

    /// <summary>
    /// 获取指定游戏对象上挂的lua组件
    /// </summary>
    /// <param name="type">lua组件表名</param>
    public static LuaTable GetLuaComponent(Transform transform, string type) {
        if(transform == null) {
            throw new ArgumentNullException("transform");
        }
        if(string.IsNullOrEmpty(type)) {
            throw new ArgumentException("type");
        }

        var loaders = transform.GetComponents<LuaComponentLoader>();
        var rightLoader = loaders.FirstOrDefault(lt => lt.luaTable != null && lt.luaComponentName == type);

        return rightLoader != null ? rightLoader.luaTable : null;
    }

    /// <summary>
    /// 获取指定游戏对象上挂的表名为type的lua组件
    /// </summary>
    public static LuaTable GetLuaComponent(GameObject gameObject, string type) {
        return GetLuaComponent(gameObject.transform, type);
    }

    /// <summary>
    /// 为游戏对象添加lua组件
    /// </summary>
    /// <param name="type">lua组件名</param>
    public static LuaTable AddLuaComponent(GameObject gameObject, string type) {
        var loader = gameObject.AddComponent<LuaComponentLoader>();
        loader.luaComponentName = type;
        loader.Load();
        return loader.luaTable;
    }

    #endregion

    XLuaManager() {
        LuaEnv = new LuaEnv();
        LuaEnv.CustomLoader loader = OriginalLuaLoader;
        LuaEnv.AddLoader(loader);
    }

    // 从项目中加载原始的Lua脚本，仅在editor模式下执行
    byte[] OriginalLuaLoader(ref string filepath) {
        if(string.IsNullOrEmpty(filepath)) {
            Debug.LogError("Load original lua failed, because filepath is empty");
            return null;
        }

        string luaName = filepath + ".lua";
        string folder = string.Format("{0}/XLua/MyExamples/Action/Resources", Application.dataPath);
        string[] files = Directory.GetFiles(folder, "*.txt", SearchOption.AllDirectories);
        string rightFile = files.FirstOrDefault(f => {
            string n = Path.GetFileNameWithoutExtension(f);
            return n == luaName;
        });

        if(string.IsNullOrEmpty(rightFile)) {
            Debug.LogError("Load original lua failed, because lua file not exist, file name: " + luaName);
            return null;
        }

        filepath = rightFile;
        return File.ReadAllBytes(rightFile);
    }

    // void Tick()： 清除Lua的未手动释放的LuaBase（比如，LuaTable， LuaFunction），以及其它一些事情。需要定期调用，比如在MonoBehaviour的Update中调用。
    public void Update() {
        if(LuaEnv != null) {
            LuaEnv.Tick();
        }
    }

    /// <summary>
    /// 加载lua表
    /// </summary>
    /// <param name="name">lua表名，注意：表名与lua文件名必须一致</param>
    public void LoadLuaTable(string name) {
        if(string.IsNullOrEmpty(name)) {
            Debug.LogError("string.IsNullOrEmpty(name)");
            return;
        }

        string code = string.Format("require '{0}'", name);
        LuaEnv.DoString(code);
    }

    /// <summary>
    /// 获取lua表
    /// </summary>
    /// <param name="name">lua表名，注意：表名与lua文件名必须一致</param>
    public LuaTable GetLuaTable(string name) {
        if(string.IsNullOrEmpty(name)) {
            Debug.LogError("string.IsNullOrEmpty(name)");
            return null;
        }

        LuaTable table = LuaEnv.Global.Get<LuaTable>(name);
        if(table == null) {
            LoadLuaTable(name);
            table = LuaEnv.Global.Get<LuaTable>(name);
        }
        return table;
    }

    #region IDisposable

    public void Dispose() {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected void Dispose(bool disposing) {
        if(disposing) {
            if(LuaEnv != null) {
                LuaEnv.GC();
                LuaEnv.Dispose();

                if(m_bundle != null) {
                    m_bundle.Unload(true);
                }
            }
        }

        LuaEnv = null;
        m_instance = null;
        m_bundle = null;
    }

    ~XLuaManager() {
        Dispose(false);
    }

    #endregion

    // just for testing lua hotfix
    public static void TestLuaHotfix() {
        Debug.LogError("Lua hotfix failed.");
        try {
            var luaTable = XLuaManager.m_instance.GetLuaTable("GameHelper");
            if(luaTable != null) {
                Action testAction = luaTable.Get<Action>("ShowHotfixError");
                if(testAction != null) {
                    testAction();
                }
            }
        } catch(Exception ex) {
            Debug.LogError(ex.Message);
        }
    }
}
