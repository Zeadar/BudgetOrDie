using System.Text.Json;

namespace BudgetOrDie{
	public class Coordinator{
		List<BudgetItem> budgetItems = new List<BudgetItem>();

		//UInt16 data type to prevent accidental negative number
		public void AddExpense(UInt16 expense){
			budgetItems.Add(new BudgetItem(-(int)expense));
		}

		public void AddIncome(UInt16 income){
			budgetItems.Add(new BudgetItem((income)));
		}

		public string Serialize(){
			return JsonSerializer.Serialize(budgetItems);
		}

		public void Deserialize(string json){
			List<BudgetItem>? input;

			try {
				input = JsonSerializer.Deserialize<List<BudgetItem>>(json);
			} catch (Exception err){
				ColorWriter.RedLine("JSON parsing failed:");
				ColorWriter.RedLine(err.ToString());
				return;
			}

			if (input == null){
				ColorWriter.RedLine("JSON parsed successfully but the result is null.");
				return;
			}

			budgetItems.Clear();
			foreach (var item in input){
				budgetItems.Add(item);
			}
		}
	}
}
