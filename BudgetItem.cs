namespace BudgetOrDie{
	public class BudgetItem{
		public int Money {get; set;}
		public bool Expense {
			get {
				return Money < 0;
			}
		}

		//Expense if < 0, otherwise income.
		public BudgetItem(int money){
			Money = money;
		}

        public override string ToString()
        {
            return Money.ToString();
        }
            
	}
}
