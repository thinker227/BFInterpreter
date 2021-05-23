using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using BFInterpreter;

namespace ConsoleInterface {
	public class Config : IInterpreterConfig {

		public Config() {
			ConsoleInputOutput inputOutput = new();
			Input = inputOutput;
			Output = inputOutput;
		}



		[JsonIgnore] public IInput Input { get; set; }
		[JsonIgnore] public IOutput Output { get; set; }
		public bool UsePBrain { get; set; }
		public OverflowBehavior MemoryOverflowBehavior { get; set; }
		public int MemorySize { get; set; }

		public static Config Default {
			get {
				ConsoleInputOutput inputOutput = new();
				return new Config() {
					UsePBrain = false,
					MemoryOverflowBehavior = OverflowBehavior.Wrap,
					MemorySize = BFProgram.DefaultMemorySize,
					Input = inputOutput,
					Output = inputOutput
				};
			}
		}



		public static Config FromFile(string path) {
			FileInfo file = new(path);

			if (file.Extension == ".json") {
				string fileString = File.ReadAllText(path);
				return JsonSerializer.Deserialize<Config>(fileString);
			}

			throw new IOException($"Invalid file extension '{file.Extension}'.");
		}

	}
}
