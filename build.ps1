$msbuild = "C:\Windows\Microsoft.NET\Framework\v4.0.30319\msbuild.EXE"
$nugetPath = ".nuget\Nuget.exe"

$version = "0.1"

if($env:PackageVersion -ne $null){
	$version = $env:PackageVersion
}

Write-Host "Building Version $version"

Invoke-Expression "$msbuild Swerl.Referee.Core/Swerl.Referee.Core.csproj /p:Configuration=Release /p:Platform=AnyCPU /t:build /p:OutputPath=../build/Swerl.Referee.Core/net45 /v:n /nologo"
Invoke-Expression "$msbuild Swerl.Referee.Core/Swerl.Referee.Core.csproj /p:Configuration=Release-Net40 /p:Platform=AnyCPU /t:build /p:OutputPath=../build/Swerl.Referee.Core/net40 /v:n /nologo"

Invoke-Expression "$msbuild Swerl.Referee.MVC/Swerl.Referee.MVC.csproj /p:Configuration=Release /p:Platform=AnyCPU /t:build /p:OutputPath=../build/Swerl.Referee.MVC/net45 /v:n /nologo"
Invoke-Expression "$msbuild Swerl.Referee.MVC/Swerl.Referee.MVC.csproj /p:Configuration=Release-Net40 /p:Platform=AnyCPU /t:build /p:OutputPath=../build/Swerl.Referee.MVC/net40 /v:n /nologo"

Invoke-Expression  "$nugetPath pack Swerl.Referee.Core/Swerl.Referee.Core.nuspec -OutputDirectory build/Swerl.Referee.Core -Version $version"
Invoke-Expression  "$nugetPath pack Swerl.Referee.MVC/Swerl.Referee.MVC.nuspec -OutputDirectory build/Swerl.Referee.MVC -Version $version"
