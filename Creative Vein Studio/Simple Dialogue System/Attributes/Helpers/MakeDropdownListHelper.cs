using System.Collections.Generic;
using System.Linq;
using CreativeVeinStudio.Simple_Dialogue_System.Models;
using CreativeVeinStudio.Simple_Dialogue_System.Scriptable_Objects;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace CreativeVeinStudio.Simple_Dialogue_System.Attributes.Helpers
{
    public static class MakeDropdownListHelper
    {
        private const string Path = "Assets/CreativeVeinStudio/Example Scenes/Dialogue Content/Objects/";

        public static List<VariablesModel> GetGlobalVars()
        {
            var lst = new List<VariablesModel>();
#if UNITY_EDITOR
            var globalVars =
                AssetDatabase.LoadAssetAtPath($"{Path}GlobalVars.asset",
                        typeof(SO_GlobalVars)) as
                    SO_GlobalVars;

            if (globalVars != null)
                lst = globalVars.globalVariables.Select(x => x).ToList();

            // Add Node RanQty
            lst.Add(new VariablesModel()
            { varName = "prev Node - RanQty" });
#endif
            return lst;
        }
    }
}