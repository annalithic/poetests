using PoeFormats;
using PoeFormats.Rows;
using Newtonsoft.Json.Linq;

namespace PoeTests {
    internal class Program {

        static void Main(string[] args) {
            Tgm tsgts = new Tgm(@"E:\Extracted\PathOfExile2\0.2.0\art\models\terrain\maps\uniqueselenite\abyss\tgms\cc_01_c1r1.tgm"); 
            return;

            Tsi tsi2 = new Tsi(@"E:\Extracted\PathOfExile2\Day5\metadata\terrain\maps\penitentiary\master.tsi");
            foreach(string tgt in tsi2.GetTileGeometries(@"E:\Extracted\PathOfExile2\Day5")) Console.WriteLine(tgt);
            var overrides = tsi2.GetMaterialOverrides();
            foreach(string key in overrides.Keys) Console.WriteLine(key + "-> " + overrides[key]);
            return;

            string gamefolder = @"E:\Extracted\PathOfExile2\Day5\";



            //Tdt tdt2 = new Tdt(@"E:\Extracted\PathOfExile2\Day5\metadata\terrain\maps\mire\tiles\mirefeaturehut_01.tdt");
            //return;

            foreach (string tsi in Directory.EnumerateFiles(gamefolder + @"metadata\terrain\", "*.tsi", SearchOption.AllDirectories)) {
                Tsi t = new Tsi(tsi);
            }
            return;

            ListTimelineAnimations(@"E:\Extracted\PathOfExile2\0.1.1c\"); return;

            //FindHiddenFilesInTDT(@"E:\Extracted\PathOfExile2\Day5"); return;

            //s.GenerateCode(@"E:\Extracted\PathOfExile2\0.1.1c\data", @"E:\Extracted\PathOfExile2\Codegen\0.1.1c.Rows.cs"); return;


            //DatCompare(@"E:\Extracted\PathOfExile2\0.1.1c\data", @"E:\Extracted\PathOfExile2\0.1.1\data"); return;
            DatStringDump(@"E:\Projects\dat-schema\dat-schema", @"E:\Extracted\PathOfExile2\0.1.1\data", false, "caalt"); return;
            Schema s = new Schema(@"E:\Projects\dat-schema\dat-schema");

            foreach (string file in Directory.EnumerateFiles(@"E:\Extracted\PathOfExile2\2Schema", "*.gql")) {
                string tablename = Path.GetFileNameWithoutExtension(file);
                if (s.GetTable(tablename) == null) File.Copy(file, Path.Combine(@"E:\Projects\dat-schema\dat-schema\poe2\undone", Path.GetFileName(file)));
            }
            return;

            Tgm asd = new Tgm(@"E:\Extracted\PathOfExile2\Day5\art\models\terrain\islands\tiles\island\cavethickwall\tgms\stmm_01_c1r1.tgm");
            return;

            foreach(string dat in Directory.EnumerateFiles(@"F:\Extracted\PathOfExile\3.25.2\data", "*.dat64", SearchOption.AllDirectories)) {
                byte[] a = File.ReadAllBytes(dat);
                byte[] b = File.ReadAllBytes(dat.Replace(".dat", ".datc"));
                if(a.Length != b.Length) {
                    Console.WriteLine(dat);
                    break;
                }
                for(int i = 0; i < a.Length; i++) {
                    if (a[i] != b[i]) {
                        Console.WriteLine(dat);
                        break;
                    }
                }
                //Console.WriteLine(Path.GetFileNameWithoutExtension(dat));
            }
            return;

            //Database d = new Database(@"E:\Extracted\PathOfExile\3.25.Settlers\data");
            //d.LoadEverything(); return;

            Dictionary<string, Dat> dats = new Dictionary<string, Dat>();

            List<string> moreData = new List<string>();
            foreach(string datPath in Directory.EnumerateFiles(@"E:\Extracted\PathOfExile\3.25.Settlers\data", "*.dat64")) {
                string datName = Path.GetFileNameWithoutExtension(datPath);
                if(!dats.ContainsKey(datName)) {
                    dats[datName] = new Dat(datPath);
                }
                    
                Dat dat = dats[datName];

                if(s.TryGetEnum(datName, out var e)) {
                    Console.WriteLine($"{dat.rowWidth} {e.name}");
                }
                

                //if (datName != "characterstartqueststate") continue;
                var table = s.GetTable(datName);
                if (table == null) continue;
                var columns = table.columns;


                bool tableHasError = false;

                int columnsEnd = columns.Length > 0 ? columns[columns.Length - 1].offset + columns[columns.Length - 1].Size() : 0;
                int distToEnd = dat.rowWidth - columnsEnd;
                if(distToEnd != 0) {
                    moreData.Add($"{datName} has {distToEnd} more bytes to row end");
                }


                for (int i = 0; i < columns.Length; i++) {
                    var column = columns[i];
                    int maxRef = 100000;
                    if (column.references != null) {
                        string refTable = column.references.ToLower();
                        if (!dats.ContainsKey(refTable)) {
                            string refTablePath = Path.Combine(Path.GetDirectoryName(datPath), refTable + ".dat64");
                            if (File.Exists(refTablePath)) dats[refTable] = new Dat(refTablePath);
                            else {
                                Console.WriteLine("COULD NOT FIND FILE " + refTablePath);
                                goto COULDNOTFINDDAT;
                            }
                        }
                        maxRef = dats[refTable].rowCount - 1;
                    }
                    COULDNOTFINDDAT:
                    //TODO analyse only for specific type????
                    DatAnalysis.Error error = DatAnalysis.AnalyseColumn(dat, column, maxRef);
                    if(error != DatAnalysis.Error.NONE) {
                        tableHasError = true;
                        Console.WriteLine($"{datName} {column.TypeName()} AT {column.offset} {error}");
                    }
                }
                if (tableHasError) Console.WriteLine();
            }

            Console.WriteLine();
            foreach(string line in moreData) Console.WriteLine(line);

            return;

            return;

            Dat dsd = new Dat(@"E:\Extracted\PathOfExile\3.25.Settlers\data\daemonspawningdata.dat64");
            var dsdSchema = s.tables["DaemonSpawningData"].columns;
            for (int i = 0; i < dsdSchema.Length; i++) {
                var column = dsdSchema[i];
                dsd.Column(column);
            }


            //Dat d = new Dat(@"E:\Extracted\PathOfExile\3.25.Settlers\data\chests.dat64");
            //var chestSchema = s.schema["Chests"];
            //for(int i = 0; i < chestSchema.Length; i++) {
            //    var column = chestSchema[i];
            //    Console.WriteLine();
            //    foreach (string str in d.Column(column)) {
            //        if(str == null) {
            //            Console.WriteLine($"{column.name} {column.type} {column.offset} {column.array}");
            //        }
            //    }
            //    Console.WriteLine();
            //}

            //return;
            //Schema.GqlReader reda = new Schema.GqlReader(File.ReadAllText(@"E:\Projects2\dat-schema\dat-schema\2_01_Talisman.gql"));
            //string t = reda.GetNextToken();
            //while (t != null) {
            //    Console.WriteLine(t);
            //    t = reda.GetNextToken();
            //}
            //return;

            //HashSet<string> banks = new HashSet<string>();
            //Database d = new Database(@"E:\Extracted\PathOfExile\3.25.Settlers\data");
            //var music = d.GetAll<Music>();
            //foreach (var track in music) banks.Add(track.bankFile);
            //foreach (string bank in banks) {
            //    if (bank == "") continue;
            //    Console.WriteLine(bank);
            //    File.Copy($@"E:\Extracted\PathOfExile\3.25.Settlers\FMOD\Desktop\{bank}", $@"E:\Extracted\PathOfExile\3.25.Settlers\MUSICBANKS\{bank}");
            //}
            //return;


            var types = Schema.SplitGqlTypes(@"E:\Projects2\dat-schema\dat-schema\3_25_Settlers_of_Kalguur.gql");
            foreach(string type in types.Keys) {
                Console.WriteLine("-------------------");
                Console.WriteLine(type);
                Console.WriteLine("-------------------");
                Console.WriteLine(types[type]);
            }
            return;

            Schema s2 = new Schema(@"E:\Projects2\dat-schema\dat-schema");
            return;

            Dat d2 = new Dat(@"E:\Extracted\PathOfExile\3.25.Settlers\data\villageupgrades.dat64");
            foreach (var column in s2.tables["VillageUpgrades"].columns) {
                Console.WriteLine();
                Console.WriteLine($"{column.name} {column.type} ({column.offset}):");
                var vals = d2.Column(column);
                for (int i = 0; i < Math.Min(vals.Length, 10); i++) {
                    Console.WriteLine(vals[i]);
                }
            }
            //return;

            //Schema s2 = new Schema(@"E:\Projects2\dat-schema\dat-schema\3_25_Settlers_of_Kalguur.gql");

            //return;


            //Schema s = new Schema(@"C:\Users\annal\Downloads\schema.min.json");
            //s.GenerateGetAllMethod(@"E:\Extracted\PathOfExile\3.25.Settlers\data");
            //return;

            //Bestiary.ListDatRowCounts(@"E:\Extracted\PathOfExile\3.25.Settlers\data"); return;
            //Dat dat = new Dat(@"E:\Extracted\PathOfExile\3.25.Settlers\data\betrayalchoiceactions.dat64");
            //var animation = d.GetAll<PoeFormats.Rows.Animation>();
            //var stat = d.Get<PoeFormats.Rows.Stat>(2930);
            //Console.WriteLine("BEGIN");
            //Database d = new Database(@"E:\Extracted\PathOfExile\3.25.Settlers\data");
            //var monsters = d.GetAll<PoeFormats.Rows.MonsterVariety>();
            //foreach (var monster in monsters) {
            //    Console.WriteLine($"{monster.name} {monster.id}");
            //}
            //MonsterType[] monsters = d.GetAll<MonsterType>();
            //Smd smd = new Smd(@"E:\Extracted\PathOfExile\3.25.Settlers\art\models\monsters\vaalzealots\vaalzealots01_armour_20402b82.smd");
            //SanctumRoomType[] roomTypes = d.GetAll<SanctumRoomType>();
            //Schema s = new Schema(@"C:\Users\annal\Downloads\schema.min.json");
            //s.GenerateCode(@"E:\Extracted\PathOfExile\3.25.Settlers\data");

            return;
            foreach (string aocpath in Directory.EnumerateFiles(@"E:\Extracted\PathOfExile\3.25.Settlers\metadata\monsters", "*.aoc", SearchOption.AllDirectories))
            {
                PoeTextFile aoc = new PoeTextFile(@"E:\Extracted\PathOfExile\3.25.Settlers\", aocpath);
                PoeTextFile.Block block = aoc.GetBlock("SkinMesh");
                string skin = null;
                for(int i = 0; i < block.keys.Count; i++) {
                    string key = block.keys[i];
                    if (key != "skin") {
                        //Console.WriteLine($"{aocpath}@{key}@{block.values[i]}");
                    } else {
                        if(skin != null) {
                            Console.WriteLine($"{aocpath}@{key}@{block.values[i]}");
                        }
                        skin = block.values[i];
                    }
                }
            }

            /*
            foreach(string fmtpath in Directory.EnumerateFiles(@"D:\Extracted\PathOfExile\3.25.Settlers\art\models\monsters", "*.fmt", SearchOption.AllDirectories)) {
                Fmt fmt = new Fmt(fmtpath);
                if(fmt.version >= 9 && fmt.modelVersion > 3)
                Console.WriteLine($"{fmt.version}|{fmt.modelVersion}|{fmtpath}");
            }
            
            //Fmt generator = new Fmt(@"D:\Extracted\PathOfExile\3.24.Necropolis\art\models\terrain\jungle\tiles\vaalinterior\vaalgenerator\vaalgenerator_generator.fmt");

            //Fmt well = new Fmt(@"D:\Extracted\PathOfExile\3.23.Affliction\art\models\terrain\doodads\leagues\hellscape\wells\hellswell01_1.fmt");


            //Mat mat = new Mat(@"D:\Extracted\PathOfExile\3.23.Affliction\art\models\monsters\fallengods\textures\fallengodse1c.mat");

            return;

            MatTests(); return;

            Ast golem = new Ast(@"D:\Extracted\PathOfExile\3.23.Affliction\art\models\monsters\flamegolem\rig.ast");

            
            Tgt tgt = new Tgt(@"D:\Extracted\PathOfExile\3.23.Affliction", @"art\models\terrain\islands\tiles\coraldungeon\thickwall\ccmm_01.tgt");
            for(int y = 0; y < tgt.sizeY; y++) {
                for(int x = 0; x < tgt.sizeX; x++) {
                    Console.WriteLine($"{x},{y}");
                    //foreach (string matname in tgt.GetSubtileMaterials(x, y)) Console.WriteLine(matname);
                    foreach (int i in tgt.GetCombinedShapeLengths(x, y)) Console.WriteLine(i);
                    Console.WriteLine();
                }
            }
            
            Tgm tgm = new Tgm(@"D:\Extracted\PathOfExile\3.23.Affliction\art\models\terrain\incadungeon\tgms\dungeon_abysswater_v01_01.tgm");


            //Tgt tgt = new Tgt(@"D:\Extracted\PathOfExile\3.23.Affliction\art\models\terrain\desert\stromatolite\alttiles\drysea\drysea_cc_01.tgt");
            return;



            Fmt chimera = new Fmt(@"D:\Extracted\PathOfExile\3.23.Affliction\art\models\monsters\chimeramonster\chimeramonster.fmt");
            using (TextWriter reader = new StreamWriter(File.Open("chimera.obj", FileMode.Create))) {
                for (int i = 0; i < chimera.meshes[0].vertCount; i++) {
                    reader.WriteLine($"v {chimera.meshes[0].verts[i * 3]} {chimera.meshes[0].verts[i * 3 + 1]} {chimera.meshes[0].verts[i * 3 + 2]}");
                }
                for (int i = 0; i < chimera.meshes[0].idx.Length; i += 3) {
                    reader.WriteLine($"f {chimera.meshes[0].idx[i] + 1} {chimera.meshes[0].idx[i + 1] + 1} {chimera.meshes[0].idx[i + 2] + 1}");
                }
            }
        

            return;

            foreach(string path in Directory.EnumerateFiles(@"D:\Extracted\PathOfExile\3.23.Affliction\metadata", "*.ao", SearchOption.AllDirectories)) {
                string file = path.Substring(@"D:\Extracted\PathOfExile\3.23.Affliction\".Length);
                PoeTextFile a = new PoeTextFile(@"D:\Extracted\PathOfExile\3.23.Affliction", file);
                var attachments = a.GetList("AttachedAnimatedObject", "attached_object");
                if(attachments.Count > 0) {
                    Console.WriteLine(file);
                    foreach (string attachment in attachments)
                        Console.WriteLine("    " + attachment);
                    Console.WriteLine();
                }
                //else Console.WriteLine(file);
            }
            return;

            PoeTextFile aoc = new PoeTextFile(@"D:\Extracted\PathOfExile\3.23.Affliction", @"Metadata/Monsters/LeagueSanctum/Boss/Lycia2Boss.aoc");

            //Smd lycia = new Smd(@"D:\Extracted\PathOfExile\3.23.Affliction\art\models\monsters\goddessofmalaise\rig_e4460e77.smd");
            //Fmt fmt = new Fmt(@"D:\Extracted\PathOfExile\3.23.Affliction\art\models\monsters\leagueheist\armoury\wingedspear.fmt");
            //Fmt fmt = new Fmt(@"D:\Extracted\PathOfExile\3.23.Affliction\art\models\items\weapons\onehandweapons\onehandswords\monsters\gargoylegolemredsword.fmt");
            //Fmt fmt = new Fmt(@"D:\Extracted\PathOfExile\3.23.Affliction\art\models\terrain\hideout\sunspire_doomguard\viennafountain_water.fmt");
            return;

            //PoeTextFile aoc = new PoeTextFile(@"D:\Extracted\PathOfExile\3.23.Affliction", @"metadata\monsters\leagueazmeri\crazedcannibalpicts\crazedcannibalpicts_female\dagger_dagger\crazedcannibalpicts_female_1reduced.aoc");

            Console.WriteLine("--------------------------");

            return;
            PoeTextFile.DumpTokens(@"D:\Extracted\PathOfExile\3.23.Affliction\metadata\parent.aoc");

            return;

            Smd smd = new Smd(@"D:\Extracted\PathOfExile\3.23.Affliction\art\models\monsters\reaperboss\rig_2eac045d.smd");
            Console.Write(smd.model.meshes[0].vertCount);

            //Mat mat = new Mat(@"D:\Extracted\PathOfExile\3.23.Affliction\art\textures\monsters\gargoylegolem\gargoylegolemredc.mat");
            //foreach(var g in mat.graphs) {
            //    Console.WriteLine(g.parent);
            //    if(g.baseTex != null) { Console.WriteLine(g.baseTex); }
            //}
            */
        }

        static void ListTimelineAnimations(string folder) {
            HashSet<string> animations = new HashSet<string>();

            foreach(string file in Directory.EnumerateFiles(folder, "*.atl", SearchOption.AllDirectories)) {
                var j = JObject.Parse(File.ReadAllText(file));
                if (j["animations"] == null) continue;
                foreach (var animation in j["animations"].Children()) {
                    string anim = animation["view"] == null ? animation["name"].Value<string>() : animation["view"]["of"].Value<string>();
                    if (!animations.Contains(anim)) {
                        Console.WriteLine($"{file} {anim}");
                        animations.Add(anim);
                    }
                }
                //break;
            }
        }

        static void FindHiddenFilesInTDT(string folder) {
            foreach (string path in Directory.EnumerateFiles(folder, "*.tdt", SearchOption.AllDirectories)) {
                Tdt tdt = new Tdt(path);
                for (int i = 0; i < tdt.strings.Count; i++) {
                    string s = tdt.strings[i];
                    if (s.IndexOf('.') == -1) continue;
                    if(!File.Exists(Path.Combine(folder, s))) Console.WriteLine(s);
                }
            }
        }

        static void DatCompare(string folderNew, string folderOld) {
            foreach (string path in Directory.EnumerateFiles(folderNew, "*.datc64")) {
                string filename = Path.GetFileName(path);
                string oldPath = Path.Combine(folderOld, filename);
                if (!File.Exists(oldPath)) Console.WriteLine("NEW DAT " + filename);
                else {
                    Dat newDat = new Dat(path);
                    Dat oldDat = new Dat(oldPath);

                    if (newDat.rowWidth != oldDat.rowWidth) Console.WriteLine($"WIDTH {filename} {oldDat.rowWidth} -> {newDat.rowWidth} ({newDat.rowWidth - oldDat.rowWidth})");
                    if (newDat.rowCount != oldDat.rowCount) Console.WriteLine($"COUNT {filename} {oldDat.rowCount} -> {newDat.rowCount} ({newDat.rowCount - oldDat.rowCount})");

                }
            }

        }


        static void DatStringDump(string schemaFolder, string datFolder, bool matchCase = false, params string[] searches) {
            Schema s = new Schema(schemaFolder);
            foreach(string datPath in Directory.EnumerateFiles(datFolder, "*.datc64")) {
                if (s.TryGetTable(Path.GetFileNameWithoutExtension(datPath), out var table)) {
                    Dat d = new Dat(datPath);
                    for (int i = 0; i < table.columns.Length; i++) {
                        var col = table.columns[i];
                        if (col.type != Schema.Column.Type.@string) continue;
                        DatAnalysis.Error error = DatAnalysis.AnalyseColumn(d, col, int.MaxValue);
                        if (error != DatAnalysis.Error.NONE) continue;
                        var values = d.Column(col);
                        for (int row = 0; row < values.Length; row++) {
                            string str = matchCase ? values[row] : values[row].ToLower();
                            for (int searchVal = 0; searchVal < searches.Length; searchVal++) {
                                if (str.Contains(searches[searchVal])) {
                                    Console.WriteLine($"{table.name}|{row}|{values[row]}");
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }



        static void MatTests() {
            string basicColour = "Metadata/Effects/Graphs/General/BasicColour.fxgraph";
            string basicColourNormalSpec = "Metadata/Effects/Graphs/General/BasicColourNormalSpec.fxgraph";

            string[] baseGraphs = new string[] {
                "Metadata/Materials/DielectricSpecGloss.fxgraph",
                "Metadata/Materials/DielectricSpecGlossBN.fxgraph",
                "Metadata/Materials/SpecGlossMetal.fxgraph",
                "Metadata/Materials/AnisotropicSpecGloss.fxgraph",
                "Metadata/Materials/MetalRough.fxgraph",
                "Metadata/Materials/MetalRoughBN.fxgraph",
                "Metadata/Materials/SpecGlossSpecMaskOpaque.fxgraph",
                "Metadata/Materials/SpecGlossSpecMaskOpaqueBN.fxgraph",
                "Metadata/Materials/SpecGloss.fxgraph",
                "Metadata/Materials/SpecGlossSpecMask.fxgraph"
            };

            string[] baseGraphs2 = new string[] {
                "Metadata/Effects/Graphs/General/BasicColour.fxgraph",
                "Metadata/Effects/Graphs/General/BasicColourNormalSpec.fxgraph",
                "Metadata/Materials/DielectricSpecGloss.fxgraph",
                "Metadata/Materials/DielectricSpecGlossBN.fxgraph",
                "Metadata/Materials/SpecGlossMetal.fxgraph",
                "Metadata/Materials/AnisotropicSpecGloss.fxgraph",
                "Metadata/Materials/MetalRough.fxgraph",
                "Metadata/Materials/MetalRoughBN.fxgraph",
                "Metadata/Materials/SpecGlossSpecMaskOpaque.fxgraph",
                "Metadata/Materials/SpecGlossSpecMaskOpaqueBN.fxgraph",
                "Metadata/Materials/SpecGloss.fxgraph",
                "Metadata/Materials/SpecGlossSpecMask.fxgraph"
            };


            int matCount = 0;
            Dictionary<string, int> graphCounts = new Dictionary<string, int>();

            Dictionary<string, HashSet<string>> graphParams = new Dictionary<string, HashSet<string>>();

            foreach (string line in File.ReadAllLines(@"E:\A\A\Visual Studio\Archbestiary\bin\Debug\net6.0\monstermaterials.txt")) {
                string path = Path.Combine(@"D:\Extracted\PathOfExile\3.23.Affliction", line);
                Mat mat = new Mat(path);
                List<string> foundBaseGraphs = new List<string>();

                string baseGraph = null;


                foreach (Mat.GraphInstance graph in mat.graphs) {
                    if (baseGraphs2.Contains(graph.parent)) {
                        if (!graphParams.ContainsKey(graph.parent)) graphParams[graph.parent] = new HashSet<string>();
                        foreach (string param in graph.parameters.Keys) {
                            graphParams[graph.parent].Add(param);
                        }
                    }
                }
                /*
                foreach (Mat.GraphInstance graph in mat.graphs) {
                    if(baseGraphs.Contains(graph.parent)) {
                        baseGraph = graph.parent;
                        break;
                    } else if (graph.parent == basicColourNormalSpec && (baseGraph == null || baseGraph == basicColour)) {
                        baseGraph = basicColourNormalSpec;
                    } else if (graph.parent == basicColour || baseGraph == null) {
                        baseGraph = basicColour;
                    }
                }

                if (baseGraph != null) {
                    if (!graphCounts.ContainsKey(baseGraph)) { graphCounts[baseGraph] = 1;
                    else graphCounts[baseGraph]++;
                }


                if (foundBaseGraphs.Count > 1) {
                    Console.WriteLine(path);
                    foreach (string parent in foundBaseGraphs)
                        Console.WriteLine(parent);
                    Console.WriteLine();
                }



                if(!foundBaseGraph) {
                    Console.WriteLine(path);
                    foreach (Mat.GraphInstance graph in mat.graphs) {
                        Console.WriteLine(graph.parent);
                        //if (!graphCounts.ContainsKey(graph.parent)) graphCounts[graph.parent] = 1;
                        //else graphCounts[graph.parent]++;
                    }
                    Console.WriteLine();
                }
                    */
            }


            foreach (string graph in graphCounts.Keys) {
                Console.WriteLine($"{graph}|{graphCounts[graph]}");
            }
            foreach (string graph in graphParams.Keys) {
                Console.WriteLine(graph);
                foreach (string param in graphParams[graph]) { Console.WriteLine(param); }
                Console.WriteLine();

            }

            //Mat mat = new Mat(@"D:\Extracted\PathOfExile\3.23.Affliction\art\textures\monsters\gargoylegolem\gargoylegolemredc.mat");
        }
    }
}