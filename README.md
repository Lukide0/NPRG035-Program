# NPRG035-Program

Zápočtový program pro předmět NPRG035.

## Popis

Terminálová todo aplikace s UI a časovači na jednotlivé úkoly.

## Požadavky

- .NET 8

## Instalace

### Postup A

1. **Stáhněte sestavenou verzi** z GitHubu (sekce Releases).  
   - Otevřete webovou stránku repozitáře na GitHubu.
   - Přejděte na záložku **Releases**.
   - Stáhněte soubor (např. `portable.zip`).
2. **Rozbalte soubor**.  
   - Klikněte pravým tlačítkem na stažený `.zip` soubor a vyberte **Extrahovat vše**.
   - Vyberte složku, kam chcete soubory rozbalit, a potvrďte.

### Postup B

1. **Naklonujte repozitář** `git clone https://github.com/Lukide0/NPRG035-Program.git`
2. **Otevřete ve Visual Studiu** – Otevřete soubor `.sln`.
3. **Obnovte NuGet balíčky** – Klikněte pravým tlačítkem na řešení a vyberte **Restore NuGet Packages**.
4. **Vyberte konfiguraci Release** - V horní liště klikněte na rozbalovací seznam vedle tlačítka pro spuštění a vyberte Release.
5. **Sestavte projekt** – Stiskněte `Ctrl + Shift + B` pro sestavení bez spuštění.
6. **Aplikace** - Aplikace se nachází ve složce `bin/Release/net8.0`

### Postup C

1. **Naklonujte repozitář** `git clone https://github.com/Lukide0/NPRG035-Program.git`
2. **Otevřete projekt** – Přejděte do složky projektu: `cd NPRG035-Program`
3. **Obnovte NuGet balíčky** `dotnet restore`
4. **Sestavte projekt bez spuštění** `dotnet build --configuration Release`
5. **Aplikace** - Aplikace se nachází ve složce `bin/Release/net8.0`


## Příkazy

```
> task_tracker --help
Description:
  Todo application

Usage:
  task_tracker [command] [options]

Options:
  -v, --verbose   Verbose output
  --no-color      No colors [default: False]
  --version       Show version information
  -?, -h, --help  Show help and usage information

Commands:
  ui                        Show UI.
  add, new                  Add task
  edit, update <task_id>    Edit task
  delete, remove <task_id>  Remove task
  filter                    Filter tasks
  timer                     Timer commands
```

### UI

Místo CLI, aplikace nabízí také UI, které obsahuje stejnou funkcionalitu.

### Přidání nového úkolu

```
> task_tracker add --help
Description:
  Add task

Usage:
  task_tracker add [options]

Options:
  --name <name> (REQUIRED)      Task name
  --description <description>   Task description []
  --priority <High|Low|Medium>  Task priority [default: Medium]
  --deadline <deadline>         Task deadline []
  -v, --verbose                 Verbose output
  --no-color                    No colors [default: False]
  -?, -h, --help                Show help and usage information
```

#### Ukázka

```
> task_tracker add --name "task 1" --description "Something" --priority low
1 # ID nového úkolu
```

### Odstranění úkolu

```
> task_tracker remove --help
Description:
  Remove task

Usage:
  task_tracker remove <task_id> [options]

Arguments:
  <task_id>  Task ID

Options:
  -v, --verbose   Verbose output
  --no-color      No colors [default: False]
  -?, -h, --help  Show help and usage information
```

#### Ukázka

```
> task_tracker remove 5
```

### Upravení úkolu

```
> task_tracker edit --help
Description:
  Edit task

Usage:
  task_tracker edit <task_id> [options]

Arguments:
  <task_id>  Task ID

Options:
  --name <name>                 Task name
  --description <description>   Task description
  --priority <High|Low|Medium>  Task priority
  --deadline <deadline>         Task deadline
  -v, --verbose                 Verbose output
  --no-color                    No colors [default: False]
  -?, -h, --help                Show help and usage information
```

```
> task_tracker edit 5 --name "new name"
```

### Filtrování úkolů

```
> task_tracker filter --help
Description:
  Filter tasks

Usage:
  task_tracker filter [options]

Options:
  --by-id <by-id>                    Filter by ID
  --by-priority <High|Low|Medium>    Filter by priority
  --by-state <Done|InProgress|ToDo>  Filter by state
  --from-date <from-date>            Filter by tasks from date
  --to-date <to-date>                Filter by tasks to date
  --limit <limit>                    Limit results [default: 100]
  --offset <offset>                  Offset results [default: 0]
  -v, --verbose                      Verbose output
  --no-color                         No colors [default: False]
  -?, -h, --help                     Show help and usage information
```

#### Ukázka

```
> task_tracker filter --by-state ToDo --by-priority High
```

### Spuštění/Obnovení časovače

```
> task_tracker timer start 1
```

### Pozastavení časovače

```
> task_tracker timer pause 1
```

### Odstranění časovače

```
> task_tracker timer remove 1
```

### Filtrování časovačů

```
> task_tracker timer filter --help
Description:
  Filter timers

Usage:
  task_tracker timer filter [options]

Options:
  --by-id <by-id>              Filter by ID
  --by-state <Paused|Running>  Filter by state
  --limit <limit>              Limit results [default: 100]
  --offset <offset>            Offset results [default: 0]
  -v, --verbose                Verbose output
  --no-color                   No colors [default: False]
  -?, -h, --help               Show help and usage information
```

#### Ukázka

```
> task_tracker timer filter --by-state running
```

### Konfigurace

Konfigurační soubor `config.json` se nachází ve stejném adresáři jako aplikace. Pokud konfigurační soubor neexistuje, aplikace automaticky vytvoří nový.

## Dokumentace pro vývojáře

[link](./docs/dev.md)
