using System;
using System.Collections.Generic;
using UnityEngine;
using XLua;

namespace MyExamples {

	public class LuaCallCSharp : MonoBehaviour {
        LuaEnv luaenv;
        string script = @"

            local GameObject = CS.UnityEngine.GameObject;
            local go = new GameObject('go');


            --访问成员方法属性
            local Test = CS.MyExamples.Test;
            local test = Test();
            test.index = 66;
            print('test.index---'..test.index);
            print('test.Add---'..test.Add(test,1,2));
            print('test:Add---'..test:Add(3,4));

            --访问父类
            local TestSon = CS.MyExamples.TestSon;
            local testSon = TestSon();
            print('test:Add---'..testSon:Add(1,2));
            print('test:Multiply---'..testSon:Multiply(3,4));



            --每个返回值都对应返回值的规则
            --每个输入的实参都对应参数的规则
            local ret, ret_b, ret_c, ret_funB = CS.MyExamples.A.Method(1,2,function()
                print('exe----funA');
            end);
            print('CS.MyExamples.A.Method  return---', ret, ret_b, ret_c, ret_funB);
            ret_funB();


            --重载
            testSon:Log(3);
            testSon:Log('qwe');



            --参数默认值
            CS.MyExamples.A.MethodB(1,2,'cccc');



            --可变参数
            CS.MyExamples.A.MethodC('s',1,2,3);



            --枚举
            CS.MyExamples.A.MethodD(CS.MyExamples.ETest.T1);
            CS.MyExamples.A.MethodD(CS.MyExamples.ETest.__CastFrom(1));
            CS.MyExamples.A.MethodD(CS.MyExamples.ETest.__CastFrom('T3'));



            --delegate
            testSon.intDelegate(10);
            
            local function lua_delegate(a)
                print('lua_delegate :', a)
            end

            testSon.intDelegate = lua_delegate + testSon.intDelegate --combine，这里演示的是C#delegate作为右值，左值也支持
            testSon.intDelegate(100)
            testSon.intDelegate = testSon.intDelegate - lua_delegate --remove
            testSon.intDelegate(1000)



            --event
            local function lua_eventCallback1(a)
                print('lua_eventCallback1 :', a)
            end
            local function lua_eventCallback2(a)
                print('lua_eventCallback2 :', a)
            end
            --增加事件回调
            testSon:intEvent('+', lua_eventCallback1);
            testSon:intEvent('+', lua_eventCallback2);
            testSon:ExeEvent(100);
            --移除事件回调
            testSon:intEvent('-', lua_eventCallback1);
            testSon:ExeEvent(1000);
            testSon:intEvent('-', lua_eventCallback2);



            --struct  table
            CS.MyExamples.A.MethodE({boxA={x=1,y=2},name='box'});


            --typeof
            go:AddComponent(typeof(CS.UnityEngine.Texture));
        ";

		void Start () {
            luaenv = new LuaEnv();
            luaenv.DoString(script);
            
        }
		
		void Update () {
            if(luaenv != null) {
                luaenv.Tick();
            }
        }

        void OnDestroy() {
            luaenv.Dispose();
        }
    }


    public class Test {
        public int index;
        public int Add(int a, int b) {
            return a + b;
        }
    }

    public class TestSon : Test {
        public int Multiply(int a, int b) {
            return a * b;
        }

        public void Log(float a) {
            Debug.Log("Log---a:" + a);
        }

        public void Log(double a) {
            Debug.Log("Log---a:" + a);
        }

        public void Log(string a) {
            Debug.Log("Log---a:" + a);
        }

        public delegate int IntDelegate(int a);
        public IntDelegate intDelegate = (a) => {
            Debug.Log("C#--intDelegate----a:" + a);
            return a;
        };

        public event IntDelegate intEvent;

        public void ExeEvent(int a) {
            intEvent(a);
        }
    }

    public class A {
        public static int Method(int a, ref int b, out int c, Action funA, out Action funB) {
            Debug.Log("Method-----a:" + a + "----b:" + b);
            c = 10;
            funA();
            funB = () => { Debug.Log("exe---funB"); };
            return 5;
        }

        public static void MethodB(int a, int b, string c, string d = "ddd") {
            Debug.Log("MethodB--a:" + a + "---b:" + b + "----c" + c + "---d:" + d);
        }

        public static void MethodC(string s, params int[] arr) {
            foreach(int i in arr) {
                Debug.Log("MethodC----" + i);
            }
        }

        public static void MethodD(ETest s) {
            Debug.Log("MethodD----" + s);
        }

        public static void MethodE(BoxB box) {
            Debug.Log("MethodE----name:" + box.name + "----x:" + box.boxA.x);
        }
    }

    [LuaCallCSharp]
    public enum ETest {
        T1,
        T2,
        T3
    }

    public struct BoxA {
        public int x;
        public int y;
    }

    public struct BoxB {
        public BoxA boxA;
        public string name;
    }
}