namespace BudgetOrDie{
	public class ColorWriter{
		static public void RedLine(string input){
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine(input);
			Console.ResetColor();
		}

		static public void GreenLine(string input){
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine(input);
			Console.ResetColor();
		}

		static public void YellowPrompt(string input){
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.Write(input.PadLeft(25));
			Console.Write(" > ");
			Console.ResetColor();
		}
	}
}
