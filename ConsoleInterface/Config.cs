using BFInterpreter;

namespace ConsoleInterface {
	public class Config : IInterpreterConfig {

		public ConsoleInputOutput InputOutput { get; } = new();
		public bool UsePBrain => true;
		public IInput Input => InputOutput;
		public IOutput Output => InputOutput;
		public OverflowBehavior MemoryOverflowBehavior => OverflowBehavior.Freeze;
		public int MemorySize => BFProgram.DefaultMemorySize;

	}
}
