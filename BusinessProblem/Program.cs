using System;
using System.Collections.Generic;
using System.IO;

namespace GreenTechFolderCreator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("GreenTech Solutions - Systém pro vytváření projektových složek");
            Console.WriteLine("===========================================================");

            try
            {
                // Vytvoření výchozí šablony
                FolderTemplate template = CreateDefaultTemplate();
                
                bool exit = false;
                while (!exit)
                {
                    Console.WriteLine("\nVyberte akci:");
                    Console.WriteLine("1 - Vytvořit složky pro nový projekt");
                    Console.WriteLine("2 - Upravit šablonu složek");
                    Console.WriteLine("3 - Zobrazit aktuální šablonu");
                    Console.WriteLine("0 - Ukončit aplikaci");

                    string choice = Console.ReadLine();

                    switch (choice)
                    {
                        case "1":
                            CreateNewProject(template);
                            break;
                        case "2":
                            EditTemplate(ref template);
                            break;
                        case "3":
                            DisplayTemplate(template);
                            break;
                        case "0":
                            exit = true;
                            break;
                        default:
                            Console.WriteLine("Neplatná volba. Zkuste to znovu.");
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Nastala chyba: {ex.Message}");
            }
        }

        static FolderTemplate CreateDefaultTemplate()
        {
            // Vytvoření výchozí šablony
            return new FolderTemplate
            {
                BaseFolders = new List<Folder>
                {
                    new Folder 
                    {
                        Name = "Dokumentace",
                        Subfolders = 
                        {
                            new Folder { Name = "Technická dokumentace" },
                            new Folder { Name = "Smlouvy" },
                            new Folder { Name = "Reporty" }
                        }
                    },
                    new Folder
                    {
                        Name = "Materiály",
                        Subfolders = 
                        {
                            new Folder { Name = "Specifikace" },
                            new Folder { Name = "Kalkulace" }
                        }
                    },
                    new Folder
                    {
                        Name = "Komunikace",
                        Subfolders = 
                        {
                            new Folder { Name = "Klient" },
                            new Folder { Name = "Interní" }
                        }
                    },
                    new Folder { Name = "Fotografie" }
                }
            };
        }

        static void CreateNewProject(FolderTemplate template)
        {
            Console.WriteLine("\nZadejte název nového projektu:");
            string projectName = Console.ReadLine();
            
            if (string.IsNullOrWhiteSpace(projectName))
            {
                Console.WriteLine("Název projektu nemůže být prázdný.");
                return;
            }

            Console.WriteLine("\nZadejte cestu, kde se má projekt vytvořit (prázdné pro aktuální adresář):");
            string basePath = Console.ReadLine();
            
            if (string.IsNullOrWhiteSpace(basePath))
            {
                basePath = Directory.GetCurrentDirectory();
            }

            try
            {
                // Vytvoření hlavní složky projektu
                string projectPath = Path.Combine(basePath, projectName);
                
                if (Directory.Exists(projectPath))
                {
                    Console.WriteLine($"Složka '{projectName}' již existuje. Chcete ji přepsat? (a/n)");
                    if (Console.ReadLine().ToLower() != "a")
                    {
                        Console.WriteLine("Vytváření projektu zrušeno.");
                        return;
                    }
                    Directory.Delete(projectPath, true);
                }
                
                Directory.CreateDirectory(projectPath);
                Console.WriteLine($"Vytvořena hlavní složka projektu: {projectPath}");

                // Vytvoření podsložek podle šablony
                foreach (var folder in template.BaseFolders)
                {
                    CreateFolderStructure(folder, projectPath);
                }

                Console.WriteLine($"Projekt '{projectName}' byl úspěšně vytvořen na cestě: {projectPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Chyba při vytváření projektu: {ex.Message}");
            }
        }

        static void CreateFolderStructure(Folder folder, string parentPath)
        {
            string newPath = Path.Combine(parentPath, folder.Name);
            Directory.CreateDirectory(newPath);
            Console.WriteLine($"Vytvořena složka: {newPath}");

            foreach (var subfolder in folder.Subfolders)
            {
                CreateFolderStructure(subfolder, newPath);
            }
        }

        static void EditTemplate(ref FolderTemplate template)
        {
            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("\nÚprava šablony:");
                Console.WriteLine("1 - Přidat hlavní složku");
                Console.WriteLine("2 - Přidat podsložku");
                Console.WriteLine("3 - Odstranit složku");
                Console.WriteLine("0 - Zpět do hlavního menu");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddMainFolder(ref template);
                        break;
                    case "2":
                        AddSubfolder(ref template);
                        break;
                    case "3":
                        RemoveFolder(ref template);
                        break;
                    case "0":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Neplatná volba. Zkuste to znovu.");
                        break;
                }
            }
        }

        static void AddMainFolder(ref FolderTemplate template)
        {
            Console.WriteLine("Zadejte název nové hlavní složky:");
            string folderName = Console.ReadLine();
            
            if (string.IsNullOrWhiteSpace(folderName))
            {
                Console.WriteLine("Název složky nemůže být prázdný.");
                return;
            }

            if (template.BaseFolders.Exists(f => f.Name.Equals(folderName, StringComparison.OrdinalIgnoreCase)))
            {
                Console.WriteLine($"Složka s názvem '{folderName}' již existuje.");
                return;
            }

            template.BaseFolders.Add(new Folder { Name = folderName });
            Console.WriteLine($"Hlavní složka '{folderName}' byla přidána do šablony.");
        }

        static void AddSubfolder(ref FolderTemplate template)
        {
            DisplayTemplate(template);
            
            Console.WriteLine("\nZadejte číslo nadřazené složky:");
            if (!int.TryParse(Console.ReadLine(), out int parentIndex) || parentIndex < 1 || parentIndex > template.BaseFolders.Count)
            {
                Console.WriteLine("Neplatný index složky.");
                return;
            }

            Folder parentFolder = template.BaseFolders[parentIndex - 1];
            Console.WriteLine($"Přidávání podsložky do '{parentFolder.Name}'");
            
            Console.WriteLine("Zadejte název nové podsložky:");
            string folderName = Console.ReadLine();
            
            if (string.IsNullOrWhiteSpace(folderName))
            {
                Console.WriteLine("Název složky nemůže být prázdný.");
                return;
            }
            
            if (parentFolder.Subfolders.Exists(f => f.Name.Equals(folderName, StringComparison.OrdinalIgnoreCase)))
            {
                Console.WriteLine($"Podsložka s názvem '{folderName}' již existuje v '{parentFolder.Name}'.");
                return;
            }

            parentFolder.Subfolders.Add(new Folder { Name = folderName });
            Console.WriteLine($"Podsložka '{folderName}' byla přidána do '{parentFolder.Name}'.");
        }

        static void RemoveFolder(ref FolderTemplate template)
        {
            DisplayTemplate(template);
            
            Console.WriteLine("\nZadejte číslo složky, kterou chcete odstranit:");
            if (!int.TryParse(Console.ReadLine(), out int folderIndex) || folderIndex < 1 || folderIndex > template.BaseFolders.Count)
            {
                Console.WriteLine("Neplatný index složky.");
                return;
            }

            string folderName = template.BaseFolders[folderIndex - 1].Name;
            template.BaseFolders.RemoveAt(folderIndex - 1);
            Console.WriteLine($"Složka '{folderName}' byla odstraněna ze šablony.");
        }

        static void DisplayTemplate(FolderTemplate template)
        {
            Console.WriteLine("\nAktuální struktura šablony:");
            
            for (int i = 0; i < template.BaseFolders.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {template.BaseFolders[i].Name}");
                DisplaySubfolders(template.BaseFolders[i].Subfolders, "  ");
            }
        }

        static void DisplaySubfolders(List<Folder> subfolders, string indent)
        {
            foreach (var folder in subfolders)
            {
                Console.WriteLine($"{indent}└─ {folder.Name}");
                DisplaySubfolders(folder.Subfolders, indent + "  ");
            }
        }
    }

    class FolderTemplate
    {
        public List<Folder> BaseFolders { get; set; } = new List<Folder>();
    }

    class Folder
    {
        public string Name { get; set; }
        public List<Folder> Subfolders { get; set; } = new List<Folder>();
    }
}