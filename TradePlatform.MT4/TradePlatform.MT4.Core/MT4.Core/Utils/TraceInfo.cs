using System;
using System.Threading;

namespace TradePlatform.MT4.Core.Utils
{
	public class TraceInfo
	{
		public BridgeTraceErrorType Type;

		public System.Exception Exception;

		public string Message;

		public TraceInfo(BridgeTraceErrorType type, System.Exception exception = null, string message = "")
		{
			object obj;
			this.Type = type;
			this.Exception = exception;
			TraceInfo traceInfo = this;
			string str = " {0} @ {1} : {2}";
			object now = DateTime.Now;
			string name = Thread.CurrentThread.Name;
			object obj1 = name;
			if (name == null)
			{
				obj1 = "";
			}
			obj = (exception == null ? message : exception.Message);
			traceInfo.Message = string.Format(str, now, obj1, obj);
		}

		public override string ToString()
		{
			return this.Message;
		}
	}
}