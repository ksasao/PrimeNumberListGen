# Prime Number List Generator

Generate huge lists of prime numbers to your disk.
Implemented in C# (.NET 5).

## Download

If you haven't installed .NET 5, [download .NET 5.0 Runtime](https://aka.ms/dotnet-core-applaunch?framework=Microsoft.NETCore.App&framework_version=5.0.0&arch=x64&rid=win10-x64).

- [Windows binary (v1.0)](https://github.com/ksasao/PrimeNumberListGen/releases/download/v1.0/PListGen_v1.0.zip)

  
## Usage

```txt
> plistgen [digits(1-18),default=7] [output_path,default=./PrimeNumber]
```

## Generate Example

To support random access (e.g., to quickly find the nth prime number), prime numbers are padded with zeros.


```txt
000000000002
000000000003
000000000005
000000000007
000000000011
000000000013
:
999999999899
999999999937
999999999959
999999999961
999999999989
```

If you generate a list of prime numbers with more than 9 digits, the file will be split.

```txt
000100000000.txt
000200000000.txt
000300000000.txt
:
```

## Required Resources

### Core i7-11700 (16 cores) / Mem 128 GB

| Digits | Largest Prime Number | Time | File Size |
| --- | --- | --- | --- |
| 7|     9999991| <1s |5.7 MB|
| 8|    99999989|   3s| 56 MB|
| 9|   999999937|  14s|533 MB|
|10|  9999999967|   2m|  5 GB|
|11| 99999999977|  20m| 50 GB|
|12|999999999989|   3h|490 GB|

## Algorithm

Sieve of Eratosthenes for parallel processing.
