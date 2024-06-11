using BudgetOrDie;

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

Coordinator? coordinator = null;

MainMenu();

void MainMenu(){
	Console.WriteLine("Enter [_] to select an option.");
	Console.WriteLine("[N]ew budget");
	Console.WriteLine("[E]dit budget");
	Console.WriteLine("[P]rint budget");
	Console.WriteLine("[S]ave budget");
	Console.WriteLine("[L]oad budget");
	Console.WriteLine("[Q]uit");

	while(true){
		try {
			string input = Query("[N] [E] [S] [L] [Q]");
			switch(input){
				case "N":
					CrateNew();
					break;
				case "E":
					EditBudget();
					break;
				case "P":
					if (!coordinatorIsNull()){
						coordinator.PrintBudget();
					}
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
	if (coordinatorIsNull()){
		return;
	}
	while(true){
		break;
	}
}

void CrateNew(){
	while (true){
		try {
			Console.WriteLine("Enter budget name");
			string name = Capitalize(Query("[name] [Q]"));
			if (name == "Q"){
				return;
			}
			coordinator = new Coordinator();
			coordinator.Name = name;
			ColorWriter.GreenLine($"Crated budget {coordinator.Name}");
			break;
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

void Load(){
	Console.WriteLine("Enter [#] to load file");

	List<string> jsonPaths = Directory.GetFiles(Directory.GetCurrentDirectory())
		.Where(fullPath => fullPath.Contains(".json")).ToList();

	int index = 1;
	foreach (string fullPath in jsonPaths){
		//ColorWriter.GreenLine(fullPath);
		string fileName = fullPath.Split("/").Last();
		Console.WriteLine($"[{index++}] {fileName}");
	}

	while (true){
		try {
			string input = Query("[#] [Q]");

			if (input == "Q"){
				return;
			}

			int fileIndex = Convert.ToInt32(input) - 1;
			string json = File.ReadAllText(jsonPaths[fileIndex]);
			ColorWriter.GreenLine(json);

			if (coordinator == null){
				coordinator = new Coordinator();
			}
		} catch (Exception ex){
			ColorWriter.RedLine(ex.Message);
		}
	}

	
	// ColorWriter.YellowPrompt("Enter budget name or [Q]");
	// string input = Console.ReadLine() ?? ""; 
	// string fileName = $"{input.Trim().ToLower()}.json";
	// string? json;

	// if (input.Trim().ToUpper() == "Q"){
	// 	return;
	// }

	// try {
	// 	json = File.ReadAllText(fileName);
	// } catch (Exception err){
	// 	ColorWriter.RedLine($"Could not load budget {input.Trim()}]");
	// 	ColorWriter.RedLine($"Error occured while loading file {fileName}:");
	// 	ColorWriter.RedLine(err.ToString());
	// 	Load();
	// 	return;
	// }

	// ColorWriter.GreenLine($"Successfully loaded budget {input.Trim()} from {fileName}");
	// coordinator.Deserialize(json);
}

void Save(){
	if (coordinatorIsNull()){
		return;
	}
	string json = coordinator.Serialize();
	string fileName = $"{coordinator.Name.ToLower()}.json";
	string path = Path.Combine(Environment.CurrentDirectory, fileName);

	try {
		File.WriteAllText(path, json);
	} catch (Exception err){
		ColorWriter.RedLine("Failed saving to file:");
		ColorWriter.RedLine(err.ToString());
	}

	ColorWriter.GreenLine($"Saved {coordinator.Name} as {path}");
}

string Query(string question){
	ColorWriter.YellowPrompt(question);
	string input = Console.ReadLine() ?? "";
	return input.Trim().ToUpper();
}

bool coordinatorIsNull(){
	bool isNull = coordinator == null;
	if (isNull){
		ColorWriter.RedLine("Load or create a new budget!");
	}
	return isNull;
}

string Capitalize(string input){
	return $"{input.Substring(0, 1).ToUpper()}{input.Substring(1).ToLower()}";
}

