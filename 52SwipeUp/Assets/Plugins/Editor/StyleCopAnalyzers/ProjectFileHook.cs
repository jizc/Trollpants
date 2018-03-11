using System.IO;
using System.Text;
using System.Xml.Linq;
using SyntaxTree.VisualStudio.Unity.Bridge;
using UnityEditor;

[InitializeOnLoad]
public class ProjectFileHook
{
    private const string basePath = @"Assets\Plugins\Editor\StyleCopAnalyzers\";
    private const string ruleSet = "WitchWing.ruleset";

    static ProjectFileHook()
    {
        ProjectFilesGenerator.ProjectFileGeneration -= OnProjectFileGeneration;
        ProjectFilesGenerator.ProjectFileGeneration += OnProjectFileGeneration;
    }

    private static string OnProjectFileGeneration(string name, string content)
    {
        var document = XDocument.Parse(content);
        if (document.Root != null)
        {
            var ie = document.Root.Elements("{http://schemas.microsoft.com/developer/msbuild/2003}PropertyGroup").GetEnumerator();
            while (ie.MoveNext())
            {
                if (ie.Current == null || ie.Current.FirstAttribute == null)
                {
                    continue;
                }

                if (!ie.Current.FirstAttribute.Value.Contains("Debug|AnyCPU"))
                {
                    continue;
                }

                ie.Current.Add(
                    new XElement(
                        "{http://schemas.microsoft.com/developer/msbuild/2003}CodeAnalysisRuleSet",
                        basePath + ruleSet));
            }

            ie.Dispose();
            ie = document.Root.Elements("{http://schemas.microsoft.com/developer/msbuild/2003}ItemGroup").GetEnumerator();
            var insertSuccessful = false;
            while (ie.MoveNext())
            {
                if (ie.Current == null || ie.Current.FirstNode == null)
                {
                    continue;
                }

                var element = ie.Current.FirstNode as XElement;
                if (element != null && element.Name != "{http://schemas.microsoft.com/developer/msbuild/2003}None")
                {
                    continue;
                }

                ie.Current.Add(GetSettingsConfigElement());
                insertSuccessful = true;
            }

            ie.Dispose();
            if (!insertSuccessful)
            {
                document.Root.Add(GetConfigItemGroup());
            }

            document.Root.Add(GetAnalyzerItemGroup());
            document.Root.Add(GetDisableAnalysersTarget());
        }

        string project;
        using (var str = new Utf8StringWriter())
        {
            document.Save(str);
            project = str.ToString();
        }

        return project;
    }

    private static XElement GetConfigItemGroup()
    {
        var newItemGroup = new XElement("{http://schemas.microsoft.com/developer/msbuild/2003}ItemGroup");
        newItemGroup.Add(GetSettingsConfigElement());
        return newItemGroup;
    }

    private static XElement GetSettingsConfigElement()
    {
        var settingsConfig = new XElement("{http://schemas.microsoft.com/developer/msbuild/2003}AdditionalFiles");
        settingsConfig.SetAttributeValue("Include", basePath + "stylecop.json");
        return settingsConfig;
    }

    private static XElement GetAnalyzerItemGroup()
    {
        var newItemGroup = new XElement("{http://schemas.microsoft.com/developer/msbuild/2003}ItemGroup");
        var analyzer1 = new XElement("{http://schemas.microsoft.com/developer/msbuild/2003}Analyzer");
        analyzer1.SetAttributeValue(
            "Include",
            basePath + "StyleCop.Analyzers.CodeFixes.dll");
        var analyzer2 = new XElement("{http://schemas.microsoft.com/developer/msbuild/2003}Analyzer");
        analyzer2.SetAttributeValue(
            "Include",
            basePath + "StyleCop.Analyzers.dll");
        newItemGroup.Add(analyzer1);
        newItemGroup.Add(analyzer2);
        return newItemGroup;
    }

    private static XElement GetDisableAnalysersTarget()
    {
        var disableAnalyzersTarget = new XElement("{http://schemas.microsoft.com/developer/msbuild/2003}Target");
        disableAnalyzersTarget.SetAttributeValue("Name", "DisableAnalyzersForVisualStudioBuild");
        disableAnalyzersTarget.SetAttributeValue("BeforeTargets", "CoreCompile");
        disableAnalyzersTarget.SetAttributeValue("Condition", "'$(BuildingInsideVisualStudio)' == 'True' And '$(BuildingProject)' == 'True'");
        var itemGroup = new XElement("{http://schemas.microsoft.com/developer/msbuild/2003}ItemGroup");
        var analyzer = new XElement("{http://schemas.microsoft.com/developer/msbuild/2003}Analyzer");
        analyzer.SetAttributeValue("Remove", "@(Analyzer)");
        itemGroup.Add(analyzer);
        disableAnalyzersTarget.Add(itemGroup);
        return disableAnalyzersTarget;
    }

    private class Utf8StringWriter : StringWriter
    {
        public override Encoding Encoding
        {
            get { return Encoding.UTF8; }
        }
    }
}
