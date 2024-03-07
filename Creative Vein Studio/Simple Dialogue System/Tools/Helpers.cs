using System;

namespace CreativeVeinStudio.Simple_Dialogue_System.Tools
{
    public static class Helpers
    {
#if UNITY_EDITOR
        public static string CreateUniqueID()
        {
            var ticks = new DateTime(1999, 1, 1).Ticks;
            var ans = DateTime.Now.Ticks - ticks;
            return ans.ToString("x");
        }

        public static int GetTimeStamp() => (int)new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
#endif
    }
}