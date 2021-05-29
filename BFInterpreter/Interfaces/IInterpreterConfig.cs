using System;

namespace BFInterpreter {
	public interface IInterpreterConfig : IProgramConfig {

		/// <summary>
		/// The delay between steps of the program.
		/// </summary>
		public TimeSpan StepDelay { get; }

	}
}
