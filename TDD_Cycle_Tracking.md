# TDD Cycle Tracking — Mastermind

## Project Overview
.NET C# console Mastermind game. Two-project solution: game implementation + xUnit tests.

## Solution Structure
```
Mastermind/
  Mastermind.sln
  Mastermind/          ← console app (net9.0)
  Mastermind.Tests/    ← xUnit test project (net9.0)
```

## TDD Cycle Plan

| Cycle | Feature | Key Classes/Methods |
|-------|---------|-------------------|
| 1 | Feedback engine | `FeedbackResult`, `FeedbackEvaluator.Evaluate(secret, guess)` |
| 2 | Secret code generation | `CodeGenerator.Generate(length, colors)` |
| 3 | Game state & rules | `Game`: attempts tracking, win/loss detection |
| 4 | Input parsing & validation | `InputParser.Parse(input)`, validate peg count & color names |
| 5 | Console UI & game loop | `Program` / `ConsoleUI`: full playable game |

---

## Cycle 1 — Feedback Engine

### Phase: 🔴 Red ✅
**Goal:** Tests for `FeedbackEvaluator` that scores a guess against a secret code.

#### Tests Written (`Mastermind.Tests/FeedbackEvaluatorTests.cs`)
| Test | Expected Result |
|------|----------------|
| `AllCorrect_Returns4ExactAnd0Color` | `(4, 0)` |
| `AllWrong_Returns0And0` | `(0, 0)` |
| `AllColorOnlyMatches_Returns0ExactAnd4Color` | `(0, 4)` |
| `MixedResult_Returns2ExactAnd1Color` | `(2, 1)` |
| `DuplicatesInSecret_NoDoubleCount` | `(0, 3)` |
| `DuplicatesInGuess_NoDoubleCount` | `(1, 0)` |

#### Status
- [x] Solution structure scaffolded
- [x] Tests written
- [x] Tests confirmed failing (`NotImplementedException`)

---

### Phase: 🟢 Green ✅
**Goal:** Implement `FeedbackEvaluator.Evaluate` to make all 6 tests pass.

#### Implementation (`Mastermind/FeedbackEvaluator.cs`)
Two-pass algorithm:
1. Walk positions — exact match → `exactMatches++`; otherwise tally into `secretFreq` / `guessFreq` dictionaries
2. Color matches = `∑ min(secretFreq[c], guessFreq[c])` for each color in guess

All 6 tests pass.

---

### Phase: 🔵 Blue ✅
**Refactoring applied to `Mastermind/FeedbackEvaluator.cs`:**
- Exact-match loop now collects unmatched pegs into `unmatchedSecret` / `unmatchedGuess` lists
- Extracted `private static CountFrequencies(IEnumerable<string>)` helper — consolidates dictionary-building, reusable in future cycles

All 6 tests remain green.

---

## Cycle 2 — Secret Code Generation

### Phase: 🔴 Red ✅
**Goal:** Tests for `CodeGenerator.Generate(string[] availableColors, int codeLength)`.

#### Tests Written (`Mastermind.Tests/CodeGeneratorTests.cs`)
| Test | Requirement |
|------|------------|
| `Generate_ReturnsArrayOfCorrectLength` | Array length == `codeLength` |
| `Generate_AllPegsAreFromAvailableColors` | All elements in `availableColors` |
| `Generate_WithSingleColor_ReturnsAllSameColor` | Only color → all pegs same |
| `Generate_WithLengthOne_ReturnsSingleElement` | Single element array |
| `Generate_ThrowsWhenColorsEmpty` | `ArgumentException` on empty input |

### Phase: 🟢 Green ✅
**Implementation (`Mastermind/CodeGenerator.cs`):** Null/empty guard + random selection loop (`Random.Shared.Next`). All 5 tests pass.

### Phase: 🔵 Blue ✅
**Refactoring:** Replaced private `static readonly Random _random` field with `Random.Shared` — idiomatic .NET 6+ pattern, no instance state.

---

## Cycle 3 — Game State & Rules

