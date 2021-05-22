using System;
using BFInterpreter;

namespace ConsoleInterface {
	public class Program {
		private static void Main() {
			Interpreter interpreter = new("+++++++[.-]");
			interpreter.Run();
		}
	}
}
