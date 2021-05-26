using System;
using System.IO;
using System.Linq;
using BFInterpreter;
using PBrain;

namespace ConsoleInterface {
	public class Program {

		private static void Main(string[] args) {
			string programString;

			if (args.Length >= 1) {
				string arg = args[0];
				programString = arg;

				if (File.Exists(arg)) {
					FileInfo file = new(arg);
					string[] fileExtensions = new string[] { ".b", ".bf" };
					if (fileExtensions.Contains(file.Extension)) programString = File.ReadAllText(arg);
					else throw new IOException($"Invalid extension '{file.Extension}'.");
				}
			} else {
				programString = Console.ReadLine();
			}

			Config config = new();
			Interpreter interpreter = new(config);
			interpreter.RegisterPBrainParsers();
			
			interpreter.Run(programString);
		}

	}
}
