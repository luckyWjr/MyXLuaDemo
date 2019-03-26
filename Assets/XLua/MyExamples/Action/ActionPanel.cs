using UnityEngine;
using UnityEngine.UI;

namespace MyExamples {

	public class ActionPanel : MonoBehaviour {

        [SerializeField] Text m_actionText;
        [SerializeField] ActionItem m_actionItem;
        [SerializeField] Transform m_parent;

        void Start () {
            m_actionText.text = "噼里啪啦活动";

            ActionItem item0 = GameObject.Instantiate(m_actionItem) as ActionItem;
            item0.transform.SetParent(m_parent);
            item0.Init(0, "活动1", "活动描述：qweqweqwewqeqweqweqweqwewqewqewqeqweeqwqweqweqwe");

            ActionItem item1 = Instantiate(m_actionItem) as ActionItem;
            item1.transform.SetParent(m_parent);
            item1.Init(1, "活动2", "活动描述：123435243543524352543254352435243523452345342523");
        }
	}
}