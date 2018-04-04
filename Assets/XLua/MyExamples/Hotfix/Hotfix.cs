using UnityEngine;

namespace MyExamples {

	public class Hotfix : MonoBehaviour {

		void Start () {
            XLuaManager.instance.Start();
            Show();
        }

        void Show() {
            Debug.Log("Show!!!");
        }
	}
}