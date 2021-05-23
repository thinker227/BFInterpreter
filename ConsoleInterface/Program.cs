using System;
using System.IO;
using BFInterpreter;
using PBrain;

namespace ConsoleInterface {
	public class Program {
		private static void Main(string[] args) {
			/*
				program string =>				Execute string using default config
				.b/.bf file =>					Read file and execute string using default config
				.json file + program string =>	Execute string using .json file as config
				.json file + .b/.bf file =>		Read .b/.bf file and execute string using .json file as config
			*/

			string program;
			Config config;

			switch (args.Length) {
				case 1: {
					config = Config.Default;
					if (File.Exists(args[0])) program = File.ReadAllText(args[0]);		// .b/.bf file
					else program = args[0];		// program string
				} break;

				case 2: {
					config = Config.FromFile(args[0]);
					if (File.Exists(args[1])) program = File.ReadAllText(args[1]);		// .json file + .b/.bf file
					else program = args[1];		// .json file + program string
				} break;

				default: Environment.Exit(1); return;
			}

			Interpreter interpreter = new(program, config);
			
			if (config.UsePBrain) interpreter.RegisterPBrainParsers();

			interpreter.Run();
		}
	}
}
