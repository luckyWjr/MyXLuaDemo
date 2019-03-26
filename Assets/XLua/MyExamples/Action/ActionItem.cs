using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MyExamples {

	public class ActionItem : MonoBehaviour {

        [SerializeField] Text m_titleText;
        [SerializeField] Text m_contentText;
        [SerializeField] Button m_btn;

        int index;

        public void Init(int index, string title, string content) {
            this.index = index;
            m_titleText.text = title;
            m_contentText.text = content;
            m_btn.onClick.AddListener(OnClick);

            transform.localPosition = new Vector3(150 + index * 230, 0, 0);
            transform.localScale = Vector3.one;

            gameObject.SetActive(true);
        }

        void OnClick() {
            Debug.Log("index:" + index);
        }
	}
}