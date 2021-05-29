using System;
using BFInterpreter;

namespace ConsoleInterface {
	public class Config : IInterpreterConfig {

		public IInput Input { get; set; }
		public IOutput Output { get; set; }
		public OverflowBehavior MemoryOverflowBehavior => OverflowBehavior.Wrap;
		public int MemorySize => BFProgram.DefaultMemorySize;
		public TimeSpan StepDelay => TimeSpan.FromSeconds(0.05);

	}
}
