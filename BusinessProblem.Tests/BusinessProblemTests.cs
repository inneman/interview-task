using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Collections.Generic;

namespace BusinessProblem.Tests
{
    [TestClass]
    public class BusinessProblemTests
    {
        private string testDir = "";

        [TestInitialize]
        public void Initialize()
        {
            // Vytvoření testovacího adresáře v aktuálním adresáři projektu
            testDir = Path.Combine(Directory.GetCurrentDirectory(), "TestTemp");
            
            // Zajistíme, že adresář existuje
            if (Directory.Exists(testDir))
            {
                Directory.Delete(testDir, true);
            }
            
            Directory.CreateDirectory(testDir);
        }

        [TestCleanup]
        public void Cleanup()
        {
            // Úklid po každém testu
            if (Directory.Exists(testDir))
            {
                try
                {
                    Directory.Delete(testDir, true);
                }
                catch (IOException)
                {
                    Console.WriteLine($"Varování: Nelze smazat adresář {testDir}");
                }
            }
        }

        [TestMethod]
        public void Test_CreateDefaultTemplate_HasExpectedFolders()
        {
            // Arrange & Act
            var template = CreateDefaultTemplate();
            
            // Assert
            Assert.IsNotNull(template);
            Assert.IsNotNull(template.BaseFolders);
            Assert.AreEqual(4, template.BaseFolders.Count);
            
            // Kontrola konkrétních složek
            var dokumentaceFolder = template.BaseFolders.Find(f => f.Name == "Dokumentace");
            Assert.IsNotNull(dokumentaceFolder);
            Assert.AreEqual(3, dokumentaceFolder.Subfolders.Count);
            
            var materialyFolder = template.BaseFolders.Find(f => f.Name == "Materiály");
            Assert.IsNotNull(materialyFolder);
            Assert.AreEqual(2, materialyFolder.Subfolders.Count);
        }

        [TestMethod]
        public void Test_AddMainFolder_AddsFolder()
        {
            // Arrange
            var template = CreateDefaultTemplate();
            int originalCount = template.BaseFolders.Count;
            
            // Act
            AddMainFolder(ref template, "TestovaciSlozka");
            
            // Assert
            Assert.AreEqual(originalCount + 1, template.BaseFolders.Count);
            var newFolder = template.BaseFolders.Find(f => f.Name == "TestovaciSlozka");
            Assert.IsNotNull(newFolder);
            Assert.AreEqual(0, newFolder.Subfolders.Count);
        }

        [TestMethod]
        public void Test_AddSubfolder_AddsSubfolder()
        {
            // Arrange
            var template = CreateDefaultTemplate();
            var parentFolder = template.BaseFolders[0]; // První složka (Dokumentace)
            int originalCount = parentFolder.Subfolders.Count;
            
            // Act
            AddSubfolder(parentFolder, "TestovaciPodsložka");
            
            // Assert
            Assert.AreEqual(originalCount + 1, parentFolder.Subfolders.Count);
            var newSubfolder = parentFolder.Subfolders.Find(f => f.Name == "TestovaciPodsložka");
            Assert.IsNotNull(newSubfolder);
        }

        [TestMethod]
        public void Test_RemoveFolder_RemovesFolder()
        {
            // Arrange
            var template = CreateDefaultTemplate();
            int originalCount = template.BaseFolders.Count;
            
            // Act
            template.BaseFolders.RemoveAt(0); // Odstraní první složku
            
            // Assert
            Assert.AreEqual(originalCount - 1, template.BaseFolders.Count);
        }

        [TestMethod]
        public void Test_CreateFolderStructure_CreatesAllFolders()
        {
            // Arrange
            var template = CreateDefaultTemplate();
            string projectPath = Path.Combine(testDir, "TestProject");
            
            // Act
            Directory.CreateDirectory(projectPath);
            
            // Vytvoření složek podle šablony
            foreach (var folder in template.BaseFolders)
            {
                CreateFolderStructure(folder, projectPath);
            }
            
            // Assert
            Assert.IsTrue(Directory.Exists(projectPath));
            Assert.IsTrue(Directory.Exists(Path.Combine(projectPath, "Dokumentace")));
            Assert.IsTrue(Directory.Exists(Path.Combine(projectPath, "Dokumentace", "Technická dokumentace")));
            Assert.IsTrue(Directory.Exists(Path.Combine(projectPath, "Materiály")));
            Assert.IsTrue(Directory.Exists(Path.Combine(projectPath, "Komunikace")));
            Assert.IsTrue(Directory.Exists(Path.Combine(projectPath, "Fotografie")));
        }

        // Pomocné metody pro testování (zjednodušené verze z hlavního programu)
        
        private FolderTemplate CreateDefaultTemplate()
        {
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
        
        private void AddMainFolder(ref FolderTemplate template, string folderName)
        {
            if (string.IsNullOrWhiteSpace(folderName))
            {
                return;
            }

            if (template.BaseFolders.Exists(f => f.Name.Equals(folderName, StringComparison.OrdinalIgnoreCase)))
            {
                return;
            }

            template.BaseFolders.Add(new Folder { Name = folderName });
        }
        
        private void AddSubfolder(Folder parentFolder, string folderName)
        {
            if (string.IsNullOrWhiteSpace(folderName))
            {
                return;
            }
            
            if (parentFolder.Subfolders.Exists(f => f.Name.Equals(folderName, StringComparison.OrdinalIgnoreCase)))
            {
                return;
            }

            parentFolder.Subfolders.Add(new Folder { Name = folderName });
        }
        
        private void CreateFolderStructure(Folder folder, string parentPath)
        {
            string newPath = Path.Combine(parentPath, folder.Name);
            Directory.CreateDirectory(newPath);

            foreach (var subfolder in folder.Subfolders)
            {
                CreateFolderStructure(subfolder, newPath);
            }
        }
    }

    // Kopie tříd z hlavního projektu pro účely testování
    public class FolderTemplate
    {
        public List<Folder> BaseFolders { get; set; } = new List<Folder>();
    }

    public class Folder
    {
        public string Name { get; set; } = string.Empty;
        public List<Folder> Subfolders { get; set; } = new List<Folder>();
    }
}