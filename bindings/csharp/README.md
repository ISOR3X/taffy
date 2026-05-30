# C# API for Taffy (csharp-taffy)

Taffy is a flexible, high-performance, cross-platform UI layout library written in Rust.

This directory contains C# bindings for Taffy. The API is built on top of the C bindings and uses [csbindgen](https://github.com/Cysharp/csbindgen/) to generate C# declarations.

## Examples

There are readable examples in the examples directory.

Assuming you have Rust and Cargo installed (and a C compiler), then this should work to run the basic example:

```bash
git clone https://github.com/ISOR3X/taffy.git
cargo build --release -p ctaffy
cd bindings/csharp
dotnet run --project examples/basic
```

The project copies `ctaffy.dll` (Windows), `libctaffy.so` (Linux), or `libctaffy.dylib` (macOS) from `target/release/` into the output directory automatically — no manual copy step is needed.