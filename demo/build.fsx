#r @"..\core\bin\Debug\xake.dll"
open xake

// dir of demo project
let dir = @"c:\pets\xake\demo"

// check fileset
let files = fileset ["**/*.cs"] dir
printfn "%A" files

// TODO write build script for demo project
