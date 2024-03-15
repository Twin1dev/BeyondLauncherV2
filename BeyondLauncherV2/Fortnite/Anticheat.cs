using BeyondLauncherV2.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using CUE4Parse.FileProvider;
using CUE4Parse.UE4.VirtualFileSystem;
using CUE4Parse.UE4.Assets;
using CUE4Parse.UE4.Readers;
using UE4Config.Parsing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BeyondLauncherV2.Fortnite
{
    internal class Anticheat
    {
        public static string GetFileHash(string FilePath)
        {
            if (File.Exists(FilePath))
            {
                using (SHA256 sha256 = SHA256.Create())
                {
                    using (FileStream stream = File.OpenRead(FilePath))
                    {
                        byte[] hash = sha256.ComputeHash(stream);

                        string hashStr = BitConverter.ToString(hash).Replace("-", "").ToLower();

                        return hashStr;
                    }
                }
            }

            return "";
        }

        public static bool Scan(out string pakstring)
        {
            bool result = false;
            string path = Settings.Default.Path;
            string paks = path + "\\FortniteGame\\Content\\Paks";
            var provider = new DefaultFileProvider(paks, SearchOption.TopDirectoryOnly, true, new CUE4Parse.UE4.Versions.VersionContainer(CUE4Parse.UE4.Versions.EGame.GAME_UE4_25));
            provider.Initialize();
            provider.SubmitKey(new CUE4Parse.UE4.Objects.Core.Misc.FGuid(), new CUE4Parse.Encryption.Aes.FAesKey("0x2713E24A338C7E8BF1A50E3F1987F33BB151F04B192E89E940A623AB34F8502F"));

            var dups = provider.Files.GroupBy(x => x.Value.Path).Where(y => y.Count() > 1).Select(z => z.Key).ToList();
            var modded = provider.Files.GroupBy(x => x.Value.Path).Select(z => z.Key).ToList();
            var dupsName = provider.Files.GroupBy(x => x.Value.Name).Where(y => y.Count() > 1).Select(z => z.Key).ToList();
            var gober = provider.MountedVfs.ToList();

            foreach (var pak in gober)
            {
                if (!pak.IsEncrypted)
                {
                    var pakname = pak.Name;
                    var files = pak.Files.GroupBy(x => x.Value.Path).Select(z => z.Key).ToList();
                    foreach (var file in files)
                    {
                        var fileLowerd = file.ToLower();
                        var withoutExt = fileLowerd.Split('.')[0];
                        var uasset = withoutExt + ".uasset";
                        var ubulk = withoutExt + ".ubulk";
                        var uexp = withoutExt + ".uexp";
                        FArchive uassetReader = null;
                        FArchive uexpReader = null;

                        if (pak.Files.Any(x => x.Key == uasset))
                            uassetReader = new FStreamArchive(uasset, new MemoryStream(pak.Extract((VfsEntry)pak.Files.First(x => x.Key == uasset).Value)), new CUE4Parse.UE4.Versions.VersionContainer(CUE4Parse.UE4.Versions.EGame.GAME_UE4_25));

                        if (pak.Files.Any(x => x.Key == uexp))
                            uexpReader = new FStreamArchive(uexp, new MemoryStream(pak.Extract((VfsEntry)pak.Files.First(x => x.Key == uexp).Value)), new CUE4Parse.UE4.Versions.VersionContainer(CUE4Parse.UE4.Versions.EGame.GAME_UE4_25));

                        if (uassetReader == null || uexpReader == null)
                            continue;

                        var package = new Package(
                            uassetReader,
                            uexpReader,
                           (FArchive)null,
                            null,
                            provider);

                        var exportTypeData = package.ExportsLazy.Any(x => x.Value.ExportType.Contains("DataTable"));

                        if (exportTypeData)
                        {
                            provider.UnloadAllVfs();
                            provider.Dispose();
                            pakstring = pak.Name;
                            return true;
                        }
                    }
                }
            }


            foreach (var dup in dups)
            {
                var dupLowered = dup.ToLower();
                var pakswithDup = provider.MountedVfs.Where(x => x.Files.ContainsKey(dup.ToLower()));

                if (!pakswithDup.Any())
                    continue;

                foreach (var pak in pakswithDup)
                {
                    var withoutExt = dupLowered.Split('.')[0];
                    var uasset = withoutExt + ".uasset";
                    var ubulk = withoutExt + ".ubulk";
                    var uexp = withoutExt + ".uexp";
                    FArchive uassetReader = null;
                    FArchive uexpReader = null;

                    if (pak.Files.Any(x => x.Key == uasset))
                        uassetReader = new FStreamArchive(uasset, new MemoryStream(pak.Extract((VfsEntry)pak.Files.First(x => x.Key == uasset).Value)), new CUE4Parse.UE4.Versions.VersionContainer(CUE4Parse.UE4.Versions.EGame.GAME_UE4_25));

                    if (pak.Files.Any(x => x.Key == uexp))
                        uexpReader = new FStreamArchive(uexp, new MemoryStream(pak.Extract((VfsEntry)pak.Files.First(x => x.Key == uexp).Value)), new CUE4Parse.UE4.Versions.VersionContainer(CUE4Parse.UE4.Versions.EGame.GAME_UE4_25));
                    if (uassetReader is null)
                    {
                        continue;
                    }

                    var package = new Package(
                        uassetReader,
                        uexpReader,
                       (FArchive)null,
                        null,
                        provider);
                    var exportType = package.ExportsLazy.Any(x => x.Value.ExportType.Contains("Blueprint") || x.Value.ExportType.Contains("DataTable"));


                    var exportTypeData = package.ExportsLazy.Any(x => x.Value.ExportType.Contains("DataTable"));

                    if (exportType)
                    {
                        provider.UnloadAllVfs();
                        provider.Dispose();
                        pakstring = pak.Name;
                        return true;
                    }

                    if (exportTypeData)
                    {
                        provider.UnloadAllVfs();
                        provider.Dispose();
                        pakstring = pak.Name;
                        return true;
                    }
                }
            }
            provider.UnloadAllVfs();
            provider.Dispose();

            pakstring = "";
            return result;
        }


    }


}
