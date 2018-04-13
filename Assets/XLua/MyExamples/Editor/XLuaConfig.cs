using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using XLua;

namespace MyExamples {

    public static class XLuaConfig {

        [LuaCallCSharp]
        public static List<Type> luaCallCSharpList = new List<Type>() {
            //typeof(Action),
            //typeof(Func<int>),
            typeof(GameObject),
            typeof(ParticleSystem),
            typeof(NeedBlackListClass),
        };

        [CSharpCallLua]
        public static List<Type> cSharpCallLuaList = new List<Type>(){
            typeof(Action),
            typeof(Func<int>),
            typeof(TestSon.IntDelegate),
            //typeof(Action<string>),
            //typeof(Action<double>),
            //typeof(UnityEngine.Events.UnityAction),
            //typeof(IEnumerator)
        };

        [BlackList]
        public static List<List<string>> blackList = new List<List<string>>()  {
             new List<string>(){ "MyExamples.NeedBlackListClass", "s"},
             new List<string>(){ "MyExamples.NeedBlackListClass", "Add", "System.Int32", "UnityEngine.GameObject"},
        };
    }
    
}