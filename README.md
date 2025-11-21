# Baubit.Collections


[![CircleCI](https://dl.circleci.com/status-badge/img/circleci/TpM4QUH8Djox7cjDaNpup5/2zTgJzKbD2m3nXCf5LKvqS/tree/master.svg?style=svg)](https://dl.circleci.com/status-badge/redirect/circleci/TpM4QUH8Djox7cjDaNpup5/2zTgJzKbD2m3nXCf5LKvqS/tree/master)
[![codecov](https://codecov.io/gh/pnagoorkar/Baubit.Collections/branch/master/graph/badge.svg)](https://codecov.io/gh/pnagoorkar/Baubit.Collections)
[![NuGet](https://img.shields.io/nuget/v/Baubit.Collections.svg)](https://www.nuget.org/packages/Baubit.Collections/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

Thread-safe collection types for .NET 9 that fill gaps in the standard library.

## Why?

I needed a thread-safe list with indexed access and enumeration support. `ConcurrentBag<T>` doesn't provide ordering or indexing, and `List<T>` with manual locking is error-prone. So I built `ConcurrentList<T>`.

## ConcurrentList<T>

A thread-safe `IList<T>` implementation using `ReaderWriterLockSlim` for efficient concurrent reads with exclusive writes.

```csharp
var list = new ConcurrentList<int>();

// Multiple threads can read simultaneously
Parallel.For(0, 100, i => {
    var count = list.Count;
    var item = list[0];
});

// Writes are exclusive and thread-safe
Parallel.For(0, 100, i => list.Add(i));

// Safe enumeration (uses snapshot)
foreach (var item in list) {
    list.Add(999); // Won't affect the enumeration
}
```

## Installation

```bash
dotnet add package Baubit.Collections
```

## Testing

77 tests with 86% line coverage and 100% branch coverage, including extensive thread-safety tests.

---

More collections coming as needed. MIT Licensed.
