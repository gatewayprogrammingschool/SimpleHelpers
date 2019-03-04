function Push-Package
{
    param(
        [Parameter(Mandatory=$true)][string]$SolutionDirectory,
        [Parameter(Mandatory=$false)][string]$Config = "Debug",
        [Parameter(Mandatory=$false)][string]$Repository = "http://localhost:81/nuget/GPS",
        [Parameter(Mandatory=$false)][string]$ApiKey = $env:LocalhostNugetAPIKey
    )

    #$SolutionName = (Get-ChildItem $SolutionDirectory"/*.sln" | Get-Item).Name

    $OutputPath = [System.IO.Path]::Combine($SolutionDirectory, "Outbound")
    $ProjectPath = [System.IO.Path]::Combine($SolutionDirectory, "GPS.SimpleHelpers", "GPS.SimpleHelpers.csproj")
    $NuSpecPath = [System.IO.Path]::Combine($SolutionDirectory, "GPS.SimpleHelpers", "GPS.SimpleHelpers.nuspec")

    Write-Host $OutputPath

    Get-Item $OutputPath -ErrorAction Stop | Remove-Item -Force -Recurse
    New-Item -Path $SolutionDirectory -Name "Outbound" -ItemType "directory"

    dotnet build $SolutionDirectory -c $Config

    Write-Host $ProjectPath

    nuget pack $NuSpecPath -OutputDirectory $OutputPath -Properties Configuration=$Config

	if($LASTEXITCODE -eq 0) {
        $Packages = Get-Item $OutputPath -ErrorAction Stop | Get-ChildItem

        $Packages | ForEach-Object -Process {
            $Command = "dotnet nuget push " + $_.FullName + " -s " + $Repository + " -k " + $ApiKey
            # Write-Host $Command
            Invoke-Expression $Command
        }
	}
}

Clear-Host

#Deploy-Package -SolutionDir %1 -BuildDir %2 -Namespace %3 -Assembly %4
Push-Package -SolutionDir 'C:\GitHub\SimpleHelpers\GPS.SimpleHelpers' -Repository "http://source.marcumllp.com:81/nuget/Default" -ApiKey "pbyrd:pbyrd"
