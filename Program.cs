using BudgetOrDie;
using System.Runtime.InteropServices;

//  ___         _             _                ___  _      
// | . > _ _  _| | ___  ___ _| |_   ___  _ _  | . \<_> ___ 
// | . \| | |/ . |/ . |/ ._> | |   / . \| '_> | | || |/ ._>
// |___/`___|\___|\_. |\___. |_|   \___/|_|   |___/|_|\___.
//                <___'                                    

Console.BackgroundColor = ConsoleColor.Black;
Console.ForegroundColor = ConsoleColor.Cyan;
Console.WriteLine(" ___         _             _                ___  _      ");
Console.WriteLine("| . > _ _  _| | ___  ___ _| |_   ___  _ _  | . \\<_> ___ ");
Console.ForegroundColor = ConsoleColor.DarkCyan;
Console.WriteLine("| . \\| | |/ . |/ . |/ ._> | |   / . \\| '_> | | || |/ ._>");
Console.ForegroundColor = ConsoleColor.Blue;
Console.WriteLine("|___/`___|\\___|\\_. |\\___. |_|   \\___/|_|   |___/|_|\\___.");
Console.ForegroundColor = ConsoleColor.DarkBlue;
Console.WriteLine("               <___'                                    ");
Console.ResetColor();

Coordinator coordinator = new Coordinator();

MainMenu();

void MainMenu(){
	while(true){
		Console.WriteLine("Enter [_] to select an option.");
		Console.WriteLine("[N]ew budget");
		Console.WriteLine("[L]oad budget");
		Console.WriteLine("[Q]uit");

		try {
			//Can't break the while loop in swtch case
			//So long live if cases in a row!
			string input = Query("[N] [L] [Q]");
			if (input == "N"){
				if (CrateNew()){ //CreateNew() & Load() returns a true if loaded or created successfully
					break;
				}
			} else if (input == "L"){
				if (Load()){					
					break;
				}
			} else if (input == "Q"){
				Quit();
			} else {
				throw new Exception($"Unrecognised input {input}");
			}
		} catch(Exception ex){
			ColorWriter.RedLine(ex.Message);
		}
	}

	while(true){
		Console.WriteLine("Enter [_] to select an option.");
		Console.WriteLine("[E]edit budget");
		Console.WriteLine("[P]rint Budget");
		Console.WriteLine("[S]ave budget");
		Console.WriteLine("[N]ew budget");
		Console.WriteLine("[L]oad budget");
		Console.WriteLine("[Q]uit");
		try {
			string input = Query("[E] [P] [S] [N] [L] [Q]");
			switch(input){
				case "N":
					CrateNew();
					break;
				case "E":
					EditBudget();
					break;
				case "P":
					PrintBudget();
					break;
				case "S":
					Save();
					break;
				case "L":
					Load();
					break;
				case "Q":
					Quit();
					break;
				default:
					throw new Exception($"Unrecognised input {input}");
			}
		} catch(Exception ex){
			ColorWriter.RedLine(ex.Message);
		}
	}
}

void EditBudget(){
	while(true){
		Console.WriteLine("[P]rint Budget");
		Console.WriteLine("[A]dd new item");
		string input = Query("[P] [A] [Q]");

		if (input == "Q"){
			break;
		}

		try {
			switch(input){
				case "P":
					PrintBudget();
					break;
				case "A":
					AddItem();
					break;
				default:
					throw new Exception($"Unrecognised input {input}");
			}			
		} catch (Exception ex){
			ColorWriter.RedLine(ex.Message);
		}
	}
}

void PrintBudget(){
	coordinator.PrintBudget(DateOnly.FromDateTime(DateTime.Now));
}

