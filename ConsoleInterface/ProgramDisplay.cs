using System;
using System.Collections.Generic;
using System.Text;
using BFInterpreter;
using static System.Console;

namespace ConsoleInterface {
	public class ProgramDisplay : IOutput {

		private int programStringHeight;
		private readonly Queue<string> output;
		private readonly int maxOutputLength;



		public Interpreter Interpreter { get; }



		public ProgramDisplay(Interpreter interpreter) {
			output = new();
			maxOutputLength = 10;

			Interpreter = interpreter;
			
			Interpreter.OnProgramStep += OnProgramStep;
			Interpreter.OnCommandStringChanged += OnCommandStringChanged;
		}



		private void OnProgramStep(Interpreter interpreter) {
			SetCursorPosition(0, 0);
			WriteArrayString();

			SetCursorPosition(0, 2);
			WriteProgramString();

			SetCursorPosition(0, 3 + programStringHeight);
			WriteOutputString();
		}

		private void WriteArrayString() {
			ClearLine();

			IEnumerator<byte> memory = Interpreter.Program.GetMemoryEnumerator();
			int memoryPointer = Interpreter.Program.MemoryPointer;
			int memoryMin = memoryPointer - 3;
			int memoryMax = memoryPointer + 3;

			Write('[');
			//int i = 0;
			for (int i = 0; i <= memoryMax; i++) {
			//while (memory.MoveNext() && i <= memoryMax) {
				if (!memory.MoveNext()) break;
				if (i < memoryMin) continue;

				if (i == memoryPointer) BackgroundColor = ConsoleColor.DarkGray;
				Write(memory.Current);
				ResetColor();

				if (i != memoryMax) Write(", ");

				//i++;
			}
			Write(']');
		}

		private void WriteProgramString() {
			for (int i = 0; i < Interpreter.CommandString.Length; i++) {
				char current = Interpreter.CommandString[i];

				if (i == Interpreter.InstructionPointer) BackgroundColor = ConsoleColor.DarkGray;
				Write(current);
				ResetColor();
			}
		}

		private void OnCommandStringChanged(Interpreter interpreter, string newCommandString) {
			programStringHeight = GetStringHeight(newCommandString);
		}

		private static int GetStringHeight(string commandString) {
			int height = 1;
			foreach (char current in commandString) {
				if (current == '\n') height++;
			}
			return height;
		}

		private static void ClearLine() {
			int line = GetCursorPosition().Top;
			Write(new string(' ', WindowWidth));
			SetCursorPosition(0, line);
		}

		private void WriteOutputString() {
			foreach (string str in output) {
				ClearLine();
				WriteLine(str);
			}
		}

		public void WriteOutput(byte output) {
			this.output.Enqueue($"{output} '{(char)output}'");
			if (this.output.Count > maxOutputLength) this.output.Dequeue();
		}

	}
}
