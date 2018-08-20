using System;
using System.Collections.Generic;
using System.Text;

namespace Latoken.CurrencyProvider.Common.Exceptions
{
	public class ParseAddressException : CurrencyProviderException
	{
		public CodeEnum Code { get; private set; }
	    public ParseAddressException(CodeEnum code , string message) :this(code, message, null){}

	    public ParseAddressException(CodeEnum code, string message, Exception innerException) : base(message, innerException)
	    {
		    Code = code;
	    }

	    public enum CodeEnum
	    {
		    Format,
			Lenght,
			FirstByte,
			CheckSumm
	    }
    }
}