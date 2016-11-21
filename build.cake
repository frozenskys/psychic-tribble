///////////////////////////////////////////////////////////////////////////////
// Tools and Addins
///////////////////////////////////////////////////////////////////////////////
#tool nuget:?package=GitVersion.CommandLine
#tool nuget:?package=NUnit.ConsoleRunner
#tool nuget:?package=NUnit.Extension.NUnitV2ResultWriter
#addin nuget:?package=Cake.Git
#addin nuget:?package=Cake.Figlet

///////////////////////////////////////////////////////////////////////////////
// ARGUMENTS
///////////////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");
var versionType = Argument("VersionType", "patch");
var artifacts = MakeAbsolute(Directory(Argument("artifactPath", "./artifacts")));
var buildFolder = MakeAbsolute(Directory(Argument("buildFolder", "./build"))).ToString();
GitVersion versionInfo = null;
var testResultsFilePath = artifacts + "/TestResult.xml";
var testOutputFile = File(testResultsFilePath);

///////////////////////////////////////////////////////////////////////////////
// SETUP / TEARDOWN
///////////////////////////////////////////////////////////////////////////////

Setup(ctx =>
{
	// Executed BEFORE the first task.
	Information(Figlet("Tribble Cake"));
});

Teardown(ctx =>
{
	// Executed AFTER the last task.
	Information("Finished running tasks.");
});

///////////////////////////////////////////////////////////////////////////////
// SYSTEM TASKS
///////////////////////////////////////////////////////////////////////////////

Task("Version")
	.Does(() =>
	{
		var semVersion = "";
		int major = 0;
		int minor = 1;
		int patch = 0;
		GitVersion assertedVersions = GitVersion(new GitVersionSettings
		{
			OutputType = GitVersionOutput.Json,
		});
		major = assertedVersions.Major;
		minor = assertedVersions.Minor;
		patch = assertedVersions.Patch;
		switch (versionType.ToLower())
		{
			case "patch":
				patch += 1; break;
			case "minor":
				minor += 1; patch = 0; break;
			case "major":
				major += 1;	minor = 0; patch = 0; break;			
		};
		semVersion = string.Format("{0}.{1}.{2}", major, minor, patch);
		GitTag(".", semVersion);
		Information("Changing version: {0} to {1}", assertedVersions.LegacySemVerPadded, semVersion);
	});

///////////////////////////////////////////////////////////////////////////////
// USER TASKS
// PUT ALL YOUR BUILD GOODNESS IN HERE
///////////////////////////////////////////////////////////////////////////////

Task("Clean")
    .Does(() =>
	{
		CleanDirectory(artifacts);
		CleanDirectory(buildFolder);
	});

Task("Default")
    .IsDependentOn("Test")
    .Does(() => 
	{
	});

Task("Build")
	.Does(() =>
	{
		NuGetRestore("./src/Psychic-Tribble/Psychic-Tribble.sln");
		MSBuild("./src/Psychic-Tribble/Psychic-Tribble.sln", settings => settings
				.WithProperty("TreatWarningsAsErrors","true")
				.WithProperty("UseSharedCompilation", "false")
				.WithProperty("AutoParameterizationWebConfigConnectionStrings", "false")
				.WithProperty("OutDir", buildFolder)
				.SetVerbosity(Verbosity.Minimal)
				.SetConfiguration(configuration)
				.WithTarget("Rebuild"));
	});

Task("Test")
	.IsDependentOn("Build")
	.Does(() =>
	{ 
		EnsureDirectoryExists(artifacts);
		NUnit3(buildFolder + "/**/*ClientTests.dll", 
            new NUnit3Settings {
                Results = testOutputFile, 
                ResultFormat = "nunit2"
            });
		NUnit3(buildFolder + "/**/*ServerTests.dll", 
            new NUnit3Settings {
                Results = testOutputFile, 
                ResultFormat = "nunit2"
            });
	});

Task("Package")
	.Does(() =>
	{
		Information("Running Packaging...");
	});

RunTarget(target);