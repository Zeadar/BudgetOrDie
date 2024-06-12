using System.Text.Json;

namespace BudgetOrDie{
	public class Coordinator{
		List<BudgetItem> budgetItems = new List<BudgetItem>();

		//UInt32 data type to prevent accidental negative number
		public void AddExpense(UInt32 expense, DateOnly date, string note){
			budgetItems.Add(new BudgetItem(-(int)expense, date, note));
		}

		public void AddIncome(UInt32 income, DateOnly date, string note){
			budgetItems.Add(new BudgetItem(income, date, note));
		}

		public string Name {get; set;} = "New Budget";

		public void PrintBudget(DateOnly date){
			//If null → every month, else → only same month and year
			var currentMonthItems = budgetItems.Where(bi
				=> bi.Date.Month == date.Month
				&& bi.Date.Year == date.Year);
			var expenses = currentMonthItems.Where(di => di.Expense).OrderBy(di => di.Date).ToList();
			var incomes = currentMonthItems.Where(di => !di.Expense).OrderBy(di => di.Date).ToList();
			var amountLines = Math.Max(expenses.Count, incomes.Count);
			var totalIncome = incomes.Select(income => income.Money).Sum();
			var totalExpense = expenses.Select(expense => Math.Abs(expense.Money)).Sum();

			Console.BackgroundColor = ConsoleColor.DarkBlue;
			Console.ForegroundColor = ConsoleColor.White;
			Console.Write($"{date.Year}-{date.Month}".PadRight(25));
			Console.Write("Income".PadLeft(24));
			Console.Write(" ");
			Console.Write("Expense".PadLeft(49));
			Console.Write(" ");
            //Console.Write("\n");
            Console.ResetColor();
            Console.WriteLine();			

			for (int i = 0; i != amountLines; ++i){
				PrintSide(incomes, i);
				PrintSide(expenses, i);
                //Console.Write("\n");
                Console.WriteLine();
            }
			Console.ResetColor();
			Console.BackgroundColor = ConsoleColor.DarkBlue;
			Console.ForegroundColor = ConsoleColor.White;
			Console.Write(" ");
			Console.Write("Total: ".PadLeft(24));
			Console.Write(totalIncome.ToString().PadLeft(24));
			Console.Write(" ");
			Console.Write(totalExpense.ToString().PadLeft(49));
			Console.Write(" ");
            //Console.Write("\n");
            Console.WriteLine();
            Console.Write("Balance: ".PadLeft(25));
			Console.Write(currentMonthItems.Select(bi => bi.Money).Sum().ToString().PadRight(75));
            //Console.Write("\n");
            Console.ResetColor();
            Console.WriteLine();
		}

		void PrintSide(List<BudgetItem> items, int i){
			Console.BackgroundColor = ConsoleColor.Black;
			Console.ForegroundColor = ConsoleColor.White;
			if (i < items.Count){
				var bi = items[i];
				Console.Write(" ");
				Console.Write(bi.Date.ToString("dddd").PadRight(9));
				Console.Write(bi.Date.ToString("MM-dd").PadLeft(13));
				Console.Write(" ");
			} else {
				Console.Write("".PadLeft(24));
			}
			Console.Write(" ");
			Console.BackgroundColor = ConsoleColor.DarkGray;
			Console.ForegroundColor = ConsoleColor.Yellow;
			if (i < items.Count){

				Console.Write(" ");
				Console.Write(items[i].Note.PadRight(17));
				Console.Write(items[i].ToString().PadLeft(6));
			} else {					
				Console.Write("".PadLeft(24));
			}
			Console.Write(" ");
		}

		public string Serialize(){
			var saveData = new SaveData(budgetItems, Name);
			return JsonSerializer.Serialize(saveData);
		}

		public void Deserialize(string json){
			SaveData? input = JsonSerializer.Deserialize<SaveData>(json);

			if (input == null){
				throw new Exception("JSON parsed successfully but the result is null.");
			}

			Name = input.Name;
			budgetItems.Clear();
			foreach (var item in input.Items){
				budgetItems.Add(item);
			}
			ColorWriter.GreenLine($"Successfully parsed data for budget \"{Name}\"");
		}
	}
}
