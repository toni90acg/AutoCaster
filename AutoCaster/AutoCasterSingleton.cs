namespace AutoCaster
{
    public class AutoCasterSingleton
    {
        private static AutoCaster _instance;

        private AutoCasterSingleton(){}

        public static AutoCaster GetInstance()
        {
            return _instance ?? (_instance = new AutoCaster());
        }
    }
}
