using System;
using System.Collections.Generic;
using System.Linq;

namespace BFInterpreter {
	/// <summary>
	/// Represents a BF program.
	/// </summary>
	public sealed class BFProgram {

		/// <summary>
		/// The default memory size.
		/// </summary>
		public const int DefaultMemorySize = 30000;
		private readonly byte[] memory;
		private int memoryPointer;



		/// <summary>
		/// The current position of the memory pointer.
		/// </summary>
		public int MemoryPointer => memoryPointer;
		/// <summary>
		/// The size of the program memory.
		/// </summary>
		public int MemorySize { get; }
		/// <summary>
		/// The <see cref="IInput"/> for getting input to the program.
		/// </summary>
		public IInput Input { get; }
		/// <summary>
		/// The <see cref="IOutput"/> for writing output from the program.
		/// </summary>
		public IOutput Output { get; }
		/// <summary>
		/// Event invoked when the program exits.
		/// </summary>
		public event ProgramExitEventHandler OnProgramExit;



		/// <summary>
		/// Initializes a new <see cref="BFProgram"/>.
		/// </summary>
		/// <param name="memorySize">The size of the program memory.</param>
		/// <param name="input">The <see cref="IInput"/> for getting input to the program.</param>
		/// <param name="output">The <see cref="IOutput"/> for writing output from the program.</param>
		public BFProgram(int memorySize, IInput input, IOutput output) {
			if (memorySize <= 0) throw new ArgumentException(
				"Memory size can't be less or equal to 0.", nameof(memorySize)
			);

			memory = new byte[memorySize];
			memoryPointer = 0;
			MemorySize = memorySize;
			Input = input;
			Output = output;
		}



		private void SetPointer(int position) {
			memoryPointer = position;
			if (memoryPointer < 0) memoryPointer = MemorySize - 1;
			if (memoryPointer >= MemorySize) memoryPointer = 0;
		}
		/// <summary>
		/// Increments the position of the memory pointer.
		/// </summary>
		public void IncrementPointer() => SetPointer(MemoryPointer + 1);
		/// <summary>
		/// Decrements the position of the memory pointer.
		/// </summary>
		public void DecrementPointer() => SetPointer(MemoryPointer - 1);

		/// <summary>
		/// Gets the value of the current memory pointed to by the memory pointer.
		/// </summary>
		/// <returns></returns>
		public byte GetCurrentMemory() => memory[memoryPointer];
		/// <summary>
		/// Sets the current memory pointed to by the memory pointer to an
		/// inputted value from the program's specified <see cref="IInput"/>.
		/// </summary>
		public void InputCurrentMemory() => memory[memoryPointer] = Input.GetInput();
		/// <summary>
		/// Increments the value of the current memory pointed to by the memory pointer.
		/// </summary>
		public void IncrementCurrentMemory() => memory[memoryPointer]++;
		/// <summary>
		/// Decrements the value of the current memory pointed to by the memory pointer.
		/// </summary>
		public void DecrementCurrentMemory() => memory[memoryPointer]--;
		/// <summary>
		/// Gets the <see cref="IEnumerator{T}"/> of the memory.
		/// </summary>
		/// <returns>The <see cref="IEnumerator{T}"/> of the program's memory.</returns>
		public IEnumerator<byte> GetMemoryEnumerator() => memory.AsEnumerable().GetEnumerator();

		/// <summary>
		/// Writes the current memory pointed to by the memory pointer
		/// as output using the program's specified <see cref="IOutput"/>.
		/// </summary>
		public void WriteOutput() => Output.WriteOutput(memory[memoryPointer]);

	}
}
