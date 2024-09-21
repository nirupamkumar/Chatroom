using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using Unity.VisualScripting;
using Mono.Cecil;
using System.Reflection;
using System;

public class CodeGenerator : MonoBehaviour
{ 
    [MenuItem("CustomSerializer/Generate Serializer")]
    public static void GenerateSerializer()
    {
        MonoScript classToSerialize = (MonoScript)Resources.LoadAll("ClassToDeSerialize")[0];

        using (FileStream stream = new FileStream ($"{Application.dataPath}/Scripts/Serializer.cs", FileMode.OpenOrCreate, FileAccess.ReadWrite))
        {
            using (StreamWriter writer = new StreamWriter(stream))
            {
                // create your serializer here
                writer.WriteLine("using System.IO;");
                writer.WriteLine("using System.Collections.Generic;");
                writer.WriteLine("using UnityEngine;");
                writer.WriteLine();
                writer.WriteLine();
                writer.WriteLine("public class Serializer");
                writer.WriteLine("{");
                writer.WriteLine("\tpublic void Serialize(object obj, BinaryWriter writer)");
                writer.WriteLine("\t{");

                SerializeFields(classToSerialize.GetClass(), writer);

                writer.WriteLine("\t}");
                writer.WriteLine();
                writer.WriteLine("\tpublic object Deserializer(BinaryReader reader)");
                writer.WriteLine("\t{");
                writer.WriteLine("\t\tobject obj = new object();");
                writer.WriteLine();
                writer.WriteLine("\t\tDeserializeFields(ref obj, reader);");
                writer.WriteLine("\t\treturn obj;");
                writer.WriteLine("\t}");

                writer.WriteLine();
                writer.WriteLine("\tprivate void SerializeFields(object obj, BinaryWriter writer)");
                writer.WriteLine("\t{");
                //SerializeFieldsRecursive(classToSerialize.GetClass(), "obj", writer);
                writer.WriteLine("\t}");

                writer.WriteLine();
                writer.WriteLine("\tprivate void DeserializeFields(ref object obj, BinaryReader reader)");
                writer.WriteLine("\t{");
                //DeserializeFieldsRecursive(classToSerialize.GetClass(), "ref obj", writer);
                writer.WriteLine("\t}");

                writer.WriteLine("}");
                AssetDatabase.Refresh();
            }
        }

    }

    private static void SerializeFields(Type type, StreamWriter streamWriter)
    {
        foreach (var field in type.GetFields())
        {
            string fieldName = field.Name;
            string fieldType = field.FieldType.FullName;
            streamWriter.WriteLine($"\t\twriter.Write(({fieldType})obj.{fieldName});");
        }
    }


}
