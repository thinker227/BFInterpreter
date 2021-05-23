using System;

#nullable enable
namespace BFInterpreter {
	/// <summary>
	/// Represents an exception that was throw during execution of a BF program.
	/// </summary>
	public class BFException : Exception {

		/// <summary>
		/// The <see cref="BFInterpreter.Interpreter"/> which threw the exception.
		/// </summary>
		public Interpreter? Interpreter { get; }



		/// <summary>
		/// Initializes a new <see cref="BFException"/> instance.
		/// </summary>
		public BFException() : base() { }

		/// <summary>
		/// Initializes a new <see cref="BFException"/> instance.
		/// </summary>
		/// <param name="interpreter">The <see cref="BFInterpreter.Interpreter"/> which threw the exception.</param>
		public BFException(Interpreter? interpreter) :
			base() => Interpreter = interpreter;

		/// <summary>
		/// Initializes a new <see cref="BFException"/> instance.
		/// </summary>
		/// <param name="interpreter">The <see cref="BFInterpreter.Interpreter"/> which threw the exception.</param>
		/// <param name="message">The message that describes the exception.</param>
		public BFException(Interpreter? interpreter, string? message) :
			base(message) => Interpreter = interpreter;

		public BFException(Interpreter? interpreter, Exception? innerException) :
			base(null, innerException) => Interpreter = interpreter;

		/// <summary>
		/// Initializes a new <see cref="BFException"/> instance.
		/// </summary>
		/// <param name="interpreter">The <see cref="BFInterpreter.Interpreter"/> which threw the exception.</param>
		/// <param name="message">The message that describes the exception.</param>
		/// <param name="innerException">The exception that is the cause of the current exception.</param>
		public BFException(Interpreter? interpreter, string? message, Exception? innerException) :
			base(message, innerException) => Interpreter = interpreter;

	}
}
