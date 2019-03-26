using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyExamples {

    public class QWE {
        string a = "111";
    }

	public class NewBehaviourScript : MonoBehaviour {


        public Dictionary<string, QWE> test;

		void Start () {
            Debug.Log(typeof(QWE).ToString());
            XLuaManager.instance.Start();
            Set();
        }
		
		void Update () {
			
		}

        void Set() {
            test.Add("1",new QWE());
        }
	}
}