fn main() {
    csbindgen::Builder::default()
        .input_extern_file("../c/src/lib.rs")
        .input_extern_file("../c/src/error.rs")
        .input_extern_file("../c/src/value.rs")
        .input_extern_file("../c/src/style_enums.rs")
        .input_extern_file("../c/src/tree.rs")
        .input_extern_file("../c/src/style.rs")
        .input_extern_file("src/csbindgen_ffi.rs")
        .csharp_dll_name("ctaffy")
        .csharp_namespace("Taffy")
        .csharp_class_name("NativeMethods")
        .csharp_class_accessibility("public")
        .csharp_type_rename(|name| {
            if name == "TaffyTree" { "TaffyNativeTree".to_string() } else { name.to_string() }
        })
        .generate_csharp_file("include/NativeMethods.g.cs")
        .unwrap();
}
