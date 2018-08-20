using System;

namespace Latoken.CurrencyProvider.Common.Exceptions
{
	public class ParseAmountException : CurrencyProviderException
	{
		public CodeEnum Code { get; private set; }
		public ParseAmountException(CodeEnum code, string message) : this(code, message, null) { }

		public ParseAmountException(CodeEnum code, string message, Exception innerException) : base(message, innerException)
		{
			Code = code;
		}

		public enum CodeEnum
		{
			Format,
			MinAmount,
			MaxAmount
		}
	}
}