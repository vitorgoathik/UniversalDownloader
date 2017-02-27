using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.BatchDownloaderUC;
using static Utilities.BatchDownloaderUC.Enums;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("UniversalDownloaderUnitTests")]
namespace BatchDownloaderUC.Exceptions
{
    internal class DownloaderUCException : Exception
    {
        internal readonly ErrorType Error;
        internal readonly string ErrorMessage;
        internal readonly Exception Exception;
        

        internal DownloaderUCException(ErrorType error, string suffix1, string suffix2, string suffix3, string suffix4, Exception exception = null)
        {
            Error = error;
            Exception = exception;
            ErrorMessage = String.Format(Enums.GetEnumDescription((ErrorType)error), suffix1, suffix2, suffix3, suffix4);
        }
        internal DownloaderUCException(ErrorType error, string suffix1, string suffix2, string suffix3, Exception exception = null) : this(error, suffix1, suffix2,suffix3,"", exception) { }
        internal DownloaderUCException(ErrorType error, string suffix1, string suffix2, Exception exception = null) : this(error, suffix1, suffix2, "", "", exception) { }
        internal DownloaderUCException(ErrorType error, string suffix1, Exception exception = null) : this(error, suffix1, "", "", "", exception) { }
        internal DownloaderUCException(ErrorType error, Exception exception = null) : this(error, "", "", "", "", exception) { }
        internal DownloaderUCException(string errorMessage) 
        {
            ErrorMessage = errorMessage;
        }


        /// <summary>
        /// Throws an UNKNOWN exception under a KNOWN scope (general exception)
        /// if it came from a known exception (DownloaderUCException) it will throw it instead
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="ex"></param>
        internal static void ThrowNewGeneral(DownloaderUCException exception)
        {
            if(exception.Exception != null && exception.Exception.GetType() == typeof(DownloaderUCException))
                throw (DownloaderUCException)exception.Exception;
            throw exception;
        }

        /// <summary>
        /// Throws an UNKNOWN exception under an UNKNOWN scope (system exception)
        /// if it came from a known exception (DownloaderUCException) it will throw it instead
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="ex"></param>
        internal static void Throw(Exception ex)
        {
            if (ex.GetType() == typeof(DownloaderUCException))
                throw (DownloaderUCException)ex;
            throw ex;
        }

        internal static ErrorType GetErrorType(Exception ex)
        {
            if (ex.GetType() == typeof(DownloaderUCException))
                return ((DownloaderUCException)ex).Error;
            throw ex;
        }
    }
}
