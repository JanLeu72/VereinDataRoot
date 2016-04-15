namespace VereinDataRoot
{
    public static class Log
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static log4net.ILog Net
        {
            get { return log; }
        }
    }
}

