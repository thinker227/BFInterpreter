using System;

namespace BFInterpreter {
	public class Interpreter {

		public const int DefaultMemorySize = 30000;

		private readonly byte[] memory;
		private int memoryPointer;



		public int MemoryPointer {
			get => memoryPointer;
			private set {
				if (value < 0) value = MemorySize - 1;
				if (value >= MemorySize) value = 0;
				memoryPointer = value;
			}
		}
		public int MemorySize { get; }



		public delegate void ProgramExitEventArgs();
		public event ProgramExitEventArgs OnProgramExit;



		public Interpreter(int memorySize) {
			if (memorySize <= 0) throw new ArgumentException(
				"Memory size can't be less or equal to 0.", nameof(memorySize)
			);

			memory = new byte[memorySize];
			MemorySize = memorySize;
			memoryPointer = 0;
		}

		public Interpreter() : this(DefaultMemorySize) { }

	}
}
