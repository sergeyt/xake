module xake

open System
open System.IO

let filter_files dir fn =
  async {
    let files = Directory.EnumerateFiles(dir, "*", SearchOption.AllDirectories)
    let filtered = files |> Seq.filter fn
    List.ofSeq filtered
  }

let glob_match path pattern =
  // TODO complete
  true

// TODO make async and faster, maybe using regular expressions
// globbing matcher - finds all files in given directory matched to specified pattern
let glob pattern cwd = 
  async {
    let dir = if String.IsNullOrEmpty(cwd) then Environment.CurrentDirectory else cwd
    // TODO get root dir
    let root_dir = dir
    filter_files root_dir, (fun (f) -> glob_match f pattern)
  }

let globs patterns cwd =
  async {
    let results = patterns |> List.map (fun (p) -> glob p cwd)
    List.collect (fun x -> x), results
  }

// TODO make async
// fileset - returns files matches to given patterns
let fileset pattern_list cwd = 
  async {
    // be safe, filter empty patterns
    let patterns = pattern_list |> List.filter (fun (p:String) -> not(String.IsNullOrEmpty(p)))
    // get include patterns
    let includes = patterns |> List.filter (fun (p:String) -> p[0] != '!')
    // get exclude patterns
    let excludes = patterns |> List.filter (fun (p) -> p[0] == '!') |> List.filter (fun (p) -> p.Substring(1))
    // get all files
    let all_files = globs includes cwd
    let excluded_files = globs excludes cwd
    // TODO subtract excluded_files
    all_files
  }
