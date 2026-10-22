# Recovered source code – ElfaWin report & import tools

Recovered on 2026-10-20 by decompiling the compiled programs in the `Reports` folder
(ILSpy 9.1 engine). The original code was C# (`M:\PROJECTS\ElfaWin\...`), so this is C#.


| 

## How to open / build
- Open any `.csproj` in Visual Studio (2017 or later) and build. Each project is an old-style
  .NET Framework console app (12 target .NET 2.0, the rest .NET 4.0). If VS asks to retarget
  the .NET 2.0 ones, choose .NET Framework 4.x — the code compiles fine there.
- `app.config` in each project is the original `*.exe.config` (connection strings, email settings).
- Excel projects (InternetReport, UnpaidOrders, TCSreport, TCSreport_b) reference
  `lib\Microsoft.Office.Interop.Excel.dll`, copied from the original program folder.

## What is NOT 100% identical to the original
- **Comments** are not stored in compiled programs, so they cannot be recovered.
- Where no matching .pdb existed (3 projects above), local variable names are generic
  (`text`, `num`, …). Logic is unaffected.
- Code layout/formatting is regenerated; some constructs (e.g. `foreach`, `using`) may be
  written slightly differently from the original but behave the same.
- Code is emitted as C# 5 for compatibility with older Visual Studio versions.
