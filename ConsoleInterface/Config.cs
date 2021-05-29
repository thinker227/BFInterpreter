using System;
using BFInterpreter;

namespace ConsoleInterface {
	public class Config : IInterpreterConfig {

		public Config() {
			ConsoleInputOutput inputOutput = new();
			Input = inputOutput;
			Output = inputOutput;
		}



		public IInput Input { get; }
		public IOutput Output { get; }
		public OverflowBehavior MemoryOverflowBehavior => OverflowBehavior.Wrap;
		public int MemorySize => BFProgram.DefaultMemorySize;
		public TimeSpan StepDelay => TimeSpan.FromSeconds(2);

	}
}
