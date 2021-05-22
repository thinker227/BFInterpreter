namespace BFInterpreter {
	/// <summary>
	/// Represents an event handler for when a BF program exists.
	/// </summary>
	/// <param name="exitCode">The exit code of the program.</param>
	/// <param name="message">The exit message of the program.</param>
	public delegate void ProgramExitEventHandler(int exitCode, string message);
}
