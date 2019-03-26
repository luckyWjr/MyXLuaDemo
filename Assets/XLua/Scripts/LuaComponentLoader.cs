using System;
using UnityEngine;
using XLua;

public class LuaComponentLoader : MonoBehaviour {
    public string luaComponentName;

    Action m_updateFunc;
    Action m_lateUpdateFunc;

    public LuaTable luaTable {
        get;
        private set;
    }

    public bool Load() {
        if(string.IsNullOrEmpty(luaComponentName)) {
            return false;
        }

        luaTable = XLuaManager.instance.GetLuaTable(luaComponentName);
        if(luaTable == null) {
            return false;
        }

        luaTable.Set<string, Transform>("transform", transform);
        luaTable.Set<string, GameObject>("gameObject", gameObject);

        m_updateFunc = luaTable.Get<Action>("Update");
        m_lateUpdateFunc = luaTable.Get<Action>("LateUpdate");

        return true;
    }

    void CallLuaFunction(string funcName) {
        if(string.IsNullOrEmpty(funcName)) {
            Debug.LogError("argument error: funcName");
            return;
        }

        Action func = luaTable.Get<Action>(funcName);
        if(func != null) {
            func();
        }
    }

    void Awake() {
        if(Load()) {
            CallLuaFunction("Awake");
        } else {
            // 如果 Name 为空，可能是 Add component
            if(!string.IsNullOrEmpty(luaComponentName)) {
                Debug.LogError("Load lua table failed, no table in " + luaComponentName);
                return;
            }
        }
    }

    void Start() {
        //if(luaTable == null) {
        //    if(string.IsNullOrEmpty(luaComponentName)) {
        //        Debug.LogError("string.IsNullOrEmpty(luaComponentName)");
        //        return;
        //    }

        //    if(!Load()) {
        //        Debug.LogError("Load lua table failed, no table in " + luaComponentName);
        //        return;
        //    }
        //}

        CallLuaFunction("Start");
    }

    void Update() {
        if(m_updateFunc != null) {
            m_updateFunc();
        }
    }

    void LateUpdate() {
        if(m_lateUpdateFunc != null) {
            m_lateUpdateFunc();
        }
    }

    void OnEnable() {
        CallLuaFunction("OnEnable");
    }

    void OnDisable() {
        CallLuaFunction("OnDisable");
    }

    void OnDestroy() {
        CallLuaFunction("OnDestroy");

        luaTable.Set<string, Transform>("transform", null);
        luaTable.Set<string, GameObject>("gameObject", null);

        m_updateFunc = null;
        m_lateUpdateFunc = null;

        luaTable.Dispose();
        luaTable = null;
    }
}