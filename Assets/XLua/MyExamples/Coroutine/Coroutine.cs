using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

namespace MyExamples {

	public class Coroutine : MonoBehaviour {

        LuaEnv luaenv = null;

        void Start() {
            luaenv = new LuaEnv();
            luaenv.DoString("require 'mycoroutine'");
        }

        void Update() {
            if(luaenv != null) {
                luaenv.Tick();
            }
        }
    }
}