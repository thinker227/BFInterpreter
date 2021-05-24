using System;
using System.Collections.Generic;
using System.Linq;
using BFInterpreter;
using PBrain.Parsers;

namespace PBrain {
	internal sealed class FunctionHandler {

		// Represents no function having been defined for a memory value
		private const int FunctionEmpty = -1;

		// Contains the instruction pointer positions each memory value points to
		// The program "++(,.)" would set the 2nd index in the array to a value of 2
		private readonly int[] functionPositions;
		// Contains the corresponding function ending for each function opening
		private readonly Dictionary<int, int> beginEndPairs;
		private readonly Stack<int> callStack;



		public Interpreter Interpreter { get; }



		public FunctionHandler(Interpreter interpreter) {
			Interpreter = interpreter;
			functionPositions = new int[byte.MaxValue + 1];
			beginEndPairs = GetBeginEndPairs(Interpreter.CommandString);
			callStack = new();

			System.Array.Fill(functionPositions, FunctionEmpty);
		}



		public static Dictionary<int, int> GetBeginEndPairs(string commandString) {
			Dictionary<int, int> returnDictionary = new();
			Stack<int> beginnings = new();

			for (int i = 0; i < commandString.Length; i++) {
				char current = commandString[i];

				if (current == ')' && beginnings.Count == 0) throw new Exception(
					$"Inconsistent function ending at position {i}."
				);
				if (current == '(') beginnings.Push(i);
				if (current == ')') returnDictionary.Add(beginnings.Pop(), i);
			}

			return returnDictionary;
		}

		public void DefineFunction() {
			int currentMemory = Interpreter.Program.GetCurrentMemory();
			functionPositions[currentMemory] = Interpreter.InstructionPointer;
		}

		public void SkipToFunctionEnd() {
			if (beginEndPairs.TryGetValue(Interpreter.InstructionPointer, out int end)) {
				Interpreter.InstructionPointer = end;
				return;
			}

			throw new Exception(
				$"No end-point has been defined for the position {Interpreter.InstructionPointer}."
			);
		}

		public void EnterFunction() {
			int currentMemory = Interpreter.Program.GetCurrentMemory();

			if (functionPositions[currentMemory] == FunctionEmpty) throw new Exception(
				$"No function has been defined for the value '{currentMemory}'."
			);

			callStack.Push(Interpreter.InstructionPointer);
			int functionPosition = functionPositions[currentMemory];
			Interpreter.InstructionPointer = functionPosition;
		}

		public void ExitFunction() {
			if (callStack.Count == 0) throw new Exception(
				"Could not exit function because the call stack is empty."
			);

			int exitPosition = callStack.Pop();
			Interpreter.InstructionPointer = exitPosition;
		}

	}
}
