#r @"..\core\bin\Debug\xake.dll"
open xake.fileset
open xake.exec
open xake.csc

// dir of demo project
let dir = @"c:\pets\xake\demo"

// check fileset
let files = fileset ["**/*.cs"] dir
printfn "%A" files

// TODO check exec
// exec("python.exe", [], "") |> ignore

// TODO write build script for demo project
