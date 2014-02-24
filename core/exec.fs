[<AutoOpen>]
module xake.exec

open System
open System.Diagnostics
open FSharp.Control.Observable // for GuardedAwaitObservable from FSharx.Core

// TODO make common logging facility
let private trace (msg:string) = Console.WriteLine(msg) |> ignore
let private trace_error (err:Object) = Console.Error.WriteLine(err) |> ignore

let private quote s =
  if String.IsNullOrEmpty s then ""
  elif s.Contains " " then "\"" + s + "\""
  else s
  
let private format_args (args: (string)list) =
  String.Join(" ", args |> List.map quote |> List.toArray)

let exec program args (workingDir:String) =
  async {
    let cl = format_args args
    // TODO use current directory if needed
    let wd = if String.IsNullOrEmpty(workingDir) then "" else workingDir
    let si = ProcessStartInfo(program, UseShellExecute = false,
      RedirectStandardError = true, RedirectStandardOutput = true, 
      WindowStyle = ProcessWindowStyle.Hidden,
      WorkingDirectory = wd, 
      Arguments = cl)
    use p = new Process(StartInfo = si)
    p.ErrorDataReceived.Add(fun e -> if e.Data <> null then trace_error e.Data)
    p.OutputDataReceived.Add(fun e -> if e.Data <> null then trace e.Data)
    p.Start() |> ignore
    p.BeginOutputReadLine()
    p.BeginErrorReadLine()
    // attaches handler to Exited event, enables raising events, then awaits event
    // the event gets triggered even if process has already finished
    let! _ = Async.GuardedAwaitObservable p.Exited (fun _ -> p.EnableRaisingEvents <- true)
    return p.ExitCode
  }
