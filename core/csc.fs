module xake.csc

open System;

type Debug=Full|PdbOnly|Boolean

// c# module spec
type CSharpModule = {
  mutable name: string;
  mutable out: string option;
  mutable deps: List<Async<string>>;
  mutable refs: List<string>;
  mutable debug: Debug option;
  mutable optimize: bool option;
  mutable define: string option;
  mutable unsafe: bool option;
  mutable platform: string option;
  mutable target: string option;
  mutable nologo: bool option;
  mutable nostdlib: bool option;
  // checked is reserved keyword!
  mutable checked_enabled: bool option;
  mutable keycontainer: string option;
  mutable keyfile: string option;
  mutable main: string option;
  mutable warn: int option;
}

// returns async function to build given module spec
let csc (spec:CSharpModule) =
  let run = 
    async {
      // await deps
      // Async.Parallel
      // exec csc.exe
      spec.out
    }
  run