### Phase: 🔴 Red ✅
#### Tests Written (`Mastermind.Tests/GameTests.cs`)
| Test | Requirement |
|------|------------|
| `MakeGuess_CorrectGuess_ReturnsIsWonTrue` | `IsWon=true`, `ExactMatches==4` |
| `MakeGuess_WrongGuess_DoesNotWin` | `IsWon=false` |
| `MakeGuess_TracksAttemptCount` | `AttemptsMade==2` after 2 guesses |
| `MakeGuess_LastAttemptAndWrong_IsGameOverTrue` | `IsGameOver=true, IsWon=false` |
| `MakeGuess_WinOnLastAttempt_IsWonAndGameOver` | `IsWon=true, IsGameOver=true` |
| `MakeGuess_AfterGameOver_ThrowsInvalidOperationException` | `InvalidOperationException` |
| `MakeGuess_FeedbackIsCorrect` | Feedback matches evaluator output |

### Phase: 🟢 Green ✅
**Implementation (`Mastermind/Game.cs`):** Constructor stores secret + maxAttempts. `MakeGuess` guards on game-over, calls `FeedbackEvaluator`, increments counter, computes win/game-over flags, returns `GuessResult`.

### Phase: 🔵 Blue ✅
**Refactoring:** Removed redundant default-value assignments (`_attemptsMade = 0`, `_isGameOver = false`) from constructor.

---

## Cycle 4 — Input Parsing & Validation

### Phase: 🔴 Red ✅
#### Tests Written (`Mastermind.Tests/InputParserTests.cs`)
| Test | Requirement |
|------|------------|
| `Parse_ValidInput_ReturnsSuccess` | Valid input → `Success` |
| `Parse_CaseInsensitive_NormalizesToValidColorName` | Mixed case → canonical casing |
| `Parse_TooFewTokens_ReturnsFailure` | Wrong count → `Failure` |
| `Parse_TooManyTokens_ReturnsFailure` | Wrong count → `Failure` |
| `Parse_InvalidColor_ReturnsFailure` | Unknown color → `Failure` |
| `Parse_EmptyInput_ReturnsFailure` | Empty string → `Failure` |
| `Parse_ValidInput_PegsMatchExact` | Pegs match canonical `validColors` entry |

### Phase: 🟢 Green ✅
**Implementation (`Mastermind/InputParser.cs`):** Whitespace split → length check → OrdinalIgnoreCase dictionary lookup → canonical peg array or Failure.

### Phase: 🔵 Blue ✅
**Refactoring:** Replaced `(char[])null` cast with `Array.Empty<char>()` + `TrimEntries`; replaced manual dictionary loop with `validColors.ToDictionary(...)`.

---

## Cycle 5 — Console UI & Game Loop

### Phase: 🔴 Red ✅
**Goal:** Tests for `GameRunner` — the full playable game loop via `IGameIO`.

#### Tests Written (`Mastermind.Tests/GameRunnerTests.cs`)
| Test | Requirement |
|------|------------|
| `Run_PlayerWinsOnFirstGuess_PrintsWinMessage` | Win message printed on correct guess |
| `Run_PlayerExhaustsAttempts_PrintsLoseMessage` | Loss message + secret revealed |
| `Run_InvalidInput_PromptsAgainAndEventuallyWins` | Error printed, loop continues |
| `Run_PrintsFeedbackAfterEachGuess` | Exact/Color feedback line output |
| `Run_ShowsRemainingAttemptsAfterEachGuess` | Attempts made printed after each guess |

### Phase: 🟢 Green ✅
**Implementation (`Mastermind/GameRunner.cs`):** Constructor injects `IGameIO`, `Game`, `validColors`, `codeLength`. `Run()` prints welcome/color list, loops reading input via `InputParser`, calls `Game.MakeGuess`, prints feedback and win/loss messages.

**Supporting additions:**
- `IGameIO` interface (`IGameIO.cs`) — `WriteLine` + `ReadLine`
- `ConsoleIO` class (`ConsoleIO.cs`) — wraps `Console` for production
- `Program.cs` wired up with `CodeGenerator` + `ConsoleIO` + `GameRunner`

### Phase: 🔵 Blue ✅
**Refactoring applied to `Mastermind/GameRunner.cs`:**
- Removed `guessNumber` local variable — was incremented after the guess but only used in the prompt, creating a slight mismatch with `guessResult.AttemptsMade`
- Simplified prompt from `$"Enter guess {guessNumber + 1}:"` to `"Enter guess:"` — removes redundant counter; attempt count is already surfaced in the `"Attempts made: N"` output line sourced from `guessResult.AttemptsMade`
- `?? string.Empty` null-coalescing on `ReadLine()` retained as a defensive boundary guard

All 5 `GameRunnerTests` remain green.

---

## Project Complete ✅

All 5 TDD cycles finished. Full test suite green.
