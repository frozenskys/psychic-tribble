///////////////////////////////////////////////////////////////////////////////
// Tools and Addins
///////////////////////////////////////////////////////////////////////////////
#tool "GitVersion.CommandLine"
#addin nuget:?package=Cake.Git
#addin nuget:?package=Cake.Figlet

///////////////////////////////////////////////////////////////////////////////
// ARGUMENTS
///////////////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");
var versionType = Argument("VersionType", "patch");
var artifacts = MakeAbsolute(Directory(Argument("artifactPath", "./artifacts")));

GitVersion versionInfo = null;

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
	});

Task("Default")
    .IsDependentOn("Test")
    .Does(() => 
	{
	});

Task("Build")
	.Does(() =>
	{
		NuGetRestore("./src/Legacy/Legacy.sln");
		var settings = new MSBuildSettings { Configuration = configuration, Verbosity = Verbosity.Minimal };

		if(IsRunningOnWindows())
		{
		MSBuild("./src/Psychic-Tribble/Psychic-Tribble.sln", settings);
		}
	});

Task("Test")
	.IsDependentOn("Build")
	.Does(() =>
	{ 
		NUnit3(buildFolder + "/**/*.Tests.dll", 
            new NUnit3Settings {
                Results = TestOuput, 
                ResultFormat = "nunit2"
            });
	});

Task("Package")
	.Does(() =>
	{
		Information("Running Packaging...");
	});

RunTarget(target);