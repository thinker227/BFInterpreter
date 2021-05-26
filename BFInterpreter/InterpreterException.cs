using System;

#nullable enable

namespace BFInterpreter {
	/// <summary>
	/// Represents an exception throw by an <see cref="BFInterpreter.Interpreter"/>.
	/// </summary>
	public class InterpreterException : Exception {

		/// <summary>
		/// The <see cref="BFInterpreter.Interpreter"/> which threw the exception.
		/// </summary>
		public Interpreter? Interpreter { get; }



		/// <summary>
		/// Initializes a new <see cref="InterpreterException"/> instance.
		/// </summary>
		public InterpreterException() : base() { }

		/// <summary>
		/// Initializes a new <see cref="InterpreterException"/> instance.
		/// </summary>
		/// <param name="interpreter">The <see cref="BFInterpreter.Interpreter"/> which threw the exception.</param>
		public InterpreterException(Interpreter? interpreter) :
			base() => Interpreter = interpreter;

		/// <summary>
		/// Initializes a new <see cref="InterpreterException"/> instance.
		/// </summary>
		/// <param name="interpreter">The <see cref="BFInterpreter.Interpreter"/> which threw the exception.</param>
		/// <param name="message">The message that describes the exception.</param>
		public InterpreterException(Interpreter? interpreter, string? message) :
			base(message) => Interpreter = interpreter;

		/// <summary>
		/// Initializes a new <see cref="InterpreterException"/> instance.
		/// </summary>
		/// <param name="interpreter">The <see cref="BFInterpreter.Interpreter"/> which threw the exception.</param>
		/// <param name="innerException">The exception that is the cause of the current exception.</param>
		public InterpreterException(Interpreter? interpreter, Exception? innerException) :
			base(null, innerException) => Interpreter = interpreter;

		/// <summary>
		/// Initializes a new <see cref="InterpreterException"/> instance.
		/// </summary>
		/// <param name="interpreter">The <see cref="BFInterpreter.Interpreter"/> which threw the exception.</param>
		/// <param name="message">The message that describes the exception.</param>
		/// <param name="innerException">The exception that is the cause of the current exception.</param>
		public InterpreterException(Interpreter? interpreter, string? message, Exception? innerException) :
			base(message, innerException) => Interpreter = interpreter;

	}
}
