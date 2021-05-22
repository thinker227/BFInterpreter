namespace BFInterpreter {
	/// <summary>
	/// Defines a method for getting input for a BF program.
	/// </summary>
	public interface IInput {

		/// <summary>
		/// Gets a byte of input.
		/// </summary>
		/// <returns>The inputted byte.</returns>
		public byte GetInput();

	}
}
