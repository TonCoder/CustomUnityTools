using System.Collections.Generic;
using CreativeVeinStudio.Simple_Dialogue_System.Models;
using JetBrains.Annotations;

namespace CreativeVeinStudio.Simple_Dialogue_System.Interface
{
    public interface ICustomVarActions
    {
        VariablesModel GetValueByName(string val);
    }
}