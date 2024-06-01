namespace BudgetOrDie{
	public class SaveData{
		public string Name{get; set;}
		public List<BudgetItem> Items {get; set;}

		public SaveData(List<BudgetItem> items, string name){
			Name = name;
			Items = items;
		}
	}
}
