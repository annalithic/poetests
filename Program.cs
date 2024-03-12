using PoeFormats;

namespace PoeTests {
    internal class Program {
        static void Main(string[] args) {
            foreach(string path in Directory.EnumerateFiles(@"D:\Extracted\PathOfExile\3.23.Affliction\metadata", "*.ao", SearchOption.AllDirectories)) {
                string file = path.Substring(@"D:\Extracted\PathOfExile\3.23.Affliction\".Length);
                PoeTextFile a = new PoeTextFile(@"D:\Extracted\PathOfExile\3.23.Affliction", file);
                if (a.blocks.ContainsKey("AttachedAnimatedObject")) Console.WriteLine(file + " HAS ATTACHED OBJECTS");
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

            Mat mat = new Mat(@"D:\Extracted\PathOfExile\3.23.Affliction\art\textures\monsters\gargoylegolem\gargoylegolemredc.mat");
            foreach(var g in mat.graphs) {
                Console.WriteLine(g.parent);
                if(g.baseTex != null) { Console.WriteLine(g.baseTex); }
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
                        foreach (string param in graph.parameters) {
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