using System;

public class Module2 {
	public string GetName(){
		var mod1 = new Module1();
		Console.WriteLine(mod1.GetName());
		return "Module2";
	}
}