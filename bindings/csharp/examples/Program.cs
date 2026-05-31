// Taffy C# examples
//
// To run this example:
//   1. Build ctaffy:  cargo build --release --manifest-path bindings/c/Cargo.toml
//   2. Copy the native library next to this executable:
//        Windows:  copy target\release\ctaffy.dll  bindings\csharp\examples
//        Linux:    cp  target/release/libctaffy.so bindings/csharp/examples
//        macOS:    cp  target/release/libctaffy.dylib bindings/csharp/examples
//   3. dotnet run --project bindings/csharp/examples [basic|measure]
//      (omit the argument to run all examples)

var example = args.Length > 0 ? args[0] : null;
switch (example)
{
    case "basic":
        BasicExample.Run();
        break;
    case "measure":
        MeasureExample.Run();
        break;
    default:
        Console.WriteLine("=== basic ===");
        BasicExample.Run();
        Console.WriteLine();
        Console.WriteLine("=== measure ===");
        MeasureExample.Run();
        break;
}
