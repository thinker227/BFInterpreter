using System;
using BFInterpreter;

namespace ConsoleInterface {
	public class ConsoleInputOutput : IInput, IOutput {

		public byte GetInput() {
			bool success;
			byte result;

			do {
				string input = Console.ReadLine();
				success = byte.TryParse(input, out result);
			} while (!success);

			return result;
		}

		public void WriteOutput(byte output) => Console.WriteLine($"{(char)output} ({output})");

	}
}
