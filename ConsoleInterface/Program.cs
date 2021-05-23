using System;
using BFInterpreter;
using PBrain;

namespace ConsoleInterface {
	public class Program {
		private static void Main() {
			string program = Console.ReadLine();

			ConsoleInputOutput inputOutput = new();
			Config config = new();
			Interpreter interpreter = new(program, config);

			if (config.UsePBrain) {
				interpreter.RegisterPBrainParsers();
			}

			interpreter.OnProgramExit += OnExit;
			interpreter.Run();
		}

		private static void OnExit(int exitCode, string message) {
			Console.WriteLine($"BF Program exited with code {exitCode} and message '{message}'.");
		}
	}
}
