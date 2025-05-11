# FootballStatsAPI ⚽

FootballStatsAPI je RESTful API razvijen u ASP.NET Core (.NET 8) za upravljanje statistikom fudbalskih utakmica i igrača. 
Sistem omogućava unos rezultata utakmica, praćenje individualne statistike igrača i pregled podataka u strukturiranom i efikasnom formatu.

## 🧩 Funkcionalnosti

- ✅ Unos rezultata odigranih utakmica putem API endpointa (`POST /api/matches/results`)
- ✅ Automatsko ažuriranje statistike svakog igrača nakon unosa rezultata
- ✅ Pregled ukupne i individualne statistike igrača
- ✅ RESTful arhitektura spremna za proširenje (timovi, sezone, tabele...)

## 🔧 Tehnologije

- ASP.NET Core 8
- C#
- Entity Framework Core
- SQL Server (ili InMemory za testiranje)
- Swagger (OpenAPI dokumentacija)
