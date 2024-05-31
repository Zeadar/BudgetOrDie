using BudgetOrDie;

// Console.WriteLine(
// """
//  ___         _             _                ___  _      
// | . > _ _  _| | ___  ___ _| |_   ___  _ _  | . \<_> ___ 
// | . \| | |/ . |/ . |/ ._> | |   / . \| '_> | | || |/ ._>
// |___/`___|\___|\_. |\___. |_|   \___/|_|   |___/|_|\___.
//                <___'                                    
// """
// );

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

var coordinator = new Coordinator();

MainMenu();

void MainMenu(){
	Console.WriteLine("Enter \"N\" to make new budget");
	Console.WriteLine("Enter \"S\" to save budget");
	Console.WriteLine("Enter \"L\" to load budget");
	Console.WriteLine("Enter \"Q\" to quit");
	while(true){
		switch(Query("[N] [S] [L] [Q]")){
			case "N":
				//TODO
				break;
			case "S":
				//TODO
				break;
			case "L":
				Load();
				break;
			case "Q":
				Quit();
				break;
			default:
				break;
		}

	}
}

void Quit(){
	Console.WriteLine("Are you sure?");
	if (Query("[y]?") == "Y"){
		Environment.Exit(0);
	}
	//No return or call. Will just run through
	//MainMenu() case structure
}

void Load(){
	ColorWriter.YellowPrompt("Enter budget name or [Q]");
	string input = Console.ReadLine() ?? ""; 
	string fileName = $"{input.Trim().ToLower()}.json";
	string json = "";

	if (input.Trim().ToUpper() == "Q"){
		return;
	}

	try {
		json = File.ReadAllText(fileName);
	} catch (Exception err){
		ColorWriter.RedLine($"Could not load budget {input.Trim()}]");
		ColorWriter.RedLine($"Error occured while loading file {fileName}:");
		ColorWriter.RedLine(err.ToString());
		Load();
		return;
	}

	coordinator.Deserialize(json);
}

string Query(string question){
	ColorWriter.YellowPrompt(question);
	string input = Console.ReadLine() ?? "";
	return input.Trim().ToUpper();
}
