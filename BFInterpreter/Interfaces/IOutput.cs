namespace BFInterpreter {
	/// <summary>
	/// Defines a method for writing output from a BF program.
	/// </summary>
	public interface IOutput {

		/// <summary>
		/// Writes a byte of output.
		/// </summary>
		/// <param name="output">The output byte to write.</param>
		public void WriteOutput(byte output);

	}
}
