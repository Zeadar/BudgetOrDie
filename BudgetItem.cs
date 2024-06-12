namespace BudgetOrDie{
	public class BudgetItem{
		public DateOnly Date {get;}
		public Int64 Money {get;}
		public string Note {get;}
		public bool Expense {
			get {
				return Money < 0;
			}
		}

        public override string ToString(){
			return Math.Abs(Money).ToString();
        }
                
		//Expense if < 0, otherwise income.
		public BudgetItem(Int64 money, DateOnly date, string note){
			Money = money;
			Date = date; //Limiting characters so the printed budget won't be offset :)
			Note = note.Substring(0, Math.Min(note.Length, 17));
		}
	}
}
