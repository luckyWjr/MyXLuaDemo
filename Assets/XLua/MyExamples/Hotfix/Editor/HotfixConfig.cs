using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using XLua;

namespace MyExamples {

	public static class HotfixConfig {

        [Hotfix]
        public static List<Type> hotfixList {
            get {
                string[] allowNamespaces = new string[] {
                    "MyExamples",
                };

                return (from type in Assembly.Load("Assembly-CSharp").GetTypes()
                        where allowNamespaces.Contains(type.Namespace)
                        select type).ToList();
            }
        }

    }
}