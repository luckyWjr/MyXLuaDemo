using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace newnamespace {

    public class FuckClass : IFuck {
        public int people {
            get;set;
        }

        public void Fuck(int people) {
            Debug.Log("FuckClass------Fuck!!!!!!!!!!!!!!!");
        }

        public void MyMethod() {

        }
    }
}