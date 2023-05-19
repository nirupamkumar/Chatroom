using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using Unity.VisualScripting;
using Mono.Cecil;
using System.Reflection;

public class CodeGenerator : MonoBehaviour
{ 
    [MenuItem("CustomSerializer/Generate Serializer")]
    public static void GenerateSerializer()
    {
        MonoScript classToSerialize = (MonoScript)Resources.LoadAll("ClassToDeSerialize")[0];

        using (FileStream stream = new FileStream ($"{Application.dataPath}/Scripts/DemoSerializer.cs", FileMode.OpenOrCreate, FileAccess.ReadWrite))
        {
            using (StreamWriter writer = new StreamWriter(stream))
            {
                // create your serializer here
                writer.WriteLine("using System.IO;");
                writer.WriteLine("using System.Collections.Generic;");
                writer.WriteLine("using UnityEngine;");
                writer.WriteLine();
                writer.WriteLine();
                writer.WriteLine("public class DemoSerializer");
                writer.WriteLine("{");
                writer.WriteLine("\tpublic void Display()");
                writer.WriteLine("\t{");

                foreach (FieldInfo fi in classToSerialize.GetClass().GetFields())
                {
                    writer.WriteLine($"\t\tDebug.Log(\"{fi.Name}\");");
                }

                writer.WriteLine("\t}");
                writer.WriteLine("}");


                AssetDatabase.Refresh();
            }
        }

    }
    
   
}
