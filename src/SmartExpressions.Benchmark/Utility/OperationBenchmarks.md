```
BenchmarkDotNet v0.15.8, Windows 11 (10.0.22621.4317/22H2/2022Update/SunValley2)
12th Gen Intel Core i7-1255U 2.60GHz, 1 CPU, 12 logical and 10 physical cores
.NET SDK 10.0.103
  [Host]     : .NET 9.0.13 (9.0.13, 9.0.1326.6317), X64 RyuJIT x86-64-v3
  DefaultJob : .NET 9.0.13 (9.0.13, 9.0.1326.6317), X64 RyuJIT x86-64-v3
```
| Method                         | Mean      | Error     | StdDev    | Median    | P95       | Allocated |
|------------------------------- |----------:|----------:|----------:|----------:|----------:|----------:|
| Ok_SmallClass                  | 1.9393 ns | 0.0356 ns | 0.0316 ns | 1.9313 ns | 1.9973 ns |         - |
| Ok_SmallClass_Null             | 0.2670 ns | 0.0191 ns | 0.0178 ns | 0.2676 ns | 0.2920 ns |         - |
| Fail_SmallClass                | 0.1142 ns | 0.0283 ns | 0.0265 ns | 0.1045 ns | 0.1612 ns |         - |
| Fail_SmallClass_With_Exception | 1.8950 ns | 0.0725 ns | 0.0642 ns | 1.8996 ns | 2.0008 ns |         - |
| Ok_LargeClass                  | 1.8083 ns | 0.0460 ns | 0.0384 ns | 1.7963 ns | 1.8786 ns |         - |
| Fail_LargeClass                | 0.1201 ns | 0.0344 ns | 0.0556 ns | 0.0991 ns | 0.2280 ns |         - |
| DirectCtor_SmallClass          | 3.7827 ns | 0.1477 ns | 0.4239 ns | 3.8751 ns | 4.3209 ns |         - |
| DirectCtor_Fail_SmallClass     | 0.5234 ns | 0.0709 ns | 0.2023 ns | 0.5008 ns | 0.9657 ns |         - |
| Ok_NonGeneric                  | 0.0507 ns | 0.0362 ns | 0.1046 ns | 0.0000 ns | 0.3202 ns |         - |
| Fail_NonGeneric                | 0.0168 ns | 0.0183 ns | 0.0532 ns | 0.0000 ns | 0.1774 ns |         - |
| Fail_With_Exception_NonGeneric | 1.3830 ns | 0.0561 ns | 0.0498 ns | 1.3851 ns | 1.4532 ns |         - |
| Ok_Generic                     | 0.0281 ns | 0.0198 ns | 0.0165 ns | 0.0221 ns | 0.0544 ns |         - |
| Fail_Generic                   | 0.0740 ns | 0.0368 ns | 0.1019 ns | 0.0295 ns | 0.3135 ns |         - |
| Fail_Generic_With_Exception    | 1.6454 ns | 0.0691 ns | 0.1055 ns | 1.6252 ns | 1.8339 ns |         - |
