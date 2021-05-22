using System;
using BFInterpreter;

namespace ConsoleInterface {
	public class Program {
		private static void Main() {
			ConsoleInputOutput inputOutput = new();
			Interpreter interpreter = new("+++++++[.-]", inputOutput, inputOutput);
			interpreter.Run();
		}
	}
}
