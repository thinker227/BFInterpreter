namespace BFInterpreter.Parsers {
	/// <summary>
	/// Represents a BF symbol parser.
	/// </summary>
	public interface ISymbolParser {

		/// <summary>
		/// The symbol corresponding to the action of this <see cref="ISymbolParser"/>.
		/// </summary>
		public char Symbol { get; }

		/// <summary>
		/// Parses a symbol and performs a corresponding action.
		/// </summary>
		/// <param name="interpreter">The <see cref="Interpreter"/> which is handling the current symbol.</param>
		public void Parse(Interpreter interpreter);

	}
}
