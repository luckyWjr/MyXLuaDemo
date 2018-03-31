using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace newnamespace {

	public class Test4 : MonoBehaviour {

        public delegate IFuck FuckDel(int s);

		void Start () {
            IFuck fuck = new FuckClass();
            fuck.Fuck(5);

            FuckDel a = new FuckDel(TestMethod);
            IFuck fuck2 = a(5);
            Debug.Log("fuck2.people----"+fuck2.people);
        }
		
		void Update () {
			
		}

        IFuck TestMethod(int s) {
            IFuck fuck = new FuckClass();
            fuck.people = s;
            return fuck;
        }
	}
}