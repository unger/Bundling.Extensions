namespace Bundling.Extensions.Helpers
{
	using System;
	using System.Diagnostics;

	public class Assert
	{
		public static void ArgumentNotNull(object argument, string argumentName)
		{
			if (argument != null)
			{
				return;
			}

			Debug.Assert(argument == null, string.Format("Argument '{0}' cannot be null", argumentName));
			if (argumentName != null)
			{
				throw new ArgumentNullException(argumentName);
			}

			throw new ArgumentNullException();
		}
	}
}
