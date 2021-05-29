using System;
using BFInterpreter;

namespace ConsoleInterface {
	public class ConsoleInput : IInput {

		public byte GetInput() {
			bool success;
			byte result;

			do {
				string input = Console.ReadLine();
				success = byte.TryParse(input, out result);
			} while (!success);

			Console.Clear();
			return result;
		}

	}
}
