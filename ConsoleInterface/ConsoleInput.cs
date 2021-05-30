using System;
using BFInterpreter;

namespace ConsoleInterface {
	public class ConsoleInput : IInput {

		public byte GetInput() {
			byte result;

			while (true) {
				string input = Console.ReadLine();

				if (byte.TryParse(input, out result)) break;
				if (char.TryParse(input, out char charResult)) {
					result = (byte)charResult;
					break;
				}
			}

			Console.Clear();
			return result;
		}

	}
}
