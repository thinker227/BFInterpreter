using System;

#nullable enable

namespace BFInterpreter {
	/// <summary>
	/// Represents an exception that was throw during execution of a BF program.
	/// </summary>
	public class BFException : Exception {


		/// <summary>
		/// Initializes a new <see cref="BFException"/> instance.
		/// </summary>
		public BFException() : base() { }

		/// <summary>
		/// Initializes a new <see cref="BFException"/> instance.
		/// </summary>
		/// <param name="message">The message that describes the exception.</param>
		public BFException(string? message) : base(message) { }

		/// <summary>
		/// Initializes a new <see cref="BFException"/> instance.
		/// </summary>
		/// <param name="message">The message that describes the exception.</param>
		/// <param name="innerException">The exception that is the cause of the current exception.</param>
		public BFException(string? message, Exception? innerException) : base(message, innerException) { }

	}
}
