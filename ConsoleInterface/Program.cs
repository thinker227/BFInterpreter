using System;
using BFInterpreter;

namespace ConsoleInterface {
	public class Program {
		private static void Main() {
			string program = Console.ReadLine();

			ConsoleInputOutput inputOutput = new();
			Interpreter interpreter = new(program, new Config());
			interpreter.OnProgramExit += OnExit;
			interpreter.Run();
		}

		private static void OnExit(int exitCode, string message) {
			Console.WriteLine($"BF Program exited with code {exitCode} and message '{message}'.");
		}
	}
}
