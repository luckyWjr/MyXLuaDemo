using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

namespace MyExamples {

	public class BlackList : MonoBehaviour {
        LuaEnv luaenv;
        string script = @"
            local go = CS.UnityEngine.GameObject('gogogo');
            local nblclass = CS.MyExamples.NeedBlackListClass;
            local nbl = nblclass();
            nbl.x = 10;
            nbl:Add(20);
            nbl.s = 'sss';
            nbl:Add(20, go);
        ";

		void Start () {
            luaenv = new LuaEnv();
            luaenv.DoString(script);
        }

        void Update() {
            if(luaenv != null) {
                luaenv.Tick();
            }
        }

        void OnDestroy() {
            if(luaenv != null) {
                luaenv.Dispose();
            }
        }
    }

    public class NeedBlackListClass {

        public int x;
        public string s;

        public void Add(int y) {
            x += y;
            Debug.Log("add---x:" + x);
        }

        public void Add(int y, GameObject go) {
            x += y;
            Debug.Log("add---x:" + x + "----go---" + go.name);
        }
    }
}