void AddItem(){
	bool expense = false;
	UInt32 amount = 0;
	string note = "";

	while (true){
		Console.WriteLine("[I]income");
		Console.WriteLine("[E]xpense");
		string input = Query("[I] [E] [Q]");

		if (input == "Q"){
			return;
		}

		if (input == "I"){
			expense = false;
			break;
		} else if (input == "E"){
			expense = true;
			break;
		} else {
			ColorWriter.RedLine($"Unrecognised input {input}");
		}
	}

	while (true){
		Console.WriteLine($"Enter note about the {(expense ? "expense" : "income")} [string]");
		Console.WriteLine("Enter blank line for empty note");
		string input = Query("[string] [Q]");

		if (input == "Q"){
			return;
		}

		if (input == ""){
			break;
		}

		try {
			note = Capitalize(input);
			break;
		} catch (Exception ex){
			ColorWriter.RedLine(ex.Message);
		}
	}

	while (true){
		Console.WriteLine("Enter amount [UInt]");
		string input = Query("[UInt] [Q]");

		if (input == "Q"){
			return;
		}

		try {
			amount = Convert.ToUInt32(input);
			break;
		} catch (Exception ex){
			ColorWriter.RedLine(ex.Message);
		}
	}

	while (true){
		Console.WriteLine("Enter date [yyyy-MM-dd]");
		Console.WriteLine("Enter [MM-dd] assuming current year");
		Console.WriteLine("Enter [dd] assuming current year and current month");
		string input = Query("[yyyy-MM-dd] [MM-dd] [dd] [Q]");

		if (input == "Q"){
			return;
		}

		try {
			DateOnly date;
			if (input.Length == 5){
				date = DateOnly.FromDateTime(Convert.ToDateTime($"{DateTime.Now.Year}-{input}"));
			} else if (input.Length < 3){
				date = DateOnly.FromDateTime(Convert.ToDateTime($"{DateTime.Now.Year}-{DateTime.Now.Month}-{input}"));
			} else {
				date = DateOnly.FromDateTime(Convert.ToDateTime(input));
			}

			if (expense){
				coordinator.AddExpense(amount, date, note);
			} else {
				coordinator.AddIncome(amount, date, note);
			}
			ColorWriter.GreenLine($"Added {(expense ? "expense" : "income")} {amount} on {date.ToString("yyyy-MM-dd").Replace("/", "-")}");
			break;
		} catch (Exception ex){
			ColorWriter.RedLine(ex.Message);
		}
	}

}

bool CrateNew(){
	while (true){
		try {
			Console.WriteLine("Enter budget name");
			string name = Capitalize(Query("[name] [Q]"));
			if (name == "Q"){
				return false;
			}
			coordinator = new Coordinator();
			coordinator.Name = name;
			ColorWriter.GreenLine($"Crated budget {coordinator.Name}");
			return true;
		} catch (Exception ex){
			ColorWriter.RedLine(ex.Message);
		}
	}
}

void Quit(){
	Console.WriteLine("Are you sure?");
	if (Query("[Y]?") == "Y"){
		Environment.Exit(0);
	}
}

bool Load(){
	Console.WriteLine("Enter [#] to load file");

	List<string> jsonPaths = Directory.GetFiles(Directory.GetCurrentDirectory())
		.Where(fullPath => fullPath.Contains(".json")
			&& !fullPath.Contains("BudgetOrDie.deps.json")
			&& !fullPath.Contains("BudgetOrDie.runtimeconfig.json")).ToList();

	if (jsonPaths.Count == 0){
		ColorWriter.RedLine("No JSON file found!");
		ColorWriter.RedLine("Looks like you have to create a new budget instead :)");
		return false;
	}

	int currentIndex = 1;
	List<int> indexes = new List<int>();

	foreach (string fullPath in jsonPaths){
		string fileName = RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
			? fullPath.Split("\\").Last()
			: fullPath.Split("/").Last();

		indexes.Add(currentIndex);
		Console.WriteLine($"[{currentIndex++}] {fileName}");
	}

	string prompt = indexes.Select(i => $"[{i}] ").Aggregate((a, b) => a + b) + "[Q]";

	while (true){
		try {
			string input = Query(prompt);

			if (input == "Q"){
				return false;
			}

			int fileIndex = Convert.ToInt32(input) - 1;
			string json = File.ReadAllText(jsonPaths[fileIndex]);
			coordinator.Deserialize(json);
			return true;
		} catch (Exception ex){
			ColorWriter.RedLine("JSON parsing issue!");
			ColorWriter.RedLine(ex.Message);
		}
	}
}

void Save(){
	string json = coordinator.Serialize();
	string fileName = $"{coordinator.Name.ToLower()}.json";
	string path = Path.Combine(Environment.CurrentDirectory, fileName);

	try {
		File.WriteAllText(path, json);
	} catch (Exception ex){
		ColorWriter.RedLine(ex.Message);
	}

	ColorWriter.GreenLine($"Saved {coordinator.Name} as {path}");
}

string Query(string question){
	ColorWriter.YellowPrompt(question);
	string input = Console.ReadLine() ?? "";
    Console.WriteLine();
    return input.Trim().ToUpper();
}

string Capitalize(string input){
	return $"{input.Substring(0, 1).ToUpper()}{input.Substring(1).ToLower()}";
}

