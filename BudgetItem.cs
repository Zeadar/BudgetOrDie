namespace BudgetOrDie{
	public class BudgetItem{
		public Int64 Money {get; set;}
		public bool Expense {
			get {
				return Money < 0;
			}
		}

		//Expense if < 0, otherwise income.
		public BudgetItem(Int64 money){
			Money = money;
		}

        public override string ToString()
        {
            return Money.ToString();
        }
            
	}
}
