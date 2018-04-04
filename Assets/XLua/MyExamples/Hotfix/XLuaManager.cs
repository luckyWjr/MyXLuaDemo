using XLua;

namespace MyExamples {

	public class XLuaManager {

        LuaEnv m_luaEnv;
        static XLuaManager m_instance;

        public static XLuaManager instance {
            get {
                if(m_instance == null) {
                    m_instance = new XLuaManager();
                }
                return m_instance;
            }
        }

        XLuaManager() {
            if(m_luaEnv == null) {
                m_luaEnv = new LuaEnv();
            }
        }

        public void Start() {
            m_luaEnv.DoString("require 'main'");
        }
    }
}