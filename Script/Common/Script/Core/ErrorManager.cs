using System.Collections;

namespace GameBase
{
    public class ErrorInfo
    {
        public string ErrorMsg;
    }

    public class ErrorManager
    {
        private static ErrorInfo _ErrorInfo = new ErrorInfo();

        public static void PushError(string errorMsg)
        {
            _ErrorInfo.ErrorMsg = errorMsg;
        }

        public static void DisplayLastError()
        {
            if (_ErrorInfo != null && !string.IsNullOrEmpty(_ErrorInfo.ErrorMsg))
            {
                LogManager.LogError("Error:" + _ErrorInfo.ErrorMsg);
            }
        }

        public static void PushAndDisplayError(string errorMsg)
        {
            PushError(errorMsg);
            DisplayLastError();
        }
    }
}
