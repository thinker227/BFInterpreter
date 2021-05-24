namespace BFInterpreter {
	public interface IProgramConfig {

		/// <summary>
		/// The <see cref="IInput"/> to use for getting input to the program.
		/// </summary>
		public IInput Input { get; }
		/// <summary>
		/// The <see cref="IOutput"/> to use for writing output from the program.
		/// </summary>
		public IOutput Output { get; }
		/// <summary>
		/// The overflow behavior when a value in memory overflows.
		/// </summary>
		public OverflowBehavior MemoryOverflowBehavior { get; }
		/// <summary>
		/// The size of the program memory.
		/// </summary>
		public int MemorySize { get; }

	}

	/// <summary>
	/// Defines behaviors for when a value in memory overflows.
	/// </summary>
	public enum OverflowBehavior {
		/// <summary>
		/// Overflowed values will wrap around to their minimum/maximum value.
		/// </summary>
		Wrap,
		/// <summary>
		/// Overflowed values will remain at their minimum/maximum value.
		/// </summary>
		Freeze,
		/// <summary>
		/// An exception will be thrown when a value overflows.
		/// </summary>
		Throw
	}
}
