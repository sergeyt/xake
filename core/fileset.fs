module xake

open System
open System.Linq
open System.IO

let private identity x = x
let private (+/) path1 path2 = Path.Combine(path1, path2)

// globbing matcher - finds all files in given directory matched to specified pattern
let private glob (cwd:String) (pattern:String) = 
  // utils
  let search dir pattern all =
    let opt = if all then SearchOption.AllDirectories else SearchOption.TopDirectoryOnly
    let files = Directory.EnumerateFiles(dir, pattern, opt)
    Seq.toList files
  // use process current directory if cwd is not specified
  let root_dir = if String.IsNullOrEmpty(cwd) then Environment.CurrentDirectory else cwd
  let i = pattern.IndexOf('*')
  if i >= 0 then // is pattern?
    // extract search pattern
    let i2 = pattern.LastIndexOf('/')
    // TODO handle \ also
    let search_pattern = if i2 >= 0 then pattern.Substring(i2 + 1) else pattern
    // get root dir
    let start_dir = pattern.Substring(0, i)
    let dir = root_dir +/ start_dir
    let all = i + 1 < pattern.Length && pattern.[i + 1] = '*'
    search dir search_pattern all
  else
    // TODO ensure file exist
    [pattern]

// TODO make async, try asyncSeq
// fileset - returns files matches to given patterns
let fileset patterns cwd = 
  // utils
  let startsWith prefix (s:String) = s.StartsWith(prefix)
  let substr startIndex (s:String) = s.Substring(startIndex)
  let exclude a b = Seq.toList(Enumerable.Except(a, b))
  let globs patterns cwd =
    let results = patterns |> List.map (glob cwd)
    List.collect identity results
  // use process current directory if cwd is not specified
  let dir = if String.IsNullOrEmpty(cwd) then Environment.CurrentDirectory else cwd
  // be safe, filter empty patterns
  let pattern_list = patterns |> List.filter (String.IsNullOrEmpty >> not)
  // get include patterns
  let includes = pattern_list |> List.filter (startsWith "!" >> not)
  // get exclude patterns
  let excludes = pattern_list |> List.filter (startsWith "!") |> List.map (substr 1)
  // get all files
  let all_files = globs includes dir
  let excluded_files = globs excludes dir
  exclude all_files excluded_files
