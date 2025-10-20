# Anweisungen für KI-Agenten (AGENTS.md) - C#/.NET

Dieses Dokument enthält Richtlinien für die Entwicklung in diesem Projekt. Bitte befolge sie, um konsistenten, wartbaren und qualitativ hochwertigen Code zu erstellen.

## 1. Projekt-Kontext

- **Projekt:** Backend-Service für eine Ticket-Management-Plattform.
- **Zweck:** Verwalten von Tickets, Benutzern und Kommentaren über eine REST-API.
- **Technologie-Stack:** .NET 8, ASP.NET Core für die Web-API, Entity Framework Core (EF Core) für die Persistenz, PostgreSQL als Datenbank, NuGet für das Paketmanagement.
- **Architektur:** 3-Schicht-Architektur (Controller, Service, Repository/DbContext). Die gesamte Geschäftslogik befindet sich **ausschließlich** in der Service-Schicht.

## 2. Code-Style und Konventionen

- **Formatierung:** Wir erzwingen den Code-Style über die `.editorconfig`-Datei im Repository. Nutze die Standard-Formatierungstools von Visual Studio oder JetBrains Rider.
- **Namenskonventionen (Microsoft Standard):**
  - Klassen, Interfaces, Enums, Methoden & Properties: `PascalCase` (z.B. `TicketService`, `GetTicketByIdAsync`).
  - Lokale Variablen & Methodenparameter: `camelCase` (z.B. `var ticketId = ...`).
  - Private Felder: `_camelCase` (z.B. `private readonly ITicketRepository _ticketRepository;`).
  - Konstanten (`const`): `PascalCase`.
- **Sprachidiome (C#):**
  - Nutze **LINQ** (Language Integrated Query) für Datenabfragen und -manipulationen anstelle von manuellen Schleifen.
  - Verwende **`async` und `await`** für alle I/O-gebundenen Operationen (Datenbankzugriffe, HTTP-Calls). Methoden, die `await` verwenden, müssen das Suffix `Async` tragen (z.B. `SaveTicketAsync`).
  - Nutze **Nullable Reference Types**, um die Abwesenheit eines Wertes explizit zu machen (`string?`). Gib `null` nur zurück, wenn es unumgänglich ist. Bevorzuge leere Collections oder spezifische Response-Objekte.
  - Verwende **`record`**-Typen für unveränderliche Datenübertragungsobjekte (DTOs).
- **Dokumentation:** Alle öffentlichen Typen und Member müssen mit **XML-Dokumentationskommentaren (`///`)** auf **Englisch** dokumentiert werden.

## 3. Git-Workflow & Commit-Messages

- **Commit-Format:** Folge dem **Conventional Commits** Standard.
  - `feat`: Ein neues Feature.
  - `fix`: Ein Bugfix.
  - `docs`: Änderungen an der Dokumentation.
  - `refactor`: Code-Änderungen, die weder ein Feature noch ein Bugfix sind.
  - `test`: Hinzufügen oder Korrigieren von Tests.
  - Beispiel: `feat(ticket): allow users to add attachments to tickets`
- **Branching:** Entwickle neue Features in Branches namens `feature/<issue-id>-<kurzbeschreibung>`.

## 4. Test-Strategie

- **Frameworks:** **xUnit.net** für Unit- und Integrationstests, **Moq** oder **NSubstitute** zum Mocken von Abhängigkeiten.
- **Test-Struktur:** Tests befinden sich in einem separaten Testprojekt, das auf das Hauptprojekt verweist (z.B. `MyProject.Api.Tests`).
- **Namenskonvention:** Tests folgen dem `Methode_Sollte_Verhalten_Wenn_Bedingung`-Muster.
  - Beispiel: `CreateTicketAsync_Should_ThrowException_WhenTitleIsEmpty`.
- **Prinzip:** Schreibe Unit-Tests für die Service-Schicht und mocke dabei die Repository/DbContext-Abhängigkeiten. Integrationstests können eine In-Memory-Datenbank verwenden.

## 5. Wichtige Prinzipien & No-Gos

- **Prinzipien:** Halte dich an SOLID und DRY (Don't Repeat Yourself).
- **Sicherheit:** Validiere alle Daten, die über API-Endpunkte eingehen, mithilfe von **Data Annotations** (`[Required]`, `[StringLength]`, etc.) auf den DTO-Klassen.
- **No-Gos:**
  - **Kein direkter `DbContext` im Controller:** Injiziere niemals den `DbContext` oder Repository-Implementierungen direkt in einen Controller. Der Datenfluss ist immer: Controller -> Service -> Repository/DbContext.
  - **Vermeide blockierende Aufrufe:** Verwende **niemals** `.Result` oder `.Wait()` auf `Task`-Objekten. Nutze `async` und `await` durchgängig ("async all the way").
  - **Keine Geschäftslogik in Controllern:** Controller sind nur für die Entgegennahme von Anfragen, Validierung und das Delegieren an die Service-Schicht zuständig.