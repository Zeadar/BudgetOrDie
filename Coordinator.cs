using System.Text.Json;

namespace BudgetOrDie{
	public class Coordinator{
		List<BudgetItem> budgetItems = new List<BudgetItem>();

		//UInt32 data type to prevent accidental negative number
		public void AddExpense(UInt32 expense){
			budgetItems.Add(new BudgetItem(-(int)expense));
		}

		public void AddIncome(UInt32 income){
			budgetItems.Add(new BudgetItem((income)));
		}

		public string Name {get; set;} = "New Budget";

		public void PrintBudget(){
			
		}

		public string Serialize(){
			var saveData = new SaveData(budgetItems, Name);
			return JsonSerializer.Serialize(saveData);
		}

		public void Deserialize(string json){
			SaveData? input;

			try {
				input = JsonSerializer.Deserialize<SaveData>(json);
			} catch (Exception ex){
				ColorWriter.RedLine(ex.Message);
				return;
			}

			if (input == null){
				ColorWriter.RedLine("JSON parsed successfully but the result is null.");
				return;
			}

			Name = input.Name;
			budgetItems.Clear();
			foreach (var item in input.Items){
				budgetItems.Add(item);
			}
			ColorWriter.GreenLine("Successfully parsed data");
		}
	}
}
