using System;
using System.Collections.Generic;
using System.Linq;
using BFInterpreter;
using PBrain.Parsers;

namespace PBrain {
	/// <summary>
	/// Represents a handler for PBrain functions.
	/// </summary>
	internal sealed class FunctionHandler {

		// Contains the instruction pointer positions each memory value points to
		// The program "++(,.)" would set the 2nd index in the array to a value of 2
		private int[] functionPositions;
		// Contains the corresponding function ending for each function opening and vice versa
		private Dictionary<int, int> beginEndPairs;
		private Stack<int> callStack;

		// Represents no function having been defined for a memory value
		private const int FunctionEmpty = -1;



		public Interpreter Interpreter { get; }



		/// <summary>
		/// Initializes a new <see cref="FunctionHandler"/> instance.
		/// </summary>
		public FunctionHandler(Interpreter interpreter) {
			Interpreter = interpreter;
			Interpreter.OnCommandStringChanged += OnCommandStringChange;
		}



		private void OnCommandStringChange(Interpreter interpreter, string newCommandString) {
			functionPositions = new int[byte.MaxValue + 1];
			beginEndPairs = interpreter.GetBeginEndPairs('(', ')');
			callStack = new();

			Array.Fill(functionPositions, FunctionEmpty);
		}

		/// <summary>
		/// Defines the current instruction pointer position as the entry point
		/// for a new function corresponding to the value of the current memory.
		/// </summary>
		public void DefineFunction() {
			int currentMemory = Interpreter.Program.GetCurrentMemory();
			functionPositions[currentMemory] = Interpreter.InstructionPointer;
		}

		/// <summary>
		/// Gets the instruction pointer position corresponding to the exit point of the current function.
		/// </summary>
		/// <returns>The exit point of the current function.</returns>
		public int GetFunctionExit() {
			if (beginEndPairs.TryGetValue(Interpreter.InstructionPointer, out int end)) return end;

			throw new Exception(
				$"No end-point has been defined for the position {Interpreter.InstructionPointer}."
			);
		}

		/// <summary>
		/// Gets the instruction pointer position corresponding to the
		/// entry point of the function pointed to by the current memory.
		/// </summary>
		/// <returns>The entry point of the function pointed to by the current memory.</returns>
		public int GetFunctionEntry() {
			int currentMemory = Interpreter.Program.GetCurrentMemory();

			if (functionPositions[currentMemory] == FunctionEmpty) throw new Exception(
				$"No function has been defined for the value '{currentMemory}'."
			);

			callStack.Push(Interpreter.InstructionPointer);
			return functionPositions[currentMemory];
		}

		/// <summary>
		/// Gets the instruction pointer position corresponding
		/// to the position to exit the current function to.
		/// </summary>
		/// <returns>The current last entry on the function callstack.</returns>
		public int GetReturnPosition() {
			if (callStack.Count == 0) throw new Exception(
				"Could not exit function because the call stack is empty."
			);

			return callStack.Pop();
		}

	}
}
