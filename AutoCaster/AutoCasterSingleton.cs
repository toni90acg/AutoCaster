using AutoCaster.Interfaces;

namespace AutoCaster
{
    public class AutoCasterSingleton
    {
        private static IAutoCaster _instance;

        private AutoCasterSingleton(){}

        public static IAutoCaster GetInstance()
        {
            return _instance ?? (_instance = new AutoCaster());
        }
    }
}
