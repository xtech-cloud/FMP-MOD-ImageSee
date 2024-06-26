

//*************************************************************************************
//   !!! Generated by the fmp-cli 1.88.0.  DO NOT EDIT!
//*************************************************************************************

namespace XTC.FMP.MOD.ImageSee.App.Service
{

    public class ArgumentRequiredException : Exception
    {
        public ArgumentRequiredException(string _message) : base(_message)
        {

        }
    }

    /// <summary>
    /// 参数检查
    /// </summary>
    public class ArgumentChecker
    {
        /// <summary>
        /// 检查必须的字符型参数
        /// </summary>
        /// <param name="_value">参数的值</param>
        /// <param name="_name">参数的名字</param>
        /// <exception cref="ArgumentException">参数为空或空白符，抛出异常</exception>
        public static void CheckRequiredString(string _value, string _name)
        {
            if (!string.IsNullOrWhiteSpace(_value))
                return;
            throw new ArgumentRequiredException(string.Format("argument {0} is required!", _name));
        }

        /// <summary>
        /// 检查必须的数值型参数
        /// </summary>
        /// <param name="_value">参数的值</param>
        /// <param name="_name">参数的名字</param>
        /// <exception cref="ArgumentException">参数等于0，抛出异常</exception>
        public static void CheckRequiredNumber(int _value, string _name)
        {
            if (0 != _value)
                return;
            throw new ArgumentRequiredException(string.Format("argument {0} is required!", _name));
        }

        /// <summary>
        /// 检查必须的对象型参数
        /// </summary>
        /// <param name="_value">参数的值</param>
        /// <param name="_name">参数的名字</param>
        /// <exception cref="ArgumentException">参数等于0，抛出异常</exception>
        public static void CheckRequiredObject(object _value, string _name)
        {
            if (null != _value)
                return;
            throw new ArgumentRequiredException(string.Format("argument {0} is required!", _name));
        }
    }
}
