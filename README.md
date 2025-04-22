# GreenTech Solutions - Projektový správce složek

![.NET Tests](https://github.com/inneman/interview-task/actions/workflows/dotnet.yml/badge.svg)

Konzolová aplikace v C# pro vytváření strukturovaných projektových složek pro společnost GreenTech Solutions.

## O projektu

Tato aplikace řeší problém manuálního vytváření adresářové struktury pro projekty společnosti GreenTech Solutions. Automatizace tohoto procesu umožňuje standardizovat strukturu projektů, snížit chybovost a ušetřit čas při zakládání nových projektů.

## Funkce

- Vytváření standardizované adresářové struktury pro nové projekty
- Možnost úpravy šablony složek (přidávání/odstraňování složek)
- Přehledné zobrazení aktuální struktury složek
- Jednoduché a intuitivní uživatelské rozhraní

## Technické detaily

- Jazyk: C#
- Typ aplikace: Konzolová
- .NET verze: 9.0
- Testy: MSTest

## Struktura adresářů

Výchozí šablona vytváří následující strukturu:

```
Projekt/
├── Dokumentace/
│   ├── Technická dokumentace/
│   ├── Smlouvy/
│   └── Reporty/
├── Materiály/
│   ├── Specifikace/
│   └── Kalkulace/
├── Komunikace/
│   ├── Klient/
│   └── Interní/
└── Fotografie/
```

## Jak používat

1. Spusťte aplikaci
2. Vyberte možnost vytvoření nového projektu
3. Zadejte název projektu
4. Zadejte cestu, kde se má projekt vytvořit
5. Aplikace vytvoří kompletní strukturu složek

## Vývoj a testování

Projekt obsahuje sadu automatizovaných testů, které ověřují hlavní funkce aplikace. Testy se automaticky spouštějí po každém pushy do repozitáře pomocí GitHub Actions.

### Spuštění testů lokálně

```bash
cd BusinessProblem.Tests
dotnet test
```

## Autor

Šimon Inneman